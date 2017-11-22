// Copyright (C) 2005-2017 by John Nordberg <john.nordberg@gmail.com>
// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted. 

namespace Diwen.Tiff.Test
{
    using Diwen.Tiff;
    using System;
    using Xunit;

    public class FieldTest
    {
        [Fact]
        public void FieldConstructorTest()
        {
            TagType tag = new TagType();
            FieldType type = new FieldType();
            Array values = null;
            Assert.Throws<ArgumentNullException>(() => new Field(tag, type, values));
        }

        /// <summary>
        ///A test for Copy
        ///</summary>
        [Fact]
        public void CopyTest()
        {
            Field original = new Field { TagType = TagType.Artist, FieldType = FieldType.Ascii, Value = "Diwen.Tiff" };
            Field copy = original.Copy();
            Assert.Equal(original.TagType, copy.TagType);
            Assert.Equal(original.FieldType, copy.FieldType);
            Assert.Equal(original.Value, copy.Value);
        }

        //[Fact]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void CreateTest()
        //{
        //    byte[] values = null;
        //    var field =  Field_Accessor.Create(Tag.Artist, FieldType.Byte, 3, values);
        //}

        [Fact]
        public void EnumeratedFieldValueTest()
        {
            var fld = new Field(TagType.Compression, FieldType.Short, new ushort[] { 2 });
            Assert.Contains("CCITTRLE", fld.ToString());
            fld = new Field((TagType)0, FieldType.Short, new ushort[] { 666 });
            Assert.Contains("666", fld.ToString());
        }
    }
}
