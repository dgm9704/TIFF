namespace Diwen.Tiff.Tags
{
    using System;
    using Diwen.Tiff.TagValues;

    [Serializable()]
    public class AsciiTag : TiffTag
    {
        public AsciiTag(TagType type)
        {
            this.DataType = TiffDataType.Ascii;
            this.TagType = type;
        }

        public AsciiTag(TagType type, string value)
            : this(type)
        {
            this.Values = (value + '\0').ToCharArray();
            this.ValueCount = (uint)Values.Length;
        }
    }
}
