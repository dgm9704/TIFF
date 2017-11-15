namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable()]
    public class AsciiTag : TiffTag
    {
        public AsciiTag(Tag type)
        {
            this.DataType = TiffDataType.Ascii;
            this.Tag = type;
        }

        public AsciiTag(Tag type, string value)
            : this(type)
        {
            this.Values = (value + '\0').ToCharArray();
            this.ValueCount = (uint)Values.Length;
        }
    }
}
