namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using Xunit;

    public class Rational32Test
    {
    
        [Fact]
        public void Rational32ConstructorTest()
        {
            Rational32 test = new Rational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((int)42), data, 4);
            Array.Copy(BitConverter.GetBytes((int)666), 0, data, 4, 4);
            int startIndex = 0;
            Rational32 target = new Rational32(data, startIndex);
            Assert.Equal(test, target);
        }

        [Fact]
        public void Rational32ConstructorTest1()
        {
            int numerator = 0;
            int denominator = 0;
            Rational32 target = new Rational32(numerator, denominator);
            Assert.Equal(numerator, target.Numerator);
            Assert.Equal(denominator, target.Denominator);
        }

        [Fact]
                public void Rational32ConstructorTest2()
        {
            byte[] data = null;
            int startIndex = 0;
            Assert.Throws<ArgumentNullException>(() => new Rational32(data, startIndex));
        }

        [Fact]
        public void Rational32ConstructorTest3()
        {
            byte[] data = new byte[8];
            int startIndex = 7;
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rational32(data, startIndex));
        }

        [Fact]
        public void EqualsTest()
        {
            Rational32 target = new Rational32();
            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void GetBytesTest()
        {
            Rational32 test = new Rational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((int)42), data, 4);
            Array.Copy(BitConverter.GetBytes((int)666), 0, data, 4, 4);
            int startIndex = 0;
            Rational32 target = new Rational32(data, startIndex);
            Assert.Equal(data, target.GetBytes());
        }

        [Fact]
        public void GetHashCodeTest()
        {
            Rational32 a = new Rational32(42, 666);
            Rational32 b = new Rational32(42, 666);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void ToStringTest()
        {
            Rational32 target = new Rational32(42, 666);
            string expected = "42/666";
            string actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_EqualityTest()
        {
            Rational32 value1 = new Rational32();
            Rational32 value2 = new Rational32();
            bool expected = true;
            bool actual;
            actual = (value1 == value2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void op_InequalityTest()
        {
            Rational32 value1 = new Rational32(1, 2);
            Rational32 value2 = new Rational32(2, 4);
            bool expected = true;
            bool actual;
            actual = (value1 != value2);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DenominatorTest()
        {
            Rational32 target = new Rational32(42, 666);
            int expected = 666;
            int actual;
            target.Denominator = expected;
            actual = target.Denominator;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NumeratorTest()
        {
            Rational32 target = new Rational32(42, 666);
            int expected = 0;
            int actual;
            target.Numerator = expected;
            actual = target.Numerator;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InverseTest()
        {
            Rational32 target = new Rational32(1207959552, 16777216);
            Rational32 expected = new Rational32(16777216, 1207959552);
            Rational32 actual;
            actual = target.Inverse();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReduceTest()
        {
            Rational32 target = new Rational32(1207959552, 16777216);
            Rational32 expected = new Rational32(72, 1);
            Rational32 actual;
            actual = target.Reduce();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReduceTest2()
        {
            Rational32 target = new Rational32(9, 25);
            Rational32 expected = new Rational32(9, 25);
            Rational32 actual;
            actual = target.Reduce();
            Assert.Equal(expected, actual);
        }
    }
}
