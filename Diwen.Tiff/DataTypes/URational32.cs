namespace Diwen.Tiff
{
    using System;

    [Serializable]
    public struct URational32
    {
        public URational32(uint numerator, uint denominator)
            : this()
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
        }

        public URational32(byte[] data, int startIndex)
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

            this.Numerator = BitConverter.ToUInt32(data, startIndex);
            this.Denominator = BitConverter.ToUInt32(data, startIndex + 4);
        }

        public uint Numerator { get; set; }

        public uint Denominator { get; set; }

        public static bool operator ==(URational32 value1, URational32 value2)
        {
            return value1.Numerator == value2.Numerator && value1.Denominator == value2.Denominator;
        }

        public static bool operator !=(URational32 value1, URational32 value2)
        {
            return !(value1 == value2);
        }

        public override string ToString()
        {
            return this.Numerator + "/" + this.Denominator;
        }

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

        public byte[] GetBytes()
        {
            var bytes = new byte[8];
            Array.Copy(BitConverter.GetBytes(this.Numerator), bytes, 4);
            Array.Copy(BitConverter.GetBytes(this.Denominator), 0, bytes, 4, 4);
            return bytes;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is URational32))
            {
                return false;
            }

            return this == (URational32)obj;
        }

        public override int GetHashCode()
        {
            return (int)(this.Numerator / this.Denominator);
        }

        public URational32 Inverse()
        {
            return new URational32 { Denominator = this.Numerator, Numerator = this.Denominator };
        }

        public URational32 Reduce()
        {
            uint gcd = (uint)Util.GCD(this.Numerator, this.Denominator);
            if (gcd != 1)
            {
                return new URational32 { Numerator = this.Numerator / gcd, Denominator = this.Denominator / gcd };
            }
            return this;
        }
    }
}