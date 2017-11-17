namespace Diwen.Tiff
{
    using System;

    [Serializable]
    public struct Rational32
    {
        public Rational32(int numerator, int denominator)
            : this()
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
        }

        public Rational32(byte[] data, int startIndex)
            : this()
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (data.Length < startIndex + 8)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            this.Numerator = BitConverter.ToInt32(data, startIndex);
            this.Denominator = BitConverter.ToInt32(data, startIndex + 4);
        }

        public int Numerator { get; set; }

        public int Denominator { get; set; }

        public static bool operator ==(Rational32 left, Rational32 right)
        => left.Numerator == right.Numerator
        && left.Denominator == right.Denominator;

        public static bool operator !=(Rational32 value1, Rational32 value2)
        => !(value1 == value2);

        public byte[] GetBytes()
        {
            var bytes = new byte[8];
            Array.Copy(BitConverter.GetBytes(this.Numerator), bytes, 4);
            Array.Copy(BitConverter.GetBytes(this.Denominator), 0, bytes, 4, 4);
            return bytes;
        }

        public override string ToString()
        => this.Numerator + "/" + this.Denominator;

        //public override string ToString(bool reduce)
        //{
        //    if (reduce)
        //    {
        //        return this.Reduce().ToString();
        //    }
        //    else
        //    {
        //        return this.ToString();
        //    }
        //}

        public override int GetHashCode()
        => this.Numerator / this.Denominator;

        public override bool Equals(object obj)
        {
            if (!(obj is Rational32))
            {
                return false;
            }

            return this == (Rational32)obj;
        }

        public Rational32 Inverse()
        => new Rational32(numerator: this.Denominator, denominator: this.Numerator);

        public Rational32 Reduce()
        {
            int gcd = (int)Util.GCD(this.Numerator, this.Denominator);
            return gcd != 1
                ? new Rational32(numerator: this.Numerator / gcd, denominator: this.Denominator / gcd)
                : this;
        }
    }
}
