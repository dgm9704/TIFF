namespace Diwen.Tiff
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

        public void SetAsciiFieldValue(Tag tag, string value)
        {
            this.Add(tag, value ?? string.Empty);
        }

        public string GetAsciiFieldValue(Tag tag)
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

        public void Add(Tag tag, FieldType type, Array values)
        {
            this.Add(new Field(tag, type, values));
        }

        public void Add(TiffTag tag)
        {
            this.Add(new Field(tag.Tag, (FieldType)tag.DataType, tag.Values));
        }

        public string Artist
        {
            get
            {
                return this.GetAsciiFieldValue(Tag.Artist);
            }
            set
            {
                this.SetAsciiFieldValue(Tag.Artist, value);
            }
        }

        public ushort PageNumber
        {
            get
            {
                if (this.Contains(Tag.PageNumber))
                {
                    return (ushort)this[Tag.PageNumber].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (this.Contains(Tag.PageNumber))
                {
                    this[Tag.PageNumber].Values.SetValue(value, 0);
                }
                else
                {
                    this.Add(Tag.PageNumber, FieldType.Short, new ushort[] { value, 0 });
                }
            }
        }

        public ushort PageTotal
        {
            get
            {
                if (this.Contains(Tag.PageNumber))
                {
                    return (ushort)this[Tag.PageNumber].Values.GetValue(1);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (this.Contains(Tag.PageNumber))
                {
                    this[Tag.PageNumber].Values.SetValue(value, 1);
                }
                else
                {
                    this.Add(Tag.PageNumber, FieldType.Short, new ushort[] { 0, value });
                }
            }
        }

        public ushort BitsPerSample
        {
            get
            {
                if (this.Contains(Tag.BitsPerSample))
                {
                    return (ushort)this[Tag.BitsPerSample].Value;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                this.Add(Tag.BitsPerSample, value);
            }
        }

        public ushort CellLength
        {
            get
            {
                if (this.Contains(Tag.CellLength))
                {
                    return (ushort)this[Tag.CellLength].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.CellLength, value);
            }
        }

        public ushort CellWidth
        {
            get
            {
                if (this.Contains(Tag.CellWidth))
                {
                    return (ushort)this[Tag.CellWidth].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.CellWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag ColorMap
        /// </summary>
        public ushort[] ColorMap
        {
            get
            {
                if (this.Contains(Tag.ColorMap))
                {
                    return (ushort[])this[Tag.ColorMap].Values;
                }
                else
                {
                    return new ushort[] { };
                }
            }
            set
            {
                this.Add(Tag.ColorMap, FieldType.Short, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Compression
        /// </summary>
        public Compression Compression
        {
            get
            {
                if (this.Contains(Tag.Compression))
                {
                    return (Compression)this[Tag.Compression].Value;
                }
                else
                {
                    return Compression.NoCompression;
                }
            }
            set
            {
                this.Add(Tag.Compression, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Copyright
        /// </summary>
        public string Copyright
        {
            get
            {
                return GetAsciiFieldValue(Tag.Copyright);
            }
            set
            {
                SetAsciiFieldValue(Tag.Copyright, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag DateTime
        /// </summary>
        public string DateTime
        {
            get
            {
                return GetAsciiFieldValue(Tag.DateTime);
            }
            set
            {
                SetAsciiFieldValue(Tag.DateTime, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag ExtraSamples
        /// </summary>
        public ExtraSampleType[] ExtraSamples
        {
            get
            {
                if (this.Contains(Tag.ExtraSamples))
                {
                    return (ExtraSampleType[])this[Tag.ExtraSamples].Values;
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

                this.Add(Tag.ExtraSamples, FieldType.Short, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag FillOrder
        /// </summary>
        public FillOrder FillOrder
        {
            get
            {
                if (this.Contains(Tag.FillOrder))
                {
                    return (FillOrder)this[Tag.FillOrder].Value;
                }
                else
                {
                    return FillOrder.LowBitsFirst;
                }
            }
            set
            {
                this.Add(Tag.FillOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag FreeByteCounts
        /// </summary>
        public uint FreeByteCounts
        {
            get
            {
                if (this.Contains(Tag.FreeByteCounts))
                {
                    return (uint)this[Tag.FreeByteCounts].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.FreeByteCounts, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag FreeOffsets
        /// </summary>
        public uint FreeOffsets
        {
            get
            {
                if (this.Contains(Tag.FreeOffsets))
                {
                    return (uint)this[Tag.FreeOffsets].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.FreeOffsets, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag GrayResponseCurve
        /// </summary>
        public ushort[] GrayResponseCurve
        {
            get
            {
                if (this.Contains(Tag.GrayResponseCurve))
                {
                    return (ushort[])this[Tag.GrayResponseCurve].Values;
                }
                else
                {
                    return new ushort[] { };
                }
            }
            set
            {
                this.Add(Tag.GrayResponseCurve, FieldType.Short, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag GrayResponseUnit
        /// </summary>
        public GrayResponseUnit GrayResponseUnit
        {
            get
            {
                if (this.Contains(Tag.GrayResponseUnit))
                {
                    return (GrayResponseUnit)this[Tag.GrayResponseUnit].Value;
                }
                else
                {
                    return GrayResponseUnit.Hundredths;
                }
            }
            set
            {
                this.Add(Tag.GrayResponseUnit, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag HostComputer
        /// </summary>
        public string HostComputer
        {
            get
            {
                return GetAsciiFieldValue(Tag.HostComputer);
            }
            set
            {
                SetAsciiFieldValue(Tag.HostComputer, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageDescription
        /// </summary>
        public string ImageDescription
        {
            get
            {
                return GetAsciiFieldValue(Tag.ImageDescription);
            }
            set
            {
                SetAsciiFieldValue(Tag.ImageDescription, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageLebgth
        /// </summary>
        public uint ImageLength
        {
            get
            {
                if (this.Contains(Tag.ImageLength))
                {
                    return Convert.ToUInt32(this[Tag.ImageLength].Value);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.ImageLength, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageWidth
        /// </summary>
        public uint ImageWidth
        {
            get
            {
                if (this.Contains(Tag.ImageWidth))
                {
                    return (uint)this[Tag.ImageWidth].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.ImageWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Make
        /// </summary>
        public string Make
        {
            get
            {
                return GetAsciiFieldValue(Tag.Make);
            }
            set
            {
                SetAsciiFieldValue(Tag.Make, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag MaxSampleValue
        /// </summary>
        public ushort MaxSampleValue
        {
            get
            {
                if (this.Contains(Tag.MaxSampleValue))
                {
                    return (ushort)this[Tag.MaxSampleValue].Value;
                }
                else
                {
                    return (ushort)(Math.Pow(2, this.BitsPerSample) - 1);
                }
            }
            set
            {
                this.Add(Tag.MaxSampleValue, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag MinSampleValue
        /// </summary>
        public ushort MinSampleValue
        {
            get
            {
                if (this.Contains(Tag.MinSampleValue))
                {
                    return (ushort)this[Tag.MinSampleValue].Value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.Add(Tag.MinSampleValue, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Model
        /// </summary>
        public string Model
        {
            get
            {
                return GetAsciiFieldValue(Tag.Model);
            }
            set
            {
                SetAsciiFieldValue(Tag.Model, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag NewSubfileType
        /// </summary>
        public NewSubfileType NewSubfileType
        {
            get
            {
                if (this.Contains(Tag.NewSubfileType))
                {
                    return (NewSubfileType)this[Tag.NewSubfileType].Value;
                }
                else
                {
                    return NewSubfileType.None;
                }
            }
            set
            {
                this.Add(Tag.NewSubfileType, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Orientation
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                if (this.Contains(Tag.Orientation))
                {
                    return (Orientation)this[Tag.Orientation].Value;
                }
                else
                {
                    return Orientation.TopLeft;
                }
            }
            set
            {
                this.Add(Tag.Orientation, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag PhotometricInterpretation
        /// </summary>
        public PhotometricInterpretation PhotometricInterpretation
        {
            get
            {
                if (this.Contains(Tag.PhotometricInterpretation))
                {
                    return (PhotometricInterpretation)this[Tag.PhotometricInterpretation].Value;
                }
                else
                {
                    return PhotometricInterpretation.WhiteIsZero;
                }
            }
            set
            {
                this.Add(Tag.PhotometricInterpretation, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag PlanarConfiguration
        /// </summary>
        public PlanarConfiguration PlanarConfiguration
        {
            get
            {
                if (this.Contains(Tag.PlanarConfiguration))
                {
                    return (PlanarConfiguration)this[Tag.PlanarConfiguration].Value;
                }
                else
                {
                    return PlanarConfiguration.Chunky;
                }
            }
            set
            {
                this.Add(Tag.PlanarConfiguration, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ResolutionUnit
        /// </summary>
        public ResolutionUnit ResolutionUnit
        {
            get
            {
                if (this.Contains(Tag.ResolutionUnit))
                {
                    return (ResolutionUnit)this[Tag.ResolutionUnit].Value;
                }
                else
                {
                    return ResolutionUnit.Inch;
                }
            }
            set
            {
                this.Add(Tag.ResolutionUnit, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag RowsPerStrip
        /// </summary>
        public uint RowsPerStrip
        {
            get
            {
                if (this.Contains(Tag.RowsPerStrip))
                {
                    return Convert.ToUInt32(this[Tag.RowsPerStrip].Value);
                }
                else
                {
                    return uint.MaxValue;
                }
            }
            set
            {
                this.Add(Tag.RowsPerStrip, value);
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
                if (this.Contains(Tag.SamplesPerPixel))
                {
                    return (ushort)this[Tag.SamplesPerPixel].Value;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                this.Add(Tag.SamplesPerPixel, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag Software
        /// </summary>
        public string Software
        {
            get
            {
                return GetAsciiFieldValue(Tag.Software);
            }
            set
            {
                SetAsciiFieldValue(Tag.Software, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag StripByteCounts
        /// </summary>
        public uint[] StripByteCounts
        {
            get
            {
                if (this.Contains(Tag.StripByteCounts))
                {
                    return (uint[])this[Tag.StripByteCounts].Values;
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

                this.Add(Tag.StripByteCounts, FieldType.Long, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag StripOffsets
        /// </summary>
        public uint[] StripOffsets
        {
            get
            {
                if (this.Contains(Tag.StripOffsets))
                {
                    return (uint[])this[Tag.StripOffsets].Values;
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

                this.Add(Tag.StripOffsets, FieldType.Long, value);
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
                if (this.Contains(Tag.Threshholding))
                {
                    return (Threshholding)this[Tag.Threshholding].Value;
                }
                else
                {
                    return Threshholding.None;
                }
            }
            set
            {
                this.Add(Tag.Threshholding, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag XResolution
        /// </summary>
        public URational32 XResolution
        {
            get
            {
                if (this.Contains(Tag.XResolution))
                {
                    return (URational32)this[Tag.XResolution].Value;
                }
                else
                {
                    return default(URational32);
                }
            }
            set
            {
                this.Add(Tag.XResolution, value);
            }
        }

        /// <summary>
        /// Gets or sets the values of baseline tag YResolution
        /// </summary>
        public URational32 YResolution
        {
            get
            {
                if (this.Contains(Tag.YResolution))
                {
                    return (URational32)this[Tag.YResolution].Value;
                }
                else
                {
                    return default(URational32);
                }
            }
            set
            {
                this.Add(Tag.YResolution, value);
            }
        }
    }
}
