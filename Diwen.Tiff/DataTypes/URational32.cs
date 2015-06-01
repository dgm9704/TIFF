using System;

namespace Diwen.Tiff
{
    [Serializable()]
    public struct URational32
    {
        public uint Numerator { get; set; }
        public uint Denominator { get; set; }

        public URational32(uint numerator, uint denominator)
            : this()
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public URational32(byte[] data, int startIndex)
            : this()
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (data.Length < startIndex)
                throw new ArgumentOutOfRangeException("startIndex"); 

            Numerator = BitConverter.ToUInt32(data, startIndex);
            Denominator = BitConverter.ToUInt32(data, startIndex + 4);
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

        public static bool operator ==(URational32 value1, URational32 value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(URational32 value1, URational32 value2)
        {
            return !value1.Equals(value2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is URational32))
                return false;

            return this == (URational32)obj;
        }

        public override int GetHashCode()
        {
            return (int)(Numerator / Denominator);
        }


    }
}
