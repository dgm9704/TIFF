using System;

namespace Diwen.Tiff
{
    public enum DataType : ushort
    {
        /// <summary>System.Byte</summary>
        Byte = 1,
        /// <summary>System.String</summary>
        Ascii = 2,
        /// <summary>System.UInt16</summary>
        Short = 3,
        /// <summary>System.UInt32</summary>
        Long = 4,
        /// <summary>Diwen.Tiff.URational32</summary>
        Rational = 5,
        /// <summary>System.SByte</summary>
        SByte = 6,
        /// <summary>System.Byte</summary>
        Undefined = 7,
        /// <summary>System.Int16</summary>
        SShort = 8,
        /// <summary>System.Int32</summary>
        SLong = 9, 
        /// <summary>Diwen.Tiff.Rational32</summary>
        SRational = 10,
        /// <summary>System.Single</summary>
        Float = 11,
        /// <summary>System.Double</summary>
        Double = 12,
    }
}
