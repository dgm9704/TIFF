using System;

namespace Diwen.Tiff.Extensions
{
    public static class BitConverterExtensions
    {
        public static byte[] GetBytes(Rational32 value)
        {
            var bytes = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes(value.Numerator), 0, bytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(value.Denominator), 0, bytes, 4, 4);
            return bytes;
        }

        public static byte[] GetBytes(URational32 value)
        {
            var bytes = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes(value.Numerator), 0, bytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(value.Denominator), 0, bytes,4, 4);
            return bytes;
        }
    }
}
