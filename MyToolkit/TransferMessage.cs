using System;
using System.Collections.Generic;
using System.Linq;

namespace MyToolkit
{
    public class PackagedMessage
    {
        public TransferType Type { get; set; }
        public ushort DataLength => (ushort)Data.Length;
        public ushort ArgsLength => (ushort)Args.Length;
        public byte[] Data { get; set; } = new byte[0];
        public byte[] Args { get; set; } = new byte[0];

        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte)Type);
            bytes.AddRange(BitConverter.GetBytes(DataLength));
            bytes.AddRange(BitConverter.GetBytes(ArgsLength));
            bytes.AddRange(Data);
            bytes.AddRange(Args);
            return bytes.ToArray();
        }
    }

    public class UnpackagedMessage
    {
        public TransferType Type { get; }
        public ushort DataLength { get; }
        public ushort ArgsLength { get; }
        public byte[] Data { get; }
        public byte[] Args { get; }

        public UnpackagedMessage(IEnumerable<byte> bytes)
        {
            var array = bytes.ToArray();
            Type = (TransferType)array[0];
            DataLength = BitConverter.ToUInt16(array, 1);
            ArgsLength = BitConverter.ToUInt16(array, 3);
            Data = array.Skip(5).Take(DataLength).ToArray();
            Args = array.Skip(5 + DataLength).Take(ArgsLength).ToArray();
        }
    }

    public enum TransferType : byte
    {
        Text, FileInfo, FileContent, FileEnd, Picture, Shake,
    }
}
