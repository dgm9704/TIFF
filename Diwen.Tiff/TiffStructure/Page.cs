namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Diwen.Tiff.FieldValues;

    [Serializable]
    public class Page : Ifd
    {
        public Page() : base() { }

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
        => this.Add(tag, value ?? string.Empty);

        public string GetAsciiFieldValue(TagType tag)
        => this.Contains(tag)
            ? string.Join("\n", new string((char[])this[tag].Values).Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries))
            : string.Empty;

        public void Add(TagType tag, FieldType type, Array values)
        => this.Add(new Field(tag, type, values));

        public void Add(Tag tag)
        => this.Add(new Field(tag.TagType, (FieldType)tag.FieldType, tag.Values));

        public string Artist
        {
            get => GetAsciiFieldValue(TagType.Artist);
            set => SetAsciiFieldValue(TagType.Artist, value);
        }

        public ushort PageNumber
        {
            get => this.TagValueOrDefault<ushort>(TagType.PageNumber, 0);
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
            get => this.TagValueOrDefault<ushort>(TagType.BitsPerSample, 1);
            set => this.Add(TagType.BitsPerSample, value);
        }

        public ushort CellLength
        {
            get => this.TagValueOrDefault<ushort>(TagType.CellLength, 0);
            set => this.Add(TagType.CellLength, value);
        }

        public ushort CellWidth
        {
            get => this.TagValueOrDefault<ushort>(TagType.CellWidth, 0);
            set => this.Add(TagType.CellWidth, value);
        }

        public ushort[] ColorMap
        {
            get => this.TagValuesOrDefault<ushort>(TagType.ColorMap, new ushort[0]);
            set => this.Add(TagType.ColorMap, FieldType.Short, value);
        }

        public Compression Compression
        {
            get => this.TagValueOrDefault<Compression>(TagType.Compression, Compression.NoCompression);
            set => this.Add(TagType.Compression, value);
        }

        public string Copyright
        {
            get => GetAsciiFieldValue(TagType.Copyright);
            set => SetAsciiFieldValue(TagType.Copyright, value);
        }

        public string DateTime
        {
            get => GetAsciiFieldValue(TagType.DateTime);
            set => SetAsciiFieldValue(TagType.DateTime, value);
        }

        public ExtraSampleType[] ExtraSamples
        {
            get => this.TagValuesOrDefault<ExtraSampleType>(TagType.ExtraSamples, new ExtraSampleType[0]);
            set => this.Add(TagType.ExtraSamples, FieldType.Short, value ?? new ExtraSampleType[0]);
        }

        public FillOrder FillOrder
        {
            get => this.TagValueOrDefault<FillOrder>(TagType.FillOrder, FillOrder.LowBitsFirst);
            set => this.Add(TagType.FillOrder, value);
        }

        public uint FreeByteCounts
        {
            get => this.TagValueOrDefault<uint>(TagType.FreeByteCounts, 0);
            set => this.Add(TagType.FreeByteCounts, value);
        }

        public uint FreeOffsets
        {
            get => this.TagValueOrDefault<uint>(TagType.FreeOffsets, 0);
            set => this.Add(TagType.FreeOffsets, value);
        }

        public ushort[] GrayResponseCurve
        {
            get => this.TagValuesOrDefault<ushort>(TagType.GrayResponseCurve, new ushort[0]);
            set => this.Add(TagType.GrayResponseCurve, FieldType.Short, value);
        }

        public GrayResponseUnit GrayResponseUnit
        {
            get => this.TagValueOrDefault<GrayResponseUnit>(TagType.GrayResponseUnit, GrayResponseUnit.Hundredths);
            set => this.Add(TagType.GrayResponseUnit, value);
        }

        public string HostComputer
        {
            get => GetAsciiFieldValue(TagType.HostComputer);
            set => SetAsciiFieldValue(TagType.HostComputer, value);
        }

        public string ImageDescription
        {
            get => GetAsciiFieldValue(TagType.ImageDescription);
            set => SetAsciiFieldValue(TagType.ImageDescription, value);
        }

        public uint ImageLength
        {
            get => this.Contains(TagType.ImageLength)
                ? Convert.ToUInt32(this[TagType.ImageLength].Value) // TODO: check special case for this tag
                : 0;
            set => this.Add(TagType.ImageLength, value);
        }

        public uint ImageWidth
        {
            get => this.TagValueOrDefault<uint>(TagType.ImageWidth, 0);
            set => this.Add(TagType.ImageWidth, value);
        }

        public string Make
        {
            get => GetAsciiFieldValue(TagType.Make);
            set => SetAsciiFieldValue(TagType.Make, value);
        }

        private ushort DefaultMaxSampleValue()
        => (ushort)(Math.Pow(2, this.BitsPerSample) - 1);

        public ushort MaxSampleValue
        {
            get => this.TagValueOrDefault<ushort>(TagType.MaxSampleValue, this.DefaultMaxSampleValue());
            set => this.Add(TagType.MaxSampleValue, value);
        }

        public ushort MinSampleValue
        {
            get => this.TagValueOrDefault<ushort>(TagType.MinSampleValue, 0);
            set => this.Add(TagType.MinSampleValue, value);
        }

        public string Model
        {
            get => GetAsciiFieldValue(TagType.Model);
            set => SetAsciiFieldValue(TagType.Model, value);
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
            get => this.Contains(TagType.RowsPerStrip)
                ? Convert.ToUInt32(this[TagType.RowsPerStrip].Value) // TODO: check special case for this tag
                : uint.MaxValue;
            set => this.Add(TagType.RowsPerStrip, value);
        }

        public uint StripsPerImage
        => (this.ImageLength + this.RowsPerStrip - 1) / RowsPerStrip;

        public ushort SamplesPerPixel
        {
            get => this.TagValueOrDefault<ushort>(TagType.SamplesPerPixel, 1);
            set => this.Add(TagType.SamplesPerPixel, value);
        }

        public string Software
        {
            get => GetAsciiFieldValue(TagType.Software);
            set => SetAsciiFieldValue(TagType.Software, value);
        }

        public uint[] StripByteCounts
        {
            get => this.TagValuesOrDefault<uint>(TagType.StripByteCounts, new uint[0]);
            set => this.Add(TagType.StripByteCounts, FieldType.Long, value ?? new uint[0]);
        }

        public uint[] StripOffsets
        {
            get => this.TagValuesOrDefault<uint>(TagType.StripOffsets, new uint[0]);
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
