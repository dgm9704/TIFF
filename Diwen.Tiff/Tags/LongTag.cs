using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class LongTag : Tag
    {
        public LongTag()
        {
            this.DataType = DataType.Long;
        }

        public LongTag(params uint[] values)
            : base()
        {
            this.Values = values;
            this.ValueCount = (uint)Values.Length;
        }
    }
}
