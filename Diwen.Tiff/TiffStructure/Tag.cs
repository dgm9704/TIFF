namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    [Serializable()]
    public class Tag
    {

        #region TagValueReaders
        private static Dictionary<TiffDataType, TagValueReader> tagValueReaders =
            new Dictionary<TiffDataType, TagValueReader> 
            { 
                { TiffDataType.SByte, ReadSByteValues },
                { TiffDataType.Ascii, ReadAsciiValues },
                { TiffDataType.Short, ReadShortValues },
                { TiffDataType.SShort, ReadSShortValues },
                { TiffDataType.Long, ReadLongValues },
                { TiffDataType.SLong, ReadSLongValues },
                { TiffDataType.Rational, ReadRationalValues },
                { TiffDataType.SRational, ReadSRationalValues },
                { TiffDataType.Float, ReadFloatValues },
                { TiffDataType.Double, ReadDoubleValues },
            };

        #endregion

        public Tag()
        {
        }

        public Tag(TagType tagType, TiffDataType dataType, Array values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            this.TagType = tagType;
            this.DataType = dataType;
            this.Values = values;
            this.ValueCount = (uint)values.Length;
        }

        private delegate Array TagValueReader(int count, byte[] data);

        public Array Values { get; private set; }

        public object Value
        {
            get
            {
                if (this.Values == null || this.Values.Length == 0)
                {
                    return null;
                }

                return this.Values.GetValue(0);
            }
            set
            {
                if (this.Values == null)
                {
                    this.Values = new object[] {value };
                }
            }
        }

        public TagType TagType { get; set; }

        public TiffDataType DataType { get; set; }

        internal uint ValueCount { get; set; }

        internal uint ValueOffset { get; set; }

        public static Tag Create(TagType tagType, TiffDataType dataType, int count, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Array values = null;

            if (tagValueReaders.ContainsKey(dataType))
            {
                values = tagValueReaders[dataType](count, data);
            }
            else
            {
                values = data;
            }

            return new Tag
            {
                TagType = tagType,
                DataType = dataType,
                Values = values,
                ValueCount = (uint)values.Length,
            };
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0:D}({0})", this.TagType);
            sb.Append("[");
            if (this.DataType == TiffDataType.Ascii)
            {
                return this.AppendAsciiValue(sb);
            }
            else
            {
                this.AppendValues(sb);
                return sb.ToString().Remove(sb.Length - 1) + "]";
            }
        }

        public Tag Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Tag)formatter.Deserialize(stream);
            }
        }

        internal static Tag Read(byte[] data, int startPosition)
        {
            TagType tagType = (TagType)BitConverter.ToUInt16(data, startPosition);
            TiffDataType dataType = (TiffDataType)BitConverter.ToUInt16(data, startPosition + 2);
            uint count = BitConverter.ToUInt32(data, startPosition + 4);
            uint offset = BitConverter.ToUInt32(data, startPosition + 8);
            byte[] valueBytes = GetValueBytes(data, count, dataType, offset);
            return Tag.Create(tagType, dataType, (int)count, valueBytes);
        }

        internal static Enum EnumeratedTagValue(string name, string value)
        {
            Type enumType = Type.GetType(name);

            if (enumType != null)
            {
                return (Enum)Enum.Parse(enumType, value);
            }
            else
            {
                return null;
            }
        }

        private static byte[] GetValueBytes(byte[] data, uint valueCount, TiffDataType dataType, uint offset)
        {
            byte[] valuebytes;
            int valueLength = (int)valueCount * Tif.GetValueLength(dataType);
            if (valueLength > 4)
            {
                valuebytes = new byte[valueLength];
                Buffer.BlockCopy(data, (int)offset, valuebytes, 0, valueLength);
            }
            else
            {
                valuebytes = BitConverter.GetBytes(offset);
            }

            return valuebytes;
        }

        private static Array ReadDoubleValues(int count, byte[] data)
        {
            var values = new double[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 8);
            return values;
        }

        private static Array ReadFloatValues(int count, byte[] data)
        {
            var values = new float[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 4);
            return values;
        }

        private static Array ReadSRationalValues(int count, byte[] data)
        {
            var values = new Rational32[count];
            for (int i = 0; i < count; i++)
            {
                values.SetValue(new Rational32(data, i * 4), i);
            }

            return values;
        }

        private static Array ReadRationalValues(int count, byte[] data)
        {
            var values = new URational32[count];
            for (int i = 0; i < count; i++)
            {
                values.SetValue(new URational32(data, i * 4), i);
            }

            return values;
        }

        private static Array ReadSLongValues(int count, byte[] data)
        {
            var values = new int[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 4);
            return values;
        }

        private static Array ReadLongValues(int count, byte[] data)
        {
            var values = new uint[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 4);
            return values;
        }

        private static Array ReadSShortValues(int count, byte[] data)
        {
            var values = new short[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 2);
            return values;
        }

        private static Array ReadShortValues(int count, byte[] data)
        {
            var values = new ushort[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 2);
            return values;
        }

        private static Array ReadAsciiValues(int count, byte[] data)
        {
            var values = new char[data.Length];
            Buffer.BlockCopy(data, 0, values, 0, count);
            return values;
        }

        private static Array ReadSByteValues(int count, byte[] data)
        {
            var values = new sbyte[data.Length];
            Buffer.BlockCopy(data, 0, values, 0, count);
            return values;
        }

        private void AppendValues(StringBuilder sb)
        {
            foreach (var value in this.Values)
            {
                string v = value.ToString();
                Enum temp = EnumeratedTagValue("Tiff.TagValues." + this.TagType, v);
                if (temp != null)
                {
                    v = temp.ToString();
                }

                sb.Append(v);
                sb.Append(",");
            }
        }

        private string AppendAsciiValue(StringBuilder sb)
        {
            sb.Append(this.Values as char[]);
            sb.Append("]");
            return sb.ToString();
        }
    }
}