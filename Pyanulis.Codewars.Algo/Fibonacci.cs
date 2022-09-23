using System.Numerics;

namespace Pyanulis.Codewars.Algo;

internal class Fibonacci
{
    static BigInteger FibBigInt(int N)
    {
        int n = (N >= 0) ? N : -N;

        BigInteger a = new BigInteger(0);
        BigInteger b = new BigInteger(1);
        BigInteger x = new BigInteger(0);
        BigInteger y = new BigInteger(1);

        while (n != 0)
        {
            if (n % 2 == 0)
            {
                BigInteger al = a;
                a = al * al + b * b;
                b = al * b + b * al + b * b;
                n /= 2;
            }
            else
            {
                BigInteger xl = x;
                x = a * x + b * y;
                y = b * xl + a * y + b * y;
                n -= 1;
            }
        }

        if (N < 0 && N % 2 == 0)
        {
            x = -x;
        }

        return x;
    }
}
