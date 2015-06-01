namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Represents an IFD (Image File Directory) of TIFF file
    /// </summary>
    [Serializable()]
    public class Page : TagCollection
    {
        /// <summary>
        /// Initializes a new instance of the Page class
        /// </summary>
        public Page()
            : base()
        {
        }

        //public int Number { get; set; }

        internal uint NextPageAddress { get; set; }

        internal List<byte[]> ImageData { get; set; }

        /// <summary>
        /// Returns a string representation of the page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            //sb.AppendFormat("-Page {0}: \r\n", this.Number);
            foreach (var tag in this)
            {
                sb.AppendLine(tag.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a deep copy of the Page object
        /// </summary>
        /// <returns></returns>
        public Page Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Page)formatter.Deserialize(stream);
            }
        }

        internal static Page Read(byte[] data, int pos)
        {
            var page = new Page();
            ushort tagCount = BitConverter.ToUInt16(data, pos);
            pos += 2;
            for (int i = 0; i < tagCount; i++)
            {
                page.Add(Tag.Read(data, pos));
                pos += 12;
            }

            page.NextPageAddress = BitConverter.ToUInt32(data, pos);

            var offsetTag = page[TagType.StripOffsets] ?? page[TagType.TileOffsets];
            var countTag = page[TagType.StripByteCounts] ?? page[TagType.TileByteCounts];
            page.ImageData = GetImageData(data, offsetTag, countTag);

            return page;
        }

        private static List<byte[]> GetImageData(byte[] data, Tag stripOffsetTag, Tag stripByteCountTag)
        {
            var stripData = new List<byte[]>();
            for (int i = 0; i < stripOffsetTag.Values.Length; i++)
            {
                long pos = (long)(uint)stripOffsetTag.Values.GetValue(i);
                long count = (long)(uint)stripByteCountTag.Values.GetValue(i);
                var strip = new byte[count];
                Array.Copy(data, pos, strip, 0, count);
                stripData.Add(strip);
            }

            return stripData;
        }

        /// <summary>
        /// adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="tiffDataType">data type</param>
        /// <param name="values">tag values</param>
        public void Add(TagType tagType, TiffDataType tiffDataType, Array values)
        {
            this.Add(new Tag(tagType, tiffDataType, values));
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="value">tag data</param>
        public void Add(TagType tagType, ushort value)
        {
            Add(tagType, TiffDataType.Short, new ushort[] { value });
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="value">tag data</param>
        public void Add(TagType tagType, uint value)
        {
            Add(tagType, TiffDataType.Long, new uint[] { value });
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="value">tag data</param>
        public void Add(TagType tagType, Enum value)
        {
            Type underType = Enum.GetUnderlyingType(value.GetType());

            switch (underType.Name)
            {
                case "UInt16":
                    Add(tagType, (ushort)Convert.ChangeType(value, underType));
                    break;

                case "UInt32":
                default:
                    Add(tagType, (uint)Convert.ChangeType(value, underType));
                    break;
            }
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="tiffDataType">type of data contained in the tag</param>
        /// <param name="value">tag data</param>
        public void Add(TagType tagType, TiffDataType tiffDataType, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            this.Add(tagType, tiffDataType, value.ToCharArray());
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="value">tag data</param>
        public void Add(TagType tagType, string value)
        {
            this.Add(tagType, TiffDataType.Ascii, value);
        }

        /// <summary>
        /// Adds a new tag to the page
        /// </summary>
        /// <param name="tagType">tag type</param>
        /// <param name="value">tag data</param>
        public void Add(TagType tagType, URational32 value)
        {
            Add(tagType, TiffDataType.Rational, new URational32[] { value });
        }

        private void SetAsciiTagValue(TagType tag, string value)
        {
            this.Add(tag, value ?? string.Empty);
        }

        private string GetAsciiTagValue(TagType tag)
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

        /// <summary>
        /// Gets or sets the value of baseline tag Artist
        /// </summary>
        public string Artist
        {
            get
            {
                return GetAsciiTagValue(TagType.Artist);
            }
            set
            {
                SetAsciiTagValue(TagType.Artist, value);
            }
        }

        /// <summary>
        /// Gets or sets the first component of tag PageNumber
        /// </summary>
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
                    this.Add(TagType.PageNumber, TiffDataType.Short, new ushort[] { value, 0 });
                }
            }
        }

        /// <summary>
        /// Gets or sets the second component of tag PageNumber
        /// </summary>
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
                    this.Add(TagType.PageNumber, TiffDataType.Short, new ushort[] { 0, value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag BitsPerSample
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of baseline tag CellLength
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of baseline tag CellWidth
        /// </summary>
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
                this.Add(TagType.ColorMap, TiffDataType.Short, value);
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
                return GetAsciiTagValue(TagType.Copyright);
            }
            set
            {
                SetAsciiTagValue(TagType.Copyright, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag DateTime
        /// </summary>
        public string DateTime
        {
            get
            {
                return GetAsciiTagValue(TagType.DateTime);
            }
            set
            {
                SetAsciiTagValue(TagType.DateTime, value);
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

                this.Add(TagType.ExtraSamples, TiffDataType.Short, value);
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
                    return (FillOrder)this[TagType.ExtraSamples].Value;
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
                this.Add(TagType.GrayResponseCurve, TiffDataType.Short, value);
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
                return GetAsciiTagValue(TagType.HostComputer);
            }
            set
            {
                SetAsciiTagValue(TagType.HostComputer, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of baseline tag ImageDescription
        /// </summary>
        public string ImageDescription
        {
            get
            {
                return GetAsciiTagValue(TagType.ImageDescription);
            }
            set
            {
                SetAsciiTagValue(TagType.ImageDescription, value);
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
                    return (uint)this[TagType.ImageLength].Value;
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
                return GetAsciiTagValue(TagType.Make);
            }
            set
            {
                SetAsciiTagValue(TagType.Make, value);
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
                return GetAsciiTagValue(TagType.Model);
            }
            set
            {
                SetAsciiTagValue(TagType.Model, value);
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
                    return (uint)this[TagType.RowsPerStrip].Value;
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
                return GetAsciiTagValue(TagType.Software);
            }
            set
            {
                SetAsciiTagValue(TagType.Software, value);
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
                this.Add(TagType.StripByteCounts, TiffDataType.Long, value);
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
                this.Add(TagType.StripOffsets, TiffDataType.Long, value);
            }
        }

        ///// <summary>
        ///// Gets or sets the values of baseline tag SubfileType
        ///// </summary>
        //public SubfileType SubfileType
        //{
        //    get
        //    {
        //        if (this.Contains(TagType.SubfileType))
        //        {
        //            return (SubfileType)this[TagType.SubfileType].Value;
        //        }
        //        else
        //        {
        //            return default(SubfileType);
        //        }
        //    }
        //    set
        //    {
        //        this.Add(TagType.SubfileType, value);
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
