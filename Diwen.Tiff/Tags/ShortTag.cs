using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    public class ShortTag : Tag
    {
        public ShortTag()
        {
            this.DataType = DataType.Short;
        }

        public ShortTag(params ushort[] values) : this()
        {
            this.Values = values;
            this.ValueCount = (uint)Values.Length;
        }
    }
}
