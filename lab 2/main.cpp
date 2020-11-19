#include <iostream>
#include <zconf.h>
#include <vector>
#include <list>
#include <map>

#define AMOUNT_OF_PAGES 10
#define PAGE_SIZE 1024

using namespace std;


struct header_r {
    size_t size;
    bool is_free;
    void* start;
    void* end;
};


struct page{
    void* startPageAddr;
    bool availability;
    int number_of_blocks;

    bool operator< (const page &d2) const
    {
        return this->startPageAddr < d2.startPageAddr;
    }
};


static vector<page> allPages(AMOUNT_OF_PAGES);
//static vector<vector<header_r>> blocks;
static map<page, vector<header_r>> pageMap;

void pageInit(){
    void* heapPosition = sbrk(PAGE_SIZE);
    for (int i = 0;i < AMOUNT_OF_PAGES;i++){
        allPages[i].availability = true;
        allPages[i].startPageAddr = heapPosition;
        allPages[i].number_of_blocks = NULL;
        heapPosition = sbrk(PAGE_SIZE);
    }

    vector<header_r> init_blocks(0);

    for (int i = 0; i < AMOUNT_OF_PAGES; i++) {
        pageMap.insert(pair<page,vector<header_r>>(allPages[i], init_blocks));
    }
}


int round_up(size_t size){
    for (int i = size; i <= PAGE_SIZE/2; ++i) {
        if ((double)((int)log2(i)) == log2(i)){
            return i;
        }
    }
    return 0;
}

page get_free_page(int block_class){
    for (int i = 0;i < AMOUNT_OF_PAGES;i++){
        if(allPages[i].availability == true){
            if (allPages[i].number_of_blocks == NULL || allPages[i].number_of_blocks == block_class ){
                allPages[i].number_of_blocks = block_class;
                return allPages[i];
            }
        }
    }
}

page& get_full_page_for_alloc(){
    for (int i = 0;i < AMOUNT_OF_PAGES;i++){
        if(allPages[i].availability == true && allPages[i].number_of_blocks == 0){
            return allPages[i];
        }
    }
};

void page_availability_check(){
    for (int i = 0; i < AMOUNT_OF_PAGES; ++i) {
        if (allPages[i].number_of_blocks == pageMap[allPages[i]].size() && allPages[i].number_of_blocks != 0) allPages[i].availability = false;
    }
}

void* page_malloc(size_t size){
    page_availability_check();
    size_t total_size;
    header_r block;
    void* page_addr;
    page free_page;

    if (!size)
        return NULL;



    total_size = size + sizeof(struct header_r);

    if(total_size > (PAGE_SIZE / 2) && total_size < PAGE_SIZE){
        page* page_t = &get_full_page_for_alloc();
        page_t->number_of_blocks = 1;
        page_t->availability = false;
        vector<header_r> blocks_in_page = pageMap[*page_t];
        block.size = size;
        block.is_free = false;
        block.start = page_t->startPageAddr;
        block.end = (uint8_t*)page_t->startPageAddr + PAGE_SIZE;
        blocks_in_page.push_back(block);
        pageMap[*page_t] = blocks_in_page;
        return page_t->startPageAddr;
    }

    if (total_size > PAGE_SIZE){
        int num_of_pages = total_size / PAGE_SIZE;
        int remainder = total_size % PAGE_SIZE;
        page* page_t;

        for (int i = 0; i < num_of_pages; ++i) {
            page_t = &get_full_page_for_alloc();
            page_t->number_of_blocks = 1;
            page_t->availability = false;
            vector<header_r> blocks_in_page = pageMap[*page_t];
            block.size = PAGE_SIZE;
            block.is_free = false;
            block.start = page_t->startPageAddr;
            block.end = (uint8_t*)page_t->startPageAddr + PAGE_SIZE;
            blocks_in_page.push_back(block);
            pageMap[*page_t] = blocks_in_page;
        }

        page* page_r = &get_full_page_for_alloc();
        page_r->number_of_blocks = 1;
        page_r->availability = false;
        vector<header_r> blocks_in_page = pageMap[*page_t];
        block.size = remainder;
        block.is_free = false;
        block.start = page_r->startPageAddr;
        block.end = (uint8_t*)page_r->startPageAddr + PAGE_SIZE;
        blocks_in_page.push_back(block);
        pageMap[*page_t] = blocks_in_page;
        return page_r->startPageAddr;
        }


    size_t block_size = round_up(total_size);
    int block_amount = PAGE_SIZE / block_size;
    free_page = get_free_page(block_amount);
    page_addr = free_page.startPageAddr;


    vector<header_r> blocks_in_page = pageMap[free_page];

    if (blocks_in_page.size() == 0) {
        block.size = size;
        block.is_free = false;
        block.start = page_addr;
        block.end = (uint8_t*)page_addr + block_size;
        blocks_in_page.push_back(block);
        pageMap[free_page] = blocks_in_page;
        return block.start;
    } else {
        for (int i = 0; i < blocks_in_page.size(); ++i) {
            if(blocks_in_page[i].is_free && blocks_in_page[i].size >= size){
                blocks_in_page[i].is_free = false;
                pageMap[free_page] = blocks_in_page;
                return blocks_in_page[i].start;
            }
        }
    }

    block.size = size;
    block.is_free = false;
    block.start = blocks_in_page.back().end;
    block.end = (uint8_t*)blocks_in_page.back().end + block_size;
    blocks_in_page.push_back(block);
    pageMap[free_page] = blocks_in_page;
    return block.start;
}

void page_free(void* addr){

    header_r *block;
    bool last_block;
    page* page_t;
    for (int i = 0; i < AMOUNT_OF_PAGES; ++i) {
        for (int j = 0; j < pageMap[allPages[i]].size(); ++j) {
            if (pageMap[allPages[i]][j].start == addr){
                block = &pageMap[allPages[i]][j];
                if (pageMap[allPages[i]].size() == 1) {
                    last_block = true;
                    page_t = &allPages[i];
                }
            }
        }
    }

    page_t->number_of_blocks = NULL;
    block->is_free = true;

}

void *realloc(void *addr, size_t size)
{
    header_r *block;
    void *ret;

    if (!block || !size)
        return page_malloc(size);

    for (int i = 0; i < AMOUNT_OF_PAGES; ++i) {
        for (int j = 0; j < pageMap[allPages[i]].size(); ++j) {
            if (pageMap[allPages[i]][j].start == addr){
                block = &pageMap[allPages[i]][j];
            }
        }
    }

    if (block->size >= size)
        return block->start;

    ret = page_malloc(size);

    if (ret) {
        memcpy(ret, block, block->size);
        free(block);
    }
    return ret;
}


void mem_dump(){
    
    for (int i = 0;i < AMOUNT_OF_PAGES;i++){
        cout<<"Page number: "<<i+1;
        cout<<"\tPage availability: "<<allPages[i].availability;
        cout<<"\tNumber Of Blocks: "<<allPages[i].number_of_blocks;
        cout<<"\tPage address: "<<allPages[i].startPageAddr<<endl;
        for (int j = 0; j < pageMap[allPages[i]].size(); ++j) {
            cout<<"\tBlock number: "<<j+1;
            cout<<"\tBlock availability: "<<pageMap[allPages[i]][j].is_free;
            cout<<"\tData size: "<<pageMap[allPages[i]][j].size;
            cout<<"\tBlock address: "<<pageMap[allPages[i]][j].start<<endl;
        }
    }
}



int main() {


    pageInit();
/*    page_malloc(10);


    void* test = page_malloc(33);*/

    page_malloc(69);
/*    page_malloc(100);
    page_malloc(100);
    page_malloc(100);
    page_malloc(100);
    page_malloc(100);*/
    page_malloc(560);
    page_malloc(2300);

    mem_dump();
/*    page_free(test);
    cout<<"---------------------------------------------------------------------------------------"<<endl;
    mem_dump();
    cout<<"---------------------------------------------------------------------------------------"<<endl;
    page_malloc(33);
    mem_dump();*/




/*    map<page, vector<header_r>> pageMap1;
    vector<header_r> init_blocks(0);

    cout<<init_blocks.size();
    page p;
    p.number_of_blocks = 12;
    p.startPageAddr = 0x0;
    p.availability = true;

    allPages[0].number_of_blocks = 12;
    allPages[0].startPageAddr = 0x0;
    allPages[0].availability = true;

    pageMap1.insert(pair<page,vector<header_r>>(p, init_blocks));*/



    return 0;
}