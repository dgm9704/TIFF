﻿namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using NUnit.Framework;

    /// <summary>
    ///This is a test class for URational32Test and is intended
    ///to contain all URational32Test Unit Tests
    ///</summary>
    [TestFixture]
    public class URational32Test
    {

        /// <summary>
        ///A test for URational32 Constructor
        ///</summary>
        [Test]
        public void URational32ConstructorTest()
        {
            URational32 test = new URational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((uint)42), data, 4);
            Array.Copy(BitConverter.GetBytes((uint)666), 0, data, 4, 4);
            int startIndex = 0;
            URational32 target = new URational32(data, startIndex);
            Assert.AreEqual(test, target);
        }

        /// <summary>
        ///A test for URational32 Constructor
        ///</summary>
        [Test]
        public void URational32ConstructorTest1()
        {
            uint numerator = 0;
            uint denominator = 0;
            URational32 target = new URational32(numerator, denominator);
            Assert.AreEqual(numerator, target.Numerator);
            Assert.AreEqual(denominator, target.Denominator);
        }

        /// <summary>
        ///A test for URational32 Constructor
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void URational32ConstructorTest2()
        {
            byte[] data = null;
            int startIndex = 0;
            URational32 target = new URational32(data, startIndex);
        }

        /// <summary>
        ///A test for URational32 Constructor
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void URational32ConstructorTest3()
        {
            byte[] data = new byte[8];
            int startIndex = 7;
            URational32 target = new URational32(data, startIndex);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Test]
        public void EqualsTest()
        {
            URational32 target = new URational32();
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
            URational32 test = new URational32(42, 666);
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes((uint)42), data, 4);
            Array.Copy(BitConverter.GetBytes((uint)666), 0, data, 4, 4);
            int startIndex = 0;
            URational32 target = new URational32(data, startIndex);
            CollectionAssert.AreEqual(data, target.GetBytes());
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [Test]
        public void GetHashCodeTest()
        {
            URational32 a = new URational32(42, 666);
            URational32 b = new URational32(42, 666);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [Test]
        public void ToStringTest()
        {
            URational32 target = new URational32(42, 666);
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
            URational32 value1 = new URational32();
            URational32 value2 = new URational32();
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
            URational32 value1 = new URational32(1, 2);
            URational32 value2 = new URational32(2, 4);
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
            URational32 target = new URational32(42, 666);
            uint expected = 666;
            uint actual;
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
            URational32 target = new URational32(42, 666);
            uint expected = 0;
            uint actual;
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
            URational32 target = new URational32(1207959552, 16777216);
            URational32 expected = new URational32(16777216, 1207959552);
            URational32 actual;
            actual = target.Inverse();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Reduce
        ///</summary>
        [Test]
        public void ReduceTest()
        {
            URational32 target = new URational32(1207959552, 16777216);
            URational32 expected = new URational32(72, 1);
            URational32 actual;
            actual = target.Reduce();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Reduce
        ///</summary>
        [Test]
        public void ReduceTest2()
        {
            URational32 target = new URational32(9, 25);
            URational32 expected = new URational32(9, 25);
            URational32 actual;
            actual = target.Reduce();
            Assert.AreEqual(expected, actual);
        }
    }
}
