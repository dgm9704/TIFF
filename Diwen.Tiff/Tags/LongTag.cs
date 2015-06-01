using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class LongTag : Tag
    {
        public LongTag()
        {
            this.DataType = TiffDataType.Long;
        }

        public LongTag(params uint[] args)
            : base()
        {
            this.Values = args;
            this.ValueCount = (uint)Values.Length;
        }
    }
}
