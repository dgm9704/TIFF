namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using NUnit.Framework;

    /// <summary>
    ///This is a test class for Rational32Test and is intended
    ///to contain all Rational32Test Unit Tests
    ///</summary>
    [TestFixture]
    public class Rational32Test
    {
    
        /// <summary>
        ///A test for Rational32 Constructor
        ///</summary>
        [Test]
        public void Rational32ConstructorTest()
        {
            Rational32 test = new Rational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((int)42), data, 4);
            Array.Copy(BitConverter.GetBytes((int)666), 0, data, 4, 4);
            int startIndex = 0;
            Rational32 target = new Rational32(data, startIndex);
            Assert.AreEqual(test, target);
        }

        /// <summary>
        ///A test for Rational32 Constructor
        ///</summary>
        [Test]
        public void Rational32ConstructorTest1()
        {
            int numerator = 0;
            int denominator = 0;
            Rational32 target = new Rational32(numerator, denominator);
            Assert.AreEqual(numerator, target.Numerator);
            Assert.AreEqual(denominator, target.Denominator);
        }

        /// <summary>
        ///A test for Rational32 Constructor
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Rational32ConstructorTest2()
        {
            byte[] data = null;
            int startIndex = 0;
            Rational32 target = new Rational32(data, startIndex);
        }

        /// <summary>
        ///A test for Rational32 Constructor
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Rational32ConstructorTest3()
        {
            byte[] data = new byte[8];
            int startIndex = 7;
            Rational32 target = new Rational32(data, startIndex);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Test]
        public void EqualsTest()
        {
            Rational32 target = new Rational32();
            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetBytes
        ///</summary>
        [Test]
        public void GetBytesTest()
        {
            Rational32 test = new Rational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((int)42), data, 4);
            Array.Copy(BitConverter.GetBytes((int)666), 0, data, 4, 4);
            int startIndex = 0;
            Rational32 target = new Rational32(data, startIndex);
            CollectionAssert.AreEqual(data, target.GetBytes());
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [Test]
        public void GetHashCodeTest()
        {
            Rational32 a = new Rational32(42, 666);
            Rational32 b = new Rational32(42, 666);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [Test]
        public void ToStringTest()
        {
            Rational32 target = new Rational32(42, 666);
            string expected = "42/666";
            string actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [Test]
        public void op_EqualityTest()
        {
            Rational32 value1 = new Rational32();
            Rational32 value2 = new Rational32();
            bool expected = true;
            bool actual;
            actual = (value1 == value2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [Test]
        public void op_InequalityTest()
        {
            Rational32 value1 = new Rational32(1, 2);
            Rational32 value2 = new Rational32(2, 4);
            bool expected = true;
            bool actual;
            actual = (value1 != value2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Denominator
        ///</summary>
        [Test]
        public void DenominatorTest()
        {
            Rational32 target = new Rational32(42, 666);
            int expected = 666;
            int actual;
            target.Denominator = expected;
            actual = target.Denominator;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Numerator
        ///</summary>
        [Test]
        public void NumeratorTest()
        {
            Rational32 target = new Rational32(42, 666);
            int expected = 0;
            int actual;
            target.Numerator = expected;
            actual = target.Numerator;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Inverse
        ///</summary>
        [Test]
        public void InverseTest()
        {
            Rational32 target = new Rational32(1207959552, 16777216);
            Rational32 expected = new Rational32(16777216, 1207959552);
            Rational32 actual;
            actual = target.Inverse();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Reduce
        ///</summary>
        [Test]
        public void ReduceTest()
        {
            Rational32 target = new Rational32(1207959552, 16777216);
            Rational32 expected = new Rational32(72, 1);
            Rational32 actual;
            actual = target.Reduce();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Reduce
        ///</summary>
        [Test]
        public void ReduceTest2()
        {
            Rational32 target = new Rational32(9, 25);
            Rational32 expected = new Rational32(9, 25);
            Rational32 actual;
            actual = target.Reduce();
            Assert.AreEqual(expected, actual);
        }
    }
}
