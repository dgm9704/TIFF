namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable]
    public class ShortTag : Tag
    {
        public ShortTag()
        {
            this.FieldType = FieldType.Short;
        }

        public ShortTag(params ushort[] args) : this()
        {
            this.Values = args;
            this.ValueCount = (uint)Values.Length;
        }
    }
}
