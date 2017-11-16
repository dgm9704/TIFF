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

        public NewSubfileType NewSubfileType
        {
            get => this.TagValueOrDefault<NewSubfileType>(TagType.NewSubfileType, NewSubfileType.None);
            set => this.Add(TagType.NewSubfileType, value);
        }

        public Orientation Orientation
        {
            get => this.TagValueOrDefault<Orientation>(TagType.Orientation, Orientation.TopLeft);
            set => this.Add(TagType.Orientation, value);
        }

        public PhotometricInterpretation PhotometricInterpretation
        {
            get => this.TagValueOrDefault<PhotometricInterpretation>(TagType.PhotometricInterpretation, PhotometricInterpretation.WhiteIsZero);
            set => this.Add(TagType.PhotometricInterpretation, value);
        }

        public PlanarConfiguration PlanarConfiguration
        {
            get => this.TagValueOrDefault<PlanarConfiguration>(TagType.PlanarConfiguration, PlanarConfiguration.Chunky);
            set => this.Add(TagType.PlanarConfiguration, value);
        }

        public ResolutionUnit ResolutionUnit
        {
            get => this.TagValueOrDefault<ResolutionUnit>(TagType.ResolutionUnit, ResolutionUnit.Inch);
            set => this.Add(TagType.ResolutionUnit, value);
        }

        public uint RowsPerStrip
        {
            get => this.TagValueOrDefault<uint>(TagType.RowsPerStrip, uint.MaxValue);
            set => this.Add(TagType.RowsPerStrip, value);
        }

        public uint StripsPerImage
        => (this.ImageLength + this.RowsPerStrip - 1) / RowsPerStrip;

        public ushort SamplesPerPixel
        {
            get => this.TagValueOrDefault<ushort>(TagType.SamplesPerPixel);
            set => this.Add(TagType.SamplesPerPixel, value);
        }

        public string Software
        {
            get => GetAsciiFieldValue(TagType.Software);
            set => SetAsciiFieldValue(TagType.Software, value);
        }

        public uint[] StripByteCounts
        {
            get => this.Contains(TagType.StripByteCounts)
                ? (uint[])this[TagType.StripByteCounts].Values
                : new uint[0];
            set => this.Add(TagType.StripByteCounts, FieldType.Long, value ?? new uint[0]);
        }

        public uint[] StripOffsets
        {
            get => this.Contains(TagType.StripOffsets)
                ? (uint[])this[TagType.StripOffsets].Values
                : new uint[0];
            set => this.Add(TagType.StripOffsets, FieldType.Long, value ?? new uint[0]);
        }

        public Threshholding Threshholding
        {
            get => this.TagValueOrDefault<Threshholding>(TagType.Threshholding, Threshholding.None);
            set => this.Add(TagType.Threshholding, value);
        }

        public URational32 XResolution
        {
            get => this.TagValueOrDefault<URational32>(TagType.XResolution);
            set => this.Add(TagType.XResolution, value);
        }

        public URational32 YResolution
        {
            get => this.TagValueOrDefault<URational32>(TagType.YResolution);
            set => this.Add(TagType.YResolution, value);
        }
    }
}
