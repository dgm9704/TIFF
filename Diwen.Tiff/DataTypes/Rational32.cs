//
//  This file is part of Diwen.Tiff.
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2005-2017 John Nordberg
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
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length < startIndex + 8)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
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
