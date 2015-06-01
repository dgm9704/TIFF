namespace Diwen.Tiff
{
    internal class Util
    {
        public static long GCD(long a, long b)
        {
            if (a == 0)
            {
                if (b == 0)
                {
                    return 0;
                }
                return a;
            }

            if (b == 0)
            {
                return a;
            }

            do
            {
                if (a < b)
                {
                    long temp;
                    temp = a;
                    a = b;
                    b = temp;
                }

                a -= b;
            }
            while (a > 0);

            return b;
        }
    }
}
