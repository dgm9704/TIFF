namespace Diwen.Tiff
{
    using System;

    [Serializable()]
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

        public static bool operator ==(Rational32 value1, Rational32 value2)
        {
            return value1.Numerator == value2.Numerator && value1.Denominator == value2.Denominator;
        }

        public static bool operator !=(Rational32 value1, Rational32 value2)
        {
            return !(value1 == value2);
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[8];
            Array.Copy(BitConverter.GetBytes(this.Numerator), bytes, 4);
            Array.Copy(BitConverter.GetBytes(this.Denominator), 0, bytes, 4, 4);
            return bytes;
        }

        public override string ToString()
        {
            return this.Numerator + "/" + this.Denominator;
        }

        public override int GetHashCode()
        {
            return this.Numerator / this.Denominator;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rational32))
            {
                return false;
            }

            return this == (Rational32)obj;
        }
    }
}
