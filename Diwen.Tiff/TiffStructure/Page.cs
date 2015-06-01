namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    [Serializable()]
    public class Page
    {
        public Page()
        {
            this.Tags = new TagCollection();
        }

        public int Number { get; set; }

        public TagCollection Tags { get; internal set; }

        internal uint NextPageAddress { get; set; }

        internal List<byte[]> ImageData { get; set; }

        public Tag this[TagType tagType]
        {
            get
            {
                return this.Tags[tagType];
            }

            set
            {
                this.Tags.Insert(0, value);
            }
        }

        public void Add(Tag item)
        {
            this.Tags.Add(item);
        }

        public void AddRange(IEnumerable<Tag> items)
        {
            if (items != null)
            {
                this.Tags.AddRange(items);
            }
        }

        public bool HasTag(TagType type)
        {
            return this.Tags.Contains(type);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("-Page {0}: \r\n", this.Number);
            foreach (var tag in this.Tags)
            {
                sb.AppendLine(tag.ToString());
            }

            return sb.ToString();
        }

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
                page.Tags.Add(Tag.Read(data, pos));
                pos += 12;
            }

            page.NextPageAddress = BitConverter.ToUInt32(data, pos);

            var offsetTag = page.Tags[TagType.StripOffsets] ?? page.Tags[TagType.TileOffsets];
            var countTag = page.Tags[TagType.StripByteCounts] ?? page.Tags[TagType.TileByteCounts];
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

        public void Add(TagType tagType, TiffDataType tiffDataType, Array values)
        {
            this.Add(new Tag(tagType, tiffDataType, values));
        }

        public void Add(TagType tagType, ushort value)
        {
            Add(tagType, TiffDataType.Short, new ushort[] { value });
        }

        private void Add(TagType tagType, uint value)
        {
            Add(tagType, TiffDataType.Long, new uint[] { value });
        }

        private void Add(TagType tagType, Enum value)
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

        public void Add(TagType tagType, TiffDataType tiffDataType, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            this.Add(tagType, tiffDataType, value.ToCharArray());
        }

        public void Add(TagType tagType, string value)
        {
            this.Add(tagType, TiffDataType.Ascii, value);
        }

        private void Add(TagType tagType, URational32 value)
        {
            Add(tagType, TiffDataType.Rational, new URational32[] { value });
        }

        public string Artist
        {
            get
            {
                if (this.HasTag(TagType.Artist))
                {
                    return new string((char[])this[TagType.Artist].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.Artist, value);
            }
        }

        public ushort PageNumber
        {
            get
            {
                if (this.HasTag(TagType.PageNumber))
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
                if (this.HasTag(TagType.PageNumber))
                {
                    this[TagType.PageNumber].Values.SetValue(value, 0);
                }
                else
                {
                    this.Add(TagType.PageNumber, TiffDataType.Short, new ushort[] { value, 0 });
                }
            }
        }

        public ushort PageTotal
        {
            get
            {
                if (this.HasTag(TagType.PageNumber))
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
                if (this.HasTag(TagType.PageNumber))
                {
                    this[TagType.PageNumber].Values.SetValue(value, 1);
                }
                else
                {
                    this.Add(TagType.PageNumber, TiffDataType.Short, new ushort[] { 0, value });
                }
            }
        }

        public ushort BitsPerSample
        {
            get
            {
                if (this.HasTag(TagType.BitsPerSample))
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
                if (this.HasTag(TagType.CellLength))
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
                if (this.HasTag(TagType.CellWidth))
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

        public ushort[] ColorMap
        {
            get
            {
                if (this.HasTag(TagType.ColorMap))
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

        public Compression Compression
        {
            get
            {
                if (this.HasTag(TagType.Compression))
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

        public string Copyright
        {
            get
            {
                if (this.HasTag(TagType.Copyright))
                {
                    return new string((char[])this[TagType.Copyright].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.Copyright, value);
            }
        }

        public string DateTime
        {
            get
            {
                if (this.HasTag(TagType.DateTime))
                {
                    return new string((char[])this[TagType.DateTime].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.DateTime, value);
            }
        }

        public ExtraSampleType[] ExtraSamples
        {
            get
            {
                if (this.HasTag(TagType.ExtraSamples))
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

        public FillOrder FillOrder
        {
            get
            {
                if (this.HasTag(TagType.FillOrder))
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

        public uint FreeByteCounts
        {
            get
            {
                if (this.HasTag(TagType.FreeByteCounts))
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

        public uint FreeOffsets
        {
            get
            {
                if (this.HasTag(TagType.FreeOffsets))
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

        public ushort[] GrayResponseCurve
        {
            get
            {
                if (this.HasTag(TagType.GrayResponseCurve))
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

        public GrayResponseUnit GrayResponseUnit
        {
            get
            {
                if (this.HasTag(TagType.GrayResponseUnit))
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

        public string HostComputer
        {
            get
            {
                if (this.HasTag(TagType.HostComputer))
                {
                    return new string((char[])this[TagType.HostComputer].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.HostComputer, value);
            }
        }

        public string ImageDescription
        {
            get
            {
                if (this.HasTag(TagType.ImageDescription))
                {
                    return new string((char[])this[TagType.ImageDescription].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.ImageDescription, value);
            }
        }

        public uint ImageLength
        {
            get
            {
                if (this.HasTag(TagType.ImageLength))
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

        public uint ImageWidth
        {
            get
            {
                if (this.HasTag(TagType.ImageWidth))
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

        public string Make
        {
            get
            {
                if (this.HasTag(TagType.Make))
                {
                    return new string((char[])this[TagType.Make].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.Make, value);
            }
        }

        public ushort MaxSampleValue
        {
            get
            {
                if (this.HasTag(TagType.MaxSampleValue))
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

        public ushort MinSampleValue
        {
            get
            {
                if (this.HasTag(TagType.MinSampleValue))
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

        public string Model
        {
            get
            {
                if (this.HasTag(TagType.Model))
                {
                    return new string((char[])this[TagType.Model].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.Model, value);
            }
        }

        public NewSubfileType NewSubfileType
        {
            get
            {
                if (this.HasTag(TagType.NewSubfileType))
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

        public Orientation Orientation
        {
            get
            {
                if (this.HasTag(TagType.Orientation))
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

        public PhotometricInterpretation PhotometricInterpretation
        {
            get
            {
                if (this.HasTag(TagType.PhotometricInterpretation))
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

        public PlanarConfiguration PlanarConfiguration
        {
            get
            {
                if (this.HasTag(TagType.PlanarConfiguration))
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

        public ResolutionUnit ResolutionUnit
        {
            get
            {
                if (this.HasTag(TagType.ResolutionUnit))
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

        public uint RowsPerStrip
        {
            get
            {
                if (this.HasTag(TagType.RowsPerStrip))
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

        public uint StripsPerImage
        {
            get
            {
                return (this.ImageLength + this.RowsPerStrip - 1) / RowsPerStrip;
            }
        }

        public ushort SamplesPerPixel
        {
            get
            {
                if (this.HasTag(TagType.SamplesPerPixel))
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

        public string Software
        {
            get
            {
                if (this.HasTag(TagType.Software))
                {
                    return new string((char[])this[TagType.Software].Values);
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.Add(TagType.Software, value);
            }
        }

        public uint[] StripByteCounts
        {
            get
            {
                if (this.HasTag(TagType.StripByteCounts))
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

        public uint[] StripOffsets
        {
            get
            {
                if (this.HasTag(TagType.StripOffsets))
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

        public SubfileType SubfileType
        {
            get
            {
                if (this.HasTag(TagType.SubfileType))
                {
                    return (SubfileType)this[TagType.SubfileType].Value;
                }
                else
                {
                    return default(SubfileType);
                }
            }
            set
            {
                this.Add(TagType.SubfileType, value);
            }
        }

        public Threshholding Threshholding
        {
            get
            {
                if (this.HasTag(TagType.Threshholding))
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

        public URational32 XResolution
        {
            get
            {
                if (this.HasTag(TagType.XResolution))
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

        public URational32 YResolution
        {
            get
            {
                if (this.HasTag(TagType.YResolution))
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
