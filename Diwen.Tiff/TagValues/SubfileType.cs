using System;

namespace Diwen.Tiff.TagValues
{
    [Obsolete("NewSubfileType should be used instead.")]
    public enum SubfileType : ushort
    {
        FullResolution = 1,
        ReducedResolution = 2,
        Page = 3,
    }
}
