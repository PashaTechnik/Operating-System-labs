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
    int numberFreeBlocks;

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
        allPages[i].numberFreeBlocks = NULL;
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
            if (allPages[i].numberFreeBlocks == NULL || allPages[i].numberFreeBlocks == block_class ){
                allPages[i].numberFreeBlocks = block_class;
                return allPages[i];
            }
        }
    }
}

void* page_malloc(size_t size){
    size_t total_size;
    header_r block;
    void* page_addr;
    page free_page;

    if (!size)
        return NULL;

    total_size = size + sizeof(struct header_r);
    size_t block_size = round_up(total_size);
    int block_amount = PAGE_SIZE/block_size;
    free_page = get_free_page(block_amount);
    page_addr = free_page.startPageAddr;

    vector<header_r> blocks_in_page = pageMap[free_page];

    if (blocks_in_page.size() == 0) {
        block.size = size;
        block.is_free = false;
        block.start = page_addr;
        block.end = (uint8_t*)page_addr + block_size;
    } else {
        block.size = size;
        block.is_free = false;
        block.start = blocks_in_page.back().end;
        block.end = (uint8_t*)blocks_in_page.back().end + block_size;
    }
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

    page_t->numberFreeBlocks = NULL;
    block->is_free = true;

}

void mem_dump(){
    
    for (int i = 0;i < AMOUNT_OF_PAGES;i++){
        cout<<"Page number: "<<i+1;
        cout<<"\tPage availability: "<<allPages[i].availability;
        cout<<"\tNumber Of FreeBlocks: "<<allPages[i].numberFreeBlocks;
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
    page_malloc(10);
    page_malloc(20);
    page_malloc(20);
    page_malloc(20);
    page_malloc(20);

    void* test = page_malloc(33);

    page_malloc(69);
    page_malloc(100);

    mem_dump();
    page_free(test);
    cout<<"------------------------------------------------------"<<endl;
    mem_dump();


/*    map<page, vector<header_r>> pageMap1;
    vector<header_r> init_blocks(0);

    cout<<init_blocks.size();
    page p;
    p.numberFreeBlocks = 12;
    p.startPageAddr = 0x0;
    p.availability = true;

    allPages[0].numberFreeBlocks = 12;
    allPages[0].startPageAddr = 0x0;
    allPages[0].availability = true;

    pageMap1.insert(pair<page,vector<header_r>>(p, init_blocks));*/



    return 0;
}