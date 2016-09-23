using System;
using System.IO;

namespace SVCore
{    
    public class SVSerialize : MemoryStream
    {
        public SVSerialize()
        {
        }

        public SVSerialize(byte[] buffer) : base(buffer)
        {
        }

        public SVSerialize pack(byte value)
        {
            this.WriteByte(value);
            return this;
        }

        public SVSerialize pack(UInt16 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(UInt32 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(UInt64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(Int16 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(Int32 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(Int64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(Single value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(Double value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            this.Write(buffer, 0, buffer.Length);
            return this;
        }

        public SVSerialize pack(Byte[] value)
        {
            this.Write(value, 0, value.Length);
            return this;
        }

        //-----------------------------------------------------------------------

        public SVSerialize unpack(ref byte value)
        {
            value = Convert.ToByte(this.ReadByte());
            return this;
        }

        public SVSerialize unpack(ref UInt16 value)
        {
            int len = sizeof(UInt16);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToUInt16(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref UInt32 value)
        {
            int len = sizeof(UInt32);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToUInt32(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref UInt64 value)
        {
            int len = sizeof(UInt64);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToUInt64(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref Int16 value)
        {
            int len = sizeof(Int16);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToInt16(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref Int32 value)
        {
            int len = sizeof(Int32);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToInt32(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref Int64 value)
        {
            int len = sizeof(Int64);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToInt64(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref Single value)
        {
            int len = sizeof(Single);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToSingle(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref Double value)
        {
            int len = sizeof(Single);
            byte[] buffer = new byte[len];
            this.Read(buffer, 0, len);

            value = BitConverter.ToDouble(buffer, 0);
            return this;
        }

        public SVSerialize unpack(ref Byte[] value)
        {
            this.Read(value, 0, value.Length);
            return this;
        }
    }
}
