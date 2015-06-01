﻿using System;

namespace Diwen.Tiff
{
    [Flags()]
    public enum T4Options : uint
    {
        OneDimensional = 0,
        TwoDimensional = 1,
        Uncompressed = 2,
        FillBits = 4,
    }
}
