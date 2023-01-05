using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    public class DataTransfer
    {
        readonly Dictionary<string, MemoryStream> memoryStreamDic = new Dictionary<string, MemoryStream>();
        public readonly ConcurrentQueue<UnpackagedMessage> Cache = new ConcurrentQueue<UnpackagedMessage>();
        public readonly ManualResetEvent DataParseSwitch = new ManualResetEvent(false);
        public Action<string>? TextReceiveAction;
        public Action<MemoryStream>? MSReceiveAction;

        public DataTransfer()
        {

        }

        public void DataReceive()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    DataParseSwitch.WaitOne();
                    Thread.Sleep(10);
                    while (!Cache.IsEmpty)
                    {
                        Cache.TryDequeue(out var message);
                        if (message != null)
                            switch (message.Type)
                            {
                                case TransferType.Text:
                                    TextReceiveAction?.Invoke(EncodeToString(message.Data));
                                    break;
                                case TransferType.FileInfo:
                                    memoryStreamDic[EncodeToString(message.Args)] = new MemoryStream();
                                    break;
                                case TransferType.FileContent:
                                    lock (memoryStreamDic[EncodeToString(message.Args)])
                                        memoryStreamDic[EncodeToString(message.Args)].Write(message.Data);
                                    break;
                                case TransferType.FileEnd:
                                    MSReceiveAction?.Invoke(memoryStreamDic[EncodeToString(message.Args)]);
                                    memoryStreamDic[EncodeToString(message.Args)].Close();
                                    memoryStreamDic[EncodeToString(message.Args)].Dispose();
                                    memoryStreamDic.Remove(EncodeToString(message.Args));
                                    DataParseSwitch.Reset();
                                    break;
                            }
                    }
                }
            });
        }

        public string EncodeToString(IEnumerable<byte> data, string encode = "utf-8")
        {
            return Encoding.GetEncoding(encode).GetString(data.ToArray());
        }
    }
}
