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
using System.Net;
using System.IO;
using Toyota.Common.Utilities;

namespace Toyota.Common.DataExchange
{
    public class FtpDataBus: DataBus
    {
        public const string PARAMETER_NAME = "_name";

        public FtpDataBus(string name) : this(name, null, null, null) { }
        public FtpDataBus(string name, string address, string username, string password): base(name)
        {
            Client = new FtpClient();

            FtpAddress = address;
            Username = username;
            Password = password;
            ReadingBufferSize = 1024;
        }

        public string FtpAddress
        {
            set
            {
                Client.FtpAddress = value;
            }
            get
            {
                return Client.FtpAddress;
            }
        }
        public string Username
        {
            set
            {
                Client.Username = value;
            }
            get
            {
                return Client.Username;
            }
        }
        public string Password
        {
            set
            {
                Client.Password = value;
            }
            get
            {
                return Client.Password;
            }
        }
        public string BasePath { set; get; }
        public int ReadingBufferSize
        {
            set
            {
                Client.ReadingBufferSize = value;
            }
            get
            {
                return Client.ReadingBufferSize;
            }
        }

        protected FtpClient Client { set; get; }

        public override void Push(DataPacket packet)
        {            
            if (string.IsNullOrEmpty(FtpAddress))
            {
                throw new Exception("Undefined FTP Address.");
            }

            if (packet != null)
            {   
                FileStream dataStream = (FileStream)packet.Data;
                if (dataStream == null)
                {
                    return;
                }

                Client.CreateDirectory(BasePath);
                Client.Upload(dataStream, BasePath);
            }
        }

        public override DataPacket Pull(string id, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(FtpAddress))
            {
                throw new Exception("Undefined FTP Address.");
            }

            if ((parameters != null) && parameters.ContainsKey(PARAMETER_NAME))
            {
                
                FileStream stream = Client.Download(BasePath + "/" + (string)parameters[PARAMETER_NAME]);
                DataPacket packet = new DataPacket(GetName());
                packet.Data = stream;
                packet.Parameters.Merge(parameters);
                
                return packet;
            }

            return null;
        }

        public override void Close() { }
        public override void Open() { }
    }
}
