# Memory optimization
## Condition
```
static void Main(string[] args)
{
    int[,,] a = new int[10,10,10];
    int res = 0;

    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            for (int k = 0; k < 10; k++)
            {
                a[k, j, i]++;
            }
        }
    }
}
```
### Timer result

For 600 elements:
**00:00:10.12**

## Solution for optimization
```
int size = 1000;
            
            
int[,,] a = new int[size,size,size];

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        for (int k = 0; k < size; k++)
        {
            a[i, j, k]++;
        }
    }
}
```
### Timer result after optimization

For 600 elements:
**00:00:01.72**

## Conclusion

Matrix traversal by columns has a bad locality, because the matrix is stored in memory by rows, so in the second case the traversal is 7 times faster

