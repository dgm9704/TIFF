using System;

namespace Diwen.Tiff
{
    [Serializable()]
    public struct Rational32
    {
        public int Numerator { get; set; }

        public int Denominator { get; set; }

        public Rational32(int numerator, int denominator)
            : this()
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public Rational32(byte[] data, int startIndex)
            : this()
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < startIndex)
                throw new ArgumentOutOfRangeException("startIndex");

            Numerator = BitConverter.ToInt32(data, startIndex);
            Denominator = BitConverter.ToInt32(data, startIndex + 4);
        }

        public override string ToString()
        {
            return Numerator + "/" + Denominator;
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[8];
            Array.Copy(BitConverter.GetBytes(this.Numerator), bytes, 4);
            Array.Copy(BitConverter.GetBytes(this.Denominator), 0, bytes, 4, 4);
            return bytes;
        }

        public static bool operator ==(Rational32 value1, Rational32 value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(Rational32 value1, Rational32 value2)
        {
            return !value1.Equals(value2);
        }

        public override int GetHashCode()
        {
            return Numerator / Denominator;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rational32))
                return false;

            return this == (Rational32)obj;
        }

    }
}
