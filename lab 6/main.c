#include <stdio.h>
#include <unistd.h>

    int resultOfSum(int a, int b)
    {
        return a+b;
    }

    int func1(int a, int b)
    {
        int res = 0;
        sleep(1);
        res = resultOfSum(a, b);
        return res;
    }

    int func2(int a, int b)
    {
        int res = func1(a, b);
        return res;
    }

int main(void)
{
    func2(20, 40);

    return 0;
}
