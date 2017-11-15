namespace Diwen.Tiff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    [Serializable]
    public class Field
    {

        #region FieldValueReaders

        private static Dictionary<FieldType, FieldValueReader> fieldValueReaders =
            new Dictionary<FieldType, FieldValueReader>
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

        private delegate Array FieldValueReader(int count,byte[] data,bool flip);

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

        private static Field Create(Tag tag, FieldType type, int count, byte[] data, bool flip)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Array values = null;

            if (fieldValueReaders.ContainsKey(type))
            {
                //TODO: Move the flippin flippin here
                values = fieldValueReaders[type](count, data, flip);
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
            sb.AppendFormat("{0}\t{1}\t", this.Tag, this.FieldType);

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
                if (this.Count > 10)
                {
                    sb.AppendFormat("({0})", this.Count);
                    return sb.ToString() + "]";
                }
                else
                {
                    this.AppendValues(sb);
                    return sb.ToString().Remove(sb.Length - 1) + "]";
                }

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

        internal static Field Read(byte[] data, int startPosition, bool flip)
        {
            Tag tag = (Tag)BitConverter.ToUInt16(Tif.GetBytes(data, startPosition, 2, flip), 0);
            FieldType type = (FieldType)BitConverter.ToUInt16(Tif.GetBytes(data, startPosition + 2, 2, flip), 0);
            if (!Enum.IsDefined(typeof(FieldType), type))
            {
                return null;
            }
            uint count = BitConverter.ToUInt32(Tif.GetBytes(data, startPosition + 4, 4, flip), 0);
            uint offset = BitConverter.ToUInt32(data, startPosition + 8);
            byte[] valueBytes = GetValueBytes(data, count, type, offset, flip);
            return Field.Create(tag, type, (int)count, valueBytes, flip);
        }

        internal static Enum EnumeratedFieldValue(Type enumType, string value)
        {
            if (enumType != null)
            {
                return (Enum)Enum.Parse(enumType, value);
            }
            else
            {
                return null;
            }
        }

        private static byte[] GetValueBytes(byte[] data, uint valueCount, FieldType type, uint offset, bool flip)
        {


            byte[] valuebytes;
            int valueLength = (int)valueCount * Tif.GetValueLength(type);

            if (data == null || valueLength == 0 || data.Length < valueLength)
            {
                return new byte[] { };
            }

            if (valueLength > 4)
            {
                if (flip)
                {
                    offset = Tif.SwapUInt32(offset);
                }
                valuebytes = new byte[valueLength];
                Buffer.BlockCopy(data, (int)offset, valuebytes, 0, valueLength);
            }
            else
            {
                valuebytes = BitConverter.GetBytes(offset);
            }

            return valuebytes;
        }

        private static double[] ReadDoubleValues(int count, byte[] data, bool flip)
        {
            if (flip)
            {
                Array.Reverse(data);
            }
            var values = new double[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 8);
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static float[] ReadFloatValues(int count, byte[] data, bool flip)
        {
            if (flip)
            {
                Array.Reverse(data);
            }
            var values = new float[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 4);
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static Rational32[] ReadSRationalValues(int count, byte[] data, bool flip)
        {
            if (flip)
            {
                Array.Reverse(data);
            }
            var values = new Rational32[count];
            for (int i = 0; i < count; i++)
            {
                var value = new Rational32(data, i * 4);
                if (flip)
                {
                    value = value.Inverse();
                }
                values.SetValue(value, i);
            }
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static URational32[] ReadRationalValues(int count, byte[] data, bool flip)
        {
            if (flip)
            {
                Array.Reverse(data);
            }
            var values = new URational32[count];
            for (int i = 0; i < count; i++)
            {
                var value = new URational32(data, i * 4);
                if (flip)
                {
                    value = value.Inverse();
                }
                values.SetValue(value, i);
            }
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static int[] ReadSLongValues(int count, byte[] data, bool flip)
        {
            if (data == null || data.Length < count * 4)
            {
                return new int[] { };
            }
            if (flip)
            {
                Array.Reverse(data);
            }
            var values = new int[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 4);
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static uint[] ReadLongValues(int count, byte[] data, bool flip)
        {

            if (data == null || data.Length < count * 4)
            {
                return new uint[] { };
            }
            if (flip)
            {
                Array.Reverse(data);
            }
            var values = new uint[count];
            Buffer.BlockCopy(data, 0, values, 0, count * 4);
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static short[] ReadSShortValues(int count, byte[] data, bool flip)
        {
            int pos = 0;
            if (flip)
            {
                Array.Reverse(data);
                pos = data.Length - count * 2;
            }
            var values = new short[count];
            Buffer.BlockCopy(data, pos, values, 0, count * 2);
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static ushort[] ReadShortValues(int count, byte[] data, bool flip)
        {
            int pos = 0;
            if (flip)
            {
                Array.Reverse(data);
                pos = data.Length - count * 2;
            }
            var values = new ushort[count];
            Buffer.BlockCopy(data, pos, values, 0, count * 2);
            if (flip)
            {
                Array.Reverse(values);
            }
            return values;
        }

        private static char[] ReadAsciiValues(int count, byte[] data, bool flip)
        {
            var values = Tif.Ascii.GetString(data).ToCharArray();
            return values;
        }

        private static sbyte[] ReadSByteValues(int count, byte[] data, bool flip)
        {
            var values = new sbyte[data.Length];
            Buffer.BlockCopy(data, 0, values, 0, count);
            return values;
        }

        private void AppendValues(StringBuilder sb)
        {
            Type enumType = Type.GetType("Diwen.Tiff.FieldValues." + this.Tag);

            foreach (var value in this.Values)
            {
                string v = value.ToString();
                Enum temp = EnumeratedFieldValue(enumType, v);
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
            Type enumType = Type.GetType("Diwen.Tiff.FieldValues." + this.Tag);
            Enum temp = EnumeratedFieldValue(enumType, v);
            if (temp != null)
            {
                v = temp.ToString();
            }

            sb.Append(v);
        }

        private string AppendAsciiValue(StringBuilder sb)
        {
            string value = new string(this.Values as char[]);
            var values = value.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

            sb.Append(string.Join("|", values));
            return sb.ToString();
        }

        internal bool IsIfdField()
        {
            return new List<Tag> { Tag.ExifIFD }.Contains(this.Tag);
        }
    }
}