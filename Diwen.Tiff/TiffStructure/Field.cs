namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Represents a TIFF Field ie. an IFD Entry ie. a Tag
    /// </summary>
    [Serializable()]
    public class Field
    {

        #region FieldValueReaders
        private static Dictionary<FieldType, TagValueReader> tagValueReaders =
            new Dictionary<FieldType, TagValueReader> 
            { 
                { FieldType.SByte, ReadSByteValues },
                { FieldType.Ascii, ReadAsciiValues },
                { FieldType.Short, ReadShortValues },
                { FieldType.SShort, ReadSShortValues },
                { FieldType.Long, ReadLongValues },
                { FieldType.SLong, ReadSLongValues },
                { FieldType.Rational, ReadRationalValues },
                { FieldType.SRational, ReadSRationalValues },
                { FieldType.Float, ReadFloatValues },
                { FieldType.Double, ReadDoubleValues },
            };

        #endregion

        /// <summary>
        /// Initializes a new instance of the field class
        /// </summary>
        public Field()
        {
        }

        /// <summary>
        /// Initializes a new instance of the field class with the given properties
        /// </summary>
        /// <param name="tag">tag type</param>
        /// <param name="type">data type</param>
        /// <param name="values">values</param>
        public Field(Tag tag, FieldType type, Array values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            this.Tag = tag;
            this.FieldType = type;
            this.Values = values;
            this.Count = (uint)values.Length;
        }

        private delegate Array TagValueReader(int count, byte[] data);

        /// <summary>
        /// Returns the values of the field object
        /// </summary>
        public Array Values { get; private set; }

        /// <summary>
        /// Return the value of the field object
        /// If the field has more than one value, then the first value is returned.
        /// </summary>
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
                    this.Values = new object[] { value };
                }
            }
        }

        /// <summary>
        /// Gets or sets the field type of the field
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Gets or sets the data type of the field
        /// </summary>
        public FieldType FieldType { get; set; }

        internal uint Count { get; set; }

        internal uint ValueOffset { get; set; }

        private static Field Create(Tag tag, FieldType type, int count, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Array values = null;

            if (tagValueReaders.ContainsKey(type))
            {
                values = tagValueReaders[type](count, data);
            }
            else
            {
                values = data;
            }

            return new Field
            {
                Tag = tag,
                FieldType = type,
                Values = values,
                Count = (uint)values.Length,
            };
        }

        /// <summary>
        /// Returns a string representation of the field
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}\t", this.Tag);
            
            if (this.FieldType == FieldType.Ascii)
            {
                return this.AppendAsciiValue(sb);
            }
            else if (this.Count == 1)
            {
                AppendSingleValue(sb);
                return sb.ToString();
            }
            else
            {
                sb.Append("[");
                this.AppendValues(sb);
                return sb.ToString().Remove(sb.Length - 1) + "]";
            }
        }

        /// <summary>
        /// Returns a deep copy of this Tag instance
        /// </summary>
        /// <returns></returns>
        public Field Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Field)formatter.Deserialize(stream);
            }
        }

        internal static Field Read(byte[] data, int startPosition)
        {
            Tag tag = (Tag)BitConverter.ToUInt16(data, startPosition);
            FieldType type = (FieldType)BitConverter.ToUInt16(data, startPosition + 2);
            uint count = BitConverter.ToUInt32(data, startPosition + 4);
            uint offset = BitConverter.ToUInt32(data, startPosition + 8);
            byte[] valueBytes = GetValueBytes(data, count, type, offset);
            return Field.Create(tag, type, (int)count, valueBytes);
        }

        internal static Enum EnumeratedFieldValue(string name, string value)
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

        private static byte[] GetValueBytes(byte[] data, uint valueCount, FieldType type, uint offset)
        {
            byte[] valuebytes;
            int valueLength = (int)valueCount * Tif.GetValueLength(type);
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
            var values = Tif.Ascii.GetString(data).ToCharArray();
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
                Enum temp = EnumeratedFieldValue("Diwen.Tiff.FieldValues." + this.Tag, v);
                if (temp != null)
                {
                    v = temp.ToString();
                }

                sb.Append(v);
                sb.Append(",");
            }
        }

        private void AppendSingleValue(StringBuilder sb)
        {
            string v = this.Value.ToString();
            Enum temp = EnumeratedFieldValue("Diwen.Tiff.FieldValues." + this.Tag, v);
            if (temp != null)
            {
                v = temp.ToString();
            }

            sb.Append(v);
        }

        private string AppendAsciiValue(StringBuilder sb)
        {
            sb.Append(this.Values as char[]);
            return sb.ToString();
        }
    }
}