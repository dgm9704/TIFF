namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable]
    public class AsciiTag : Tag
    {
        public AsciiTag(TagType type)
        {
            this.FieldType = FieldType.Ascii;
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
