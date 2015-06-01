using System;

namespace Diwen.Tiff.Tags
{
    [Serializable()]
    class SubFileTypeTag : ShortTag
    {
        public SubFileTypeTag(SubfileType type)
            : base((ushort)type)
        {
            this.TagType = TagType.SubfileType;
        }
    }
}
