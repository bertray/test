///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Toyota.Common.Utilities;

namespace Toyota.Common.DataExchange.Bus
{
    public class FileDataBus: DataBus
    {
        public const string PARAMETER_NAME = "_name";

        public FileDataBus(string name, string path): base(name)
        {
            BasePath = path;
            ReadingBufferSize = 1024;
        }
        public FileDataBus(string name): this(name, null) {}

        public string BasePath { set; get; }
        public int ReadingBufferSize { set; get; }

        public override void Push(DataPacket packet)
        {
            if (!string.IsNullOrEmpty(BasePath) && (packet != null))
            {
                if (!Directory.Exists(BasePath))
                {
                    Directory.CreateDirectory(BasePath);
                }
                
                if (packet.Data == null)
                {
                    return;
                }

                string filePath;
                FileStream data = (FileStream)packet.Data;
                if (packet.Parameters.ContainsKey(PARAMETER_NAME))
                {
                    filePath = BasePath + Path.DirectorySeparatorChar + packet.Parameters[PARAMETER_NAME];
                }
                else
                {
                    filePath = BasePath + Path.DirectorySeparatorChar + Path.GetFileName(data.Name);
                }

                WriteFile(packet, filePath);  
            }
        }
        public override DataPacket Pull(string id, IDictionary<string, object> parameters)
        {
            if ((parameters != null) && (parameters.ContainsKey(PARAMETER_NAME)))
            {
                string filePath = BasePath + Path.DirectorySeparatorChar + parameters[PARAMETER_NAME];
                if (File.Exists(filePath))
                {
                    DataPacket packet = new DataPacket(GetName());
                    packet.Data = File.OpenRead(filePath);
                    packet.Parameters.Merge(parameters);

                    return packet;
                }   
            }            

            return null;
        }

        public override void Close() { }
        public override void Open() { }

        protected  virtual void WriteFile(DataPacket packet, string targetPath) 
        {
            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
            FileStream fileStream = File.Create(targetPath);

            byte[] buffer = new byte[ReadingBufferSize];
            FileStream dataStream = (FileStream)packet.Data;            
            
            int readingSize;
            while (dataStream.Position < dataStream.Length)
            {
                if (dataStream.Length < ReadingBufferSize)
                {
                    readingSize = (int) dataStream.Length;
                }
                else
                {
                    readingSize = ReadingBufferSize;
                }

                Array.Clear(buffer, 0, readingSize);
                dataStream.Read(buffer, 0, readingSize);
                fileStream.Write(buffer, 0, readingSize);
            }

            dataStream.Close();
            fileStream.Flush();
            fileStream.Close();
        }
    }
}
