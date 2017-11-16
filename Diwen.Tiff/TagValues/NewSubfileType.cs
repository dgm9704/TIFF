namespace Diwen.Tiff.TagValues
{
    using System;

    [Flags]
    public enum NewSubfileType : uint
    {
        None = 0,
        ReducedResolutionVersion = 1,
        Page = 2,
        TransparencyMask = 4,
    }
}
