#include <iostream>
#include <zconf.h>


using namespace std;




struct header_t
{
    size_t size;
    unsigned is_free;
    struct header_t *next;
};


struct header_t *head = NULL, *tail = NULL;



struct header_t *get_free_block(size_t size)
{
    struct header_t *curr = head;
    while(curr) {
        if (curr->is_free && curr->size >= size)
            return curr;
        curr = curr->next;
    }
    return NULL;
}


void *malloc(size_t size)
{
    size_t total_size;
    void *block;
    struct header_t *header;

    if (!size)
        return NULL;


    header = get_free_block(size);

    if (header) {
        header->is_free = 0;
        return (void*)(header + 1);
    }

    total_size = sizeof(struct header_t) + size;
    block = sbrk(total_size);
    if (block == (void*) -1) {
        return NULL;
    }

    header = static_cast<header_t *>(block);
    header->size = size;
    header->is_free = 0;
    header->next = NULL;
    if (!head)
        head = header;
    if (tail)
        tail->next = header;
    tail = header;

    return (void*)(header + 1);
}


void free(void *block)
{
    struct header_t *header, *tmp;
    void *programbreak;

    if (!block)
        return;



    header = (struct header_t*)block - 1;


    programbreak = sbrk(0);


    if ((char*)block + header->size == programbreak) {

        if (head == tail) {
            head = tail = NULL;
        }

        else {
            tmp = head;
            while (tmp) {
                if(tmp->next == tail) {
                    tmp->next = NULL;
                    tail = tmp;
                }
                tmp = tmp->next;
            }
        }

        sbrk(0 - sizeof(struct header_t) - header->size);

        return;
    }
    header->is_free = 1;

}

void *realloc(void *block, size_t size)
{
    struct header_t *header;
    void *ret;


    if (!block || !size)
        return malloc(size);


    header = (struct header_t*)block - 1;

    if (header->size >= size)
        return block;

    ret = malloc(size);

    if (ret) {
        memcpy(ret, block, header->size);

        free(block);
    }

    return ret;
}

void print_mem_list()
{
    header_t *curr = head;
    printf("head = %p, tail = %p \n", (void*)head, (void*)tail);
    while(curr) {
        printf("addr = %p, size = %zu, is_free=%u, next=%p\n",
               (void*)curr, curr->size, curr->is_free, (void*)curr->next);
        curr = curr->next;
    }
}


int main() {



    cout<<malloc(1)<<endl;
    cout<<malloc(1)<<endl;
    cout<<malloc(1)<<endl;

    print_mem_list();
    free(head->next);
    print_mem_list();
    malloc(2);
    print_mem_list();


    print_mem_list();

    malloc(1);

    print_mem_list();




    return 0;
}