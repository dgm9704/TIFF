namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using NUnit.Framework;

    /// <summary>
    ///This is a test class for FieldTest and is intended
    ///to contain all FieldTest Unit Tests
    ///</summary>
    [TestFixture]
    public class FieldTest
    {
        /// <summary>
        ///A test for Field Constructor
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldConstructorTest()
        {
            Tag tag = new Tag(); 
            FieldType type = new FieldType(); 
            Array values = null; 
            Field target = new Field(tag, type, values);
        }

        /// <summary>
        ///A test for Copy
        ///</summary>
        [Test]
        public void CopyTest()
        {
            Field original = new Field { Tag = Tag.Artist, FieldType = FieldType.Ascii, Value = "Diwen.Tiff" };
            Field copy = original.Copy();
            Assert.AreEqual(original.Tag, copy.Tag);
            Assert.AreEqual(original.FieldType, copy.FieldType);
            Assert.AreEqual(original.Value, copy.Value);
        }

        ///// <summary>
        /////A test for Create
        /////</summary>
        //[Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void CreateTest()
        //{
        //    byte[] values = null;
        //    var field =  Field_Accessor.Create(Tag.Artist, FieldType.Byte, 3, values);
        //}

        [Test]
        public void EnumeratedFieldValueTest()
        {
            var fld = new Field(Tag.Compression, FieldType.Short, new ushort[] { 2 });
            StringAssert.Contains("CCITTRLE", fld.ToString());
            fld = new Field((Tag)0, FieldType.Short, new ushort[] { 666 });
            StringAssert.Contains("666", fld.ToString());
        }
    }
}
