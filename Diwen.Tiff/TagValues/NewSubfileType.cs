using System;

namespace Diwen.Tiff.TagValues
{
    [Flags()]
    public enum NewSubfileType : uint
    {
        None = 0,
        ReducedResolutionVersion = 1,
        Page = 2,
        TransparencyMask = 4,
    }
}
