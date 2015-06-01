using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    [Obsolete("NewSubfileTypeTag should be used instead.")]
    public class SubfileTypeTag : ShortTag
    {
        public SubfileTypeTag(SubfileType type)
            : base((ushort)type)
        {
            this.TagType = TagType.SubfileType;
        }
    }
}
