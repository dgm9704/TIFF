namespace Diwen.Tiff.Tags
{
    using System;

    [Serializable]
    public class LongTag : Tag
    {
        public LongTag()
        {
            this.FieldType = FieldType.Long;
        }

        public LongTag(params uint[] args) : base()
        {
            this.Values = args;
            this.ValueCount = (uint)Values.Length;
        }
    }
}
