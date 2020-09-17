#include <iostream>

#define HEAP_SIZE 1024
//static char buffer[HEAP_SIZE];


void* alloc(size_t sz)
{
    static size_t off = 0;
    char* ptr = 0;
    if(off + sz < HEAP_SIZE)
    {
        ptr = (char*)off;
        off += sz;
    }

    return ptr;
}



int main() {



    std::cout<<alloc(1)<<std::endl;
    std::cout<<alloc(1)<<std::endl;
    std::cout<<alloc(1)<<std::endl;
    std::cout<<alloc(5)<<std::endl;
    std::cout<<alloc(1000)<<std::endl;
    std::cout<<alloc(5)<<std::endl;
    std::cout<<alloc(5)<<std::endl;
    std::cout<<alloc(5)<<std::endl;
    std::cout<<alloc(1)<<std::endl;
    std::cout<<alloc(1)<<std::endl;
    std::cout<<alloc(200)<<std::endl;
    std::cout<<alloc(12)<<std::endl;


    return 0;
}




//class Memory
//{
//public:
//    Memory();
//    int getCM();
//    char* mem;
//    void* mem_alloc(int);
//    void mem_free(void*);
//    void* mem_realloc(void*, int);
//private:
//    int cm = 0;
//};
//
//Memory::Memory()
//{
//    mem = new char[1024];
//    cm++;
//
//    int* s = (int*)mem;
//    *s=10240;
//}
//
//int Memory::getCM() {
//    return cm;
//}
//
//void* Memory::mem_alloc(int size) {
//
//
//
//    int p = 0;
//    int rozp = 0;
//    int pp;
//    bool fl = false;
//    for (int i = 0; i < cm; i++) {
//        rozp += p;
//        pp = *(int*)(mem+rozp);
//        p = pp/10;
//        if ((size <= p) && (pp % 2 == 0)) {
//            fl = true;
//            break;
//        }
//    }
//    if (fl) {
//        int*re = (int*)(mem+rozp);
//        *re = size * 10 + 1;
//        if (p - size != 0) {
//            cm++;
//            int*ree = (int*)(mem+rozp+size);
//            *ree = (p - size)*10;
//        }
//
//        return mem+rozp;
//    } else {
//        return NULL;
//    }
//}
//
//void Memory::mem_free(void* addr) {
//
//    clock_t start=clock();
//    if ((*((int*)(addr)))%2 == 1) {
//        (*((int*)(addr)))--;
//    }
//
//    if ((char*)addr < mem+1024) {
//        if ((*(int*)((char*)(addr)+((*((int*)(addr)))/10))) % 2 == 0) {
//            (*((int*)(addr))) = (((*((int*)(addr)))/10) + (*(int*)((char*)(addr)+((*((int*)(addr)))/10)))/10)*10;
//            cm--;
//        }
//    }
//
//    if ((char*)addr != mem) {
//        char* c = mem;
//        int pam = 0;
//        while ((char*)addr != (c+((*((int*)c))/10))){
//
//            int pam = (*((int*)c))/10;
//            c += pam;
//        }
//        c -= pam;
//        if (((*((int*)c)) % 2 == 0) && (c >= mem)) {
//            (*((int*)(c))) = (((*((int*)(c)))/10) + (*(int*)((c)+((*((int*)(c)))/10)))/10)*10;
//            cm--;
//        }
//    }
//    clock_t end=clock();
//    std::cout << "START: " << start << " END: " << end << " TIME: " << end-start << std::endl;
//}
//
//void* Memory::mem_realloc(void* addr, int size) {
//
//    int* bup = new int[cm];
//    int cmm = cm;
//    int p = 0;
//    for(int i = 0; i < cm; i++) {
//        bup[i] = *((int*)(mem+p));
//        p += (bup[i]/10);
//    }
//
//    int siz = (*(int*)addr)/10;
//    if (size < siz) {
//        siz = size;
//    }
//    mem_free(addr);
//    void* c = mem_alloc(size);
//
//    if (c != NULL) {
//
//        for (int i = 4; i < siz; i++) {
//            *((char*)(c)+i) = *((char*)(addr)+i);
//        }
//    } else {
//
//
//        p = 0;
//        cm = cmm;
//        for(int i = 0; i < cm; i++) {
//            *((int*)(mem+p)) = bup[i];
//            p += (bup[i]/10);
//        }
//        return addr;
//
//    }
//    return c;
//
//}