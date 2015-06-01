using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Diwen.Tiff;

namespace Diwen.Tiff
{
    [Serializable()]
    public class Tag
    {
        public Array Values { get; internal set; }
        public TagType TagType { get; set; }
        public TiffDataType DataType { get; set; }

        internal uint ValueCount { get; set; }
        internal uint ValueOffset { get; set; }

        public Tag() { }

        public Tag(TagType tagType, TiffDataType dataType, Array values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            this.TagType = tagType;
            this.DataType = dataType;
            this.Values = values;
            this.ValueCount = (uint)values.Length;
        }

        internal static Tag Read(byte[] data, int startPosition)
        {
            var tag = new Tag
            {
                TagType = (TagType)BitConverter.ToUInt16(data, startPosition),
                DataType = (TiffDataType)BitConverter.ToUInt16(data, startPosition + 2),
                ValueCount = BitConverter.ToUInt32(data, startPosition + 4),
                ValueOffset = BitConverter.ToUInt32(data, startPosition + 8),
            };

            byte[] valuebytes = GetValueBytes(data, tag);
            tag.Values = ReadValues(valuebytes, tag.DataType, (int)tag.ValueCount);

            return tag;
        }


        internal static Tag ReadMM(byte[] data, int startPosition)
        {
            var tag = new Tag
            {
                TagType = (TagType)(ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, startPosition)),
                DataType = (TiffDataType)(ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, startPosition + 2)),
                ValueCount = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, startPosition + 4)),

            };
            if ((int)tag.ValueCount * Tif.ValueLength[tag.DataType] > 4)
                tag.ValueOffset = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, startPosition + 8));
            else
                tag.ValueOffset = (uint)BitConverter.ToInt32(data, startPosition + 8);

            byte[] valuebytes = SwitchEndian(GetValueBytes(data, tag), Tif.ValueLength[tag.DataType]);

            tag.Values = ReadValues(valuebytes, tag.DataType, (int)tag.ValueCount);

            return tag;
        }

        private static byte[] GetValueBytes(byte[] data, Tag tag)
        {
            byte[] valuebytes;
            int valueLength = (int)tag.ValueCount * Tif.ValueLength[tag.DataType];
            if (valueLength > 4)
            {
                valuebytes = new byte[valueLength];
                Buffer.BlockCopy(data, (int)tag.ValueOffset, valuebytes, 0, valueLength);
            }
            else
            {
                valuebytes = BitConverter.GetBytes(tag.ValueOffset);
            }
            return valuebytes;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0:D}({0})", this.TagType);
            sb.Append("[");

            if (this.DataType == TiffDataType.Ascii)
            {
                sb.Append(Values as char[]);
                sb.Append("]");
                return sb.ToString();
            }

            foreach (var value in this.Values)
            {
                string v = value.ToString();
                Enum temp = EnumeratedTagValue("Tiff.TagValues." + this.TagType, v);
                if (temp != null)
                    v = temp.ToString();
                sb.Append(v);
                sb.Append(",");
            }
            return sb.ToString().Remove(sb.Length - 1) + "]";
        }

        internal static Enum EnumeratedTagValue(string name, string value)
        {
            Type enumType = System.Type.GetType(name);

            if (enumType != null)
                return (Enum)Enum.Parse(enumType, value);
            else
                return null;
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

        private static Array ReadValues(byte[] data, TiffDataType type, int count)
        {
            Array values = null;

            switch (type)
            {
                case TiffDataType.Byte:
                case TiffDataType.Undefined:
                    values = data;
                    break;
                case TiffDataType.SByte:
                    values = new sbyte[data.Length];
                    Buffer.BlockCopy(data, 0, values, 0, count);
                    break;
                case TiffDataType.Ascii:
                    values = Tif.Ascii.GetString(data).ToCharArray();
                    break;
                case TiffDataType.Short:
                    values = new ushort[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 2);
                    break;
                case TiffDataType.SShort:
                    values = new short[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 2);
                    break;
                case TiffDataType.Long:
                    values = new uint[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 4);
                    break;
                case TiffDataType.SLong:
                    values = new int[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 4);
                    break;
                case TiffDataType.Rational:
                    values = new URational32[count];
                    for (int i = 0; i < count; i++)
                        values.SetValue(new URational32(data, i * 4), i);
                    break;
                case TiffDataType.SRational:
                    values = new Rational32[count];
                    for (int i = 0; i < count; i++)
                        values.SetValue(new Rational32(data, i * 4), i);
                    break;
                case TiffDataType.Float:
                    values = new float[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 4);
                    break;
                case TiffDataType.Double:
                    values = new double[count];
                    Buffer.BlockCopy(data, 0, values, 0, count * 8);
                    break;
                default:
                    break;
            }

            return values;
        }

        private static byte[] SwitchEndian(byte[] data, int typeLength)
        {
            byte[] temp = new byte[typeLength];
            byte[] switched = new byte[data.Length];
            for (int i = 0; i < data.Length; i += typeLength)
            {
                Buffer.BlockCopy(data, i, temp, 0, typeLength);
                Array.Reverse(temp);
                Buffer.BlockCopy(temp, 0, switched, i, typeLength);
            }

            return switched;
        }

    }
}