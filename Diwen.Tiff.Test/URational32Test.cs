// Copyright (C) 2005-2017 by John Nordberg <john.nordberg@gmail.com>
// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted. 

namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using Xunit;

    public class URational32Test
    {

        [Fact]
        public void URational32ConstructorTest()
        {
            URational32 test = new URational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((uint)42), data, 4);
            Array.Copy(BitConverter.GetBytes((uint)666), 0, data, 4, 4);
            int startIndex = 0;
            URational32 target = new URational32(data, startIndex);
            Assert.Equal(test, target);
        }

        [Fact]
        public void URational32ConstructorTest1()
        {
            uint numerator = 0;
            uint denominator = 0;
            URational32 target = new URational32(numerator, denominator);
            Assert.Equal(numerator, target.Numerator);
            Assert.Equal(denominator, target.Denominator);
        }

        [Fact]
        public void URational32ConstructorTest2()
        {
            byte[] data = null;
            int startIndex = 0;
            Assert.Throws<ArgumentNullException>(() => new URational32(data, startIndex));
        }

        [Fact]
        public void URational32ConstructorTest3()
        {
            byte[] data = new byte[8];
            int startIndex = 7;
            Assert.Throws<ArgumentOutOfRangeException>(() => new URational32(data, startIndex));
        }

        [Fact]
        public void EqualsTest()
        {
            URational32 target = new URational32();
            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void GetBytesTest()
        {
            URational32 test = new URational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((uint)42), data, 4);
            Array.Copy(BitConverter.GetBytes((uint)666), 0, data, 4, 4);
            int startIndex = 0;
            URational32 target = new URational32(data, startIndex);
            Assert.Equal(data, target.GetBytes());
        }

        [Fact]
        public void GetHashCodeTest()
        {
            URational32 a = new URational32(42, 666);
            URational32 b = new URational32(42, 666);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void ToStringTest()
        {
            URational32 target = new URational32(42, 666);
            string expected = "42/666";
            string actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_EqualityTest()
        {
            URational32 value1 = new URational32();
            URational32 value2 = new URational32();
            bool expected = true;
            bool actual;
            actual = (value1 == value2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_InequalityTest()
        {
            URational32 value1 = new URational32(1, 2);
            URational32 value2 = new URational32(2, 4);
            bool expected = true;
            bool actual;
            actual = (value1 != value2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DenominatorTest()
        {
            URational32 target = new URational32(42, 666);
            uint expected = 666;
            uint actual;
            target.Denominator = expected;
            actual = target.Denominator;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NumeratorTest()
        {
            URational32 target = new URational32(42, 666);
            uint expected = 0;
            uint actual;
            target.Numerator = expected;
            actual = target.Numerator;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InverseTest()
        {
            URational32 target = new URational32(1207959552, 16777216);
            URational32 expected = new URational32(16777216, 1207959552);
            URational32 actual;
            actual = target.Inverse();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReduceTest()
        {
            URational32 target = new URational32(1207959552, 16777216);
            URational32 expected = new URational32(72, 1);
            URational32 actual;
            actual = target.Reduce();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReduceTest2()
        {
            URational32 target = new URational32(9, 25);
            URational32 expected = new URational32(9, 25);
            URational32 actual;
            actual = target.Reduce();
            Assert.Equal(expected, actual);
        }
    }
}
