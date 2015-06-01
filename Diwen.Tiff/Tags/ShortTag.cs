using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class ShortTag : Tag
    {
        public ShortTag()
        {
            this.DataType = TiffDataType.Short;
        }

        public ShortTag(params ushort[] args) : base()
        {
            this.Values = args;
            this.ValueCount = (uint)Values.Length;
        }
    }
}
