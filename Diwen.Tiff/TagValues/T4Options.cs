namespace Diwen.Tiff.TagValues
{
    using System;

    [Flags]
    public enum T4Options : uint
    {
        OneDimensional = 0,
        TwoDimensional = 1,
        Uncompressed = 2,
        FillBits = 4,
    }
}
