using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class AsciiTag : Tag
    {
        public AsciiTag()
        {
            this.DataType = TiffDataType.Ascii;
        }

        public AsciiTag(string value)
            : this()
        {
            this.Values = (value + '\0').ToCharArray();
            this.ValueCount = (uint)Values.Length;
        }
    }
}
