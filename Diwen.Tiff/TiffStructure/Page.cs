﻿namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using Diwen.Tiff.FieldValues;
    using System.Net;

    [Serializable]
    public class Page : Ifd
    {
        public Page()
            : base()
        {
        }

        public List<byte[]> ImageData { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var tag in this)
            {
                sb.AppendLine(tag.ToString());
            }

            return sb.ToString();
        }

        public void SetAsciiFieldValue(TagType tag, string value)
        {
            this.Add(tag, value ?? string.Empty);
        }

        public string GetAsciiFieldValue(TagType tag)
        {
            if (this.Contains(tag))
            {
                return string.Join("\n", new string((char[])this[tag].Values).Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                return string.Empty;
            }
        }

        public void Add(TagType tag, FieldType type, Array values)
        {
            this.Add(new Field(tag, type, values));
        }

        public void Add(Tag tag)
        {
            this.Add(new Field(tag.TagType, (FieldType)tag.FieldType, tag.Values));
        }

        public string Artist
        {
            get => GetAsciiFieldValue(TagType.Artist);
            set => SetAsciiFieldValue(TagType.Artist, value);
        }

        public ushort PageNumber
        {
            get
            {
                if (this.Contains(TagType.PageNumber))
                {
                    return (ushort)this[TagType.PageNumber].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (this.Contains(TagType.PageNumber))
                {
                    this[TagType.PageNumber].Values.SetValue(value, 0);
                }
                else
                {
                    this.Add(TagType.PageNumber, FieldType.Short, new ushort[] { value, 0 });
                }
            }
        }

        public ushort PageTotal
        {
            get
            {
                if (this.Contains(TagType.PageNumber))
                {
                    return (ushort)this[TagType.PageNumber].Values.GetValue(1);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (this.Contains(TagType.PageNumber))
                {
                    this[TagType.PageNumber].Values.SetValue(value, 1);
                }
                else
                {
                    this.Add(TagType.PageNumber, FieldType.Short, new ushort[] { 0, value });
                }
            }
        }

        public ushort BitsPerSample
        {
            get
            {
                if (this.Contains(TagType.BitsPerSample))
                {
                    return (ushort)this[TagType.BitsPerSample].Value;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                this.Add(TagType.BitsPerSample, value);
            }
        }

        public ushort CellLength
        {
            get
            {
                if (this.Contains(TagType.CellLength))
                {
                    return (ushort)this[TagType.CellLength].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.CellLength, value);
            }
        }

        public ushort CellWidth
        {
            get
            {
                if (this.Contains(TagType.CellWidth))
                {
                    return (ushort)this[TagType.CellWidth].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.CellWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag ColorMap
        /// </summary>
        public ushort[] ColorMap
        {
            get
            {
                if (this.Contains(TagType.ColorMap))
                {
                    return (ushort[])this[TagType.ColorMap].Values;
                }
                else
                {
                    return new ushort[] { };
                }
            }
            set
            {
                this.Add(TagType.ColorMap, FieldType.Short, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Compression
        /// </summary>
        public Compression Compression
        {
            get
            {
                if (this.Contains(TagType.Compression))
                {
                    return (Compression)this[TagType.Compression].Value;
                }
                else
                {
                    return Compression.NoCompression;
                }
            }
            set
            {
                this.Add(TagType.Compression, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Copyright
        /// </summary>
        public string Copyright
        {
            get
            {
                return GetAsciiFieldValue(TagType.Copyright);
            }
            set
            {
                SetAsciiFieldValue(TagType.Copyright, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag DateTime
        /// </summary>
        public string DateTime
        {
            get
            {
                return GetAsciiFieldValue(TagType.DateTime);
            }
            set
            {
                SetAsciiFieldValue(TagType.DateTime, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag ExtraSamples
        /// </summary>
        public ExtraSampleType[] ExtraSamples
        {
            get
            {
                if (this.Contains(TagType.ExtraSamples))
                {
                    return (ExtraSampleType[])this[TagType.ExtraSamples].Values;
                }
                else
                {
                    return new ExtraSampleType[] { };
                }
            }
            set
            {
                if (value == null)
                {
                    value = new ExtraSampleType[] { };
                }

                this.Add(TagType.ExtraSamples, FieldType.Short, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag FillOrder
        /// </summary>
        public FillOrder FillOrder
        {
            get
            {
                if (this.Contains(TagType.FillOrder))
                {
                    return (FillOrder)this[TagType.FillOrder].Value;
                }
                else
                {
                    return FillOrder.LowBitsFirst;
                }
            }
            set
            {
                this.Add(TagType.FillOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag FreeByteCounts
        /// </summary>
        public uint FreeByteCounts
        {
            get
            {
                if (this.Contains(TagType.FreeByteCounts))
                {
                    return (uint)this[TagType.FreeByteCounts].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.FreeByteCounts, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag FreeOffsets
        /// </summary>
        public uint FreeOffsets
        {
            get
            {
                if (this.Contains(TagType.FreeOffsets))
                {
                    return (uint)this[TagType.FreeOffsets].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.FreeOffsets, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag GrayResponseCurve
        /// </summary>
        public ushort[] GrayResponseCurve
        {
            get
            {
                if (this.Contains(TagType.GrayResponseCurve))
                {
                    return (ushort[])this[TagType.GrayResponseCurve].Values;
                }
                else
                {
                    return new ushort[] { };
                }
            }
            set
            {
                this.Add(TagType.GrayResponseCurve, FieldType.Short, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag GrayResponseUnit
        /// </summary>
        public GrayResponseUnit GrayResponseUnit
        {
            get
            {
                if (this.Contains(TagType.GrayResponseUnit))
                {
                    return (GrayResponseUnit)this[TagType.GrayResponseUnit].Value;
                }
                else
                {
                    return GrayResponseUnit.Hundredths;
                }
            }
            set
            {
                this.Add(TagType.GrayResponseUnit, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag HostComputer
        /// </summary>
        public string HostComputer
        {
            get
            {
                return GetAsciiFieldValue(TagType.HostComputer);
            }
            set
            {
                SetAsciiFieldValue(TagType.HostComputer, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageDescription
        /// </summary>
        public string ImageDescription
        {
            get
            {
                return GetAsciiFieldValue(TagType.ImageDescription);
            }
            set
            {
                SetAsciiFieldValue(TagType.ImageDescription, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageLebgth
        /// </summary>
        public uint ImageLength
        {
            get
            {
                if (this.Contains(TagType.ImageLength))
                {
                    return Convert.ToUInt32(this[TagType.ImageLength].Value);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.ImageLength, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageWidth
        /// </summary>
        public uint ImageWidth
        {
            get
            {
                if (this.Contains(TagType.ImageWidth))
                {
                    return (uint)this[TagType.ImageWidth].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.ImageWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Make
        /// </summary>
        public string Make
        {
            get
            {
                return GetAsciiFieldValue(TagType.Make);
            }
            set
            {
                SetAsciiFieldValue(TagType.Make, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag MaxSampleValue
        /// </summary>
        public ushort MaxSampleValue
        {
            get
            {
                if (this.Contains(TagType.MaxSampleValue))
                {
                    return (ushort)this[TagType.MaxSampleValue].Value;
                }
                else
                {
                    return (ushort)(Math.Pow(2, this.BitsPerSample) - 1);
                }
            }
            set
            {
                this.Add(TagType.MaxSampleValue, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag MinSampleValue
        /// </summary>
        public ushort MinSampleValue
        {
            get
            {
                if (this.Contains(TagType.MinSampleValue))
                {
                    return (ushort)this[TagType.MinSampleValue].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(TagType.MinSampleValue, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Model
        /// </summary>
        public string Model
        {
            get
            {
                return GetAsciiFieldValue(TagType.Model);
            }
            set
            {
                SetAsciiFieldValue(TagType.Model, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag NewSubfileType
        /// </summary>
        public NewSubfileType NewSubfileType
        {
            get
            {
                if (this.Contains(TagType.NewSubfileType))
                {
                    return (NewSubfileType)this[TagType.NewSubfileType].Value;
                }
                else
                {
                    return NewSubfileType.None;
                }
            }
            set
            {
                this.Add(TagType.NewSubfileType, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Orientation
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                if (this.Contains(TagType.Orientation))
                {
                    return (Orientation)this[TagType.Orientation].Value;
                }
                else
                {
                    return Orientation.TopLeft;
                }
            }
            set
            {
                this.Add(TagType.Orientation, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag PhotometricInterpretation
        /// </summary>
        public PhotometricInterpretation PhotometricInterpretation
        {
            get
            {
                if (this.Contains(TagType.PhotometricInterpretation))
                {
                    return (PhotometricInterpretation)this[TagType.PhotometricInterpretation].Value;
                }
                else
                {
                    return PhotometricInterpretation.WhiteIsZero;
                }
            }
            set
            {
                this.Add(TagType.PhotometricInterpretation, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag PlanarConfiguration
        /// </summary>
        public PlanarConfiguration PlanarConfiguration
        {
            get
            {
                if (this.Contains(TagType.PlanarConfiguration))
                {
                    return (PlanarConfiguration)this[TagType.PlanarConfiguration].Value;
                }
                else
                {
                    return PlanarConfiguration.Chunky;
                }
            }
            set
            {
                this.Add(TagType.PlanarConfiguration, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ResolutionUnit
        /// </summary>
        public ResolutionUnit ResolutionUnit
        {
            get
            {
                if (this.Contains(TagType.ResolutionUnit))
                {
                    return (ResolutionUnit)this[TagType.ResolutionUnit].Value;
                }
                else
                {
                    return ResolutionUnit.Inch;
                }
            }
            set
            {
                this.Add(TagType.ResolutionUnit, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag RowsPerStrip
        /// </summary>
        public uint RowsPerStrip
        {
            get
            {
                if (this.Contains(TagType.RowsPerStrip))
                {
                    return Convert.ToUInt32(this[TagType.RowsPerStrip].Value);
                }
                else
                {
                    return uint.MaxValue;
                }
            }
            set
            {
                this.Add(TagType.RowsPerStrip, value);
            }
        }

        /// <summary>
        /// Gets the number of strips per image calculated from ImageLength and RowsPerStrip
        /// </summary>
        public uint StripsPerImage
        {
            get
            {
                return (this.ImageLength + this.RowsPerStrip - 1) / RowsPerStrip;
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag SamplesPerPixel
        /// </summary>
        public ushort SamplesPerPixel
        {
            get
            {
                if (this.Contains(TagType.SamplesPerPixel))
                {
                    return (ushort)this[TagType.SamplesPerPixel].Value;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                this.Add(TagType.SamplesPerPixel, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Software
        /// </summary>
        public string Software
        {
            get
            {
                return GetAsciiFieldValue(TagType.Software);
            }
            set
            {
                SetAsciiFieldValue(TagType.Software, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag StripByteCounts
        /// </summary>
        public uint[] StripByteCounts
        {
            get
            {
                if (this.Contains(TagType.StripByteCounts))
                {
                    return (uint[])this[TagType.StripByteCounts].Values;
                }
                else
                {
                    return new uint[] { };
                }
            }
            set
            {
                if (value == null)
                {
                    value = new uint[] { };
                }

                this.Add(TagType.StripByteCounts, FieldType.Long, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag StripOffsets
        /// </summary>
        public uint[] StripOffsets
        {
            get
            {
                if (this.Contains(TagType.StripOffsets))
                {
                    return (uint[])this[TagType.StripOffsets].Values;
                }
                else
                {
                    return new uint[] { };
                }
            }
            set
            {
                if (value == null)
                {
                    value = new uint[] { };
                }

                this.Add(TagType.StripOffsets, FieldType.Long, value);
            }
        }

        ///// <summary>
        ///// Gets or sets the values of baseline tag SubfileType
        ///// </summary>
        //public SubfileType SubfileType
        //{
        //    get
        //    {
        //        if (this.Contains(tag.SubfileType))
        //        {
        //            return (SubfileType)this[tag.SubfileType].Value;
        //        }
        //        else
        //        {
        //            return default(SubfileType);
        //        }
        //    }
        //    set
        //    {
        //        this.Add(tag.SubfileType, value);
        //    }
        //}

        /// <summary>
        /// Gets or sets the values of baseline tag Threshholding
        /// </summary>
        public Threshholding Threshholding
        {
            get
            {
                if (this.Contains(TagType.Threshholding))
                {
                    return (Threshholding)this[TagType.Threshholding].Value;
                }
                else
                {
                    return Threshholding.None;
                }
            }
            set
            {
                this.Add(TagType.Threshholding, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag XResolution
        /// </summary>
        public URational32 XResolution
        {
            get
            {
                if (this.Contains(TagType.XResolution))
                {
                    return (URational32)this[TagType.XResolution].Value;
                }
                else
                {
                    return default(URational32);
                }
            }
            set
            {
                this.Add(TagType.XResolution, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag YResolution
        /// </summary>
        public URational32 YResolution
        {
            get
            {
                if (this.Contains(TagType.YResolution))
                {
                    return (URational32)this[TagType.YResolution].Value;
                }
                else
                {
                    return default(URational32);
                }
            }
            set
            {
                this.Add(TagType.YResolution, value);
            }
        }
    }
}
