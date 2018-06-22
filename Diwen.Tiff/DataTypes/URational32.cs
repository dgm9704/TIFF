//
//  This file is part of Diwen.Tiff.
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2005-2018 John Nordberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length < startIndex + 8)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            this.Numerator = BitConverter.ToUInt32(data, startIndex);
            this.Denominator = BitConverter.ToUInt32(data, startIndex + 4);
        }

        public uint Numerator { get; }

        public uint Denominator { get; }

        public static bool operator ==(URational32 left, URational32 right)
        => left.Numerator == right.Numerator
        && left.Denominator == right.Denominator;

        public static bool operator !=(URational32 left, URational32 right)
        => !(left == right);

        public override string ToString()
        => $"{Numerator}/{Denominator}";

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
        => (int)(this.Numerator / this.Denominator);

        public URational32 Inverse()
        => new URational32(denominator: Numerator, numerator: Denominator);

        public URational32 Reduce()
        {
            uint gcd = (uint)Util.GCD(this.Numerator, this.Denominator);
            return gcd != 1
                ? new URational32(numerator: Numerator / gcd, denominator: Denominator / gcd)
                : this;
        }
    }
}