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
using System.Net;

namespace Toyota.Common.Utilities
{
    public class FtpClient
    {
        public FtpClient()
        {
            ReadingBufferSize = 1024;
        }

        public string FtpAddress { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public int ReadingBufferSize { set; get; }

        public void Upload(string filePath, string remotePath)
        {
            FileStream stream = File.OpenRead(filePath);
            try
            {
                Upload(stream, remotePath);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }                
            }
        }
        public void Upload(FileStream stream, string remotePath)
        {
            _CheckFtpAddress();

            CreateDirectory(remotePath);
            string remoteFilePath = remotePath;
            if (!string.IsNullOrEmpty(remoteFilePath))
            {
                if (remoteFilePath.EndsWith("/"))
                {
                    remoteFilePath += Path.GetFileName(stream.Name);
                }
                else
                {
                    remoteFilePath += "/" + Path.GetFileName(stream.Name);
                }
            }
            else
            {
                remoteFilePath = Path.GetFileName(stream.Name);
            }

            FtpWebRequest request = _CreateFtpRequest(_GetAbsoluteUrl(remoteFilePath), WebRequestMethods.Ftp.UploadFile);
            Stream requestStream = null;
            try
            {
                requestStream = request.GetRequestStream();
                stream.CopyTo(requestStream);
            }
            finally
            {
                if (!requestStream.IsNull())
                {
                    requestStream.Close();
                }                
            }            
        }

        public void Download(string path, string outputPath)
        {
            string targetPath = outputPath;
            if (targetPath.EndsWith("/"))
            {
                targetPath += Path.GetFileName(path);
            }
            else
            {
                targetPath += Path.DirectorySeparatorChar + Path.GetFileName(path);
            }
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            FileStream output = File.Create(targetPath);
            Download(path, output);
            output.Flush(); 
            output.Close();
        }
        public void Download(string path, FileStream output)
        {
            if (!string.IsNullOrEmpty(path) && (output != null))
            {
                FtpWebRequest request = _CreateFtpRequest(_GetAbsoluteUrl(path), WebRequestMethods.Ftp.DownloadFile);
                FtpWebResponse response = null;
                try
                {
                    response = (FtpWebResponse) request.GetResponse();
                    Stream responseStream = response.GetResponseStream();                    
                    responseStream.CopyTo(output);
                    responseStream.Close();
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            }
        }
        public TemporaryFileStream Download(string path)
        {
            string outputPath = Path.GetTempPath() + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(path) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(path);
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            FileStream output = File.Create(outputPath, ReadingBufferSize);
            Download(path, output);
            output.Flush();
            output.Close();

            return new TemporaryFileStream(outputPath, FileMode.Open);
        }

        public void CreateDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                _CheckFtpAddress();

                string _path = path;
                if (_path.StartsWith("/"))
                {
                    _path = _path.Substring(1, path.Length);
                }

                string[] pathFractions = _path.Split('/');

                FtpWebRequest request;
                FtpWebResponse response;
                bool exists;         
                string parentPath = string.Empty;
                for (int i = 0; i < pathFractions.Length; i++)
                {
                    if (i > 0)
                    {
                        parentPath += "/" + pathFractions[i-1];
                        exists = HasDirectory(parentPath, pathFractions[i]);
                    }
                    else
                    {
                        parentPath = null;
                        exists = HasDirectory(null, pathFractions[i]);
                    }

                    if (exists)
                    {
                        continue;
                    }

                    request = _CreateFtpRequest(_GetAbsoluteUrl(parentPath + "/" + pathFractions[i]), WebRequestMethods.Ftp.MakeDirectory);
                    response = null;
                    try
                    {
                        response = (FtpWebResponse)request.GetResponse();
                    }
                    finally
                    {
                        if (response != null)
                        {
                            response.Close();
                        }
                    }
                }
            }            
        }
        public IList<string> ListDirectories(string path)
        {
            FtpWebRequest request = _CreateFtpRequest(_GetAbsoluteUrl(path), WebRequestMethods.Ftp.ListDirectory);
            FtpWebResponse response = null;
            IList<string> result = new List<string>();
            try
            {
                response = (FtpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());                
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                }
                reader.Close();
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            if (!result.IsNullOrEmpty())
            {
                return result;
            }
            return null;
        }
        public bool HasDirectory(string parentPath, string directoryName)
        {
            if(!string.IsNullOrEmpty(directoryName)) 
            {
                IList<string> directoryList = ListDirectories(parentPath);
                if (!directoryList.IsNullOrEmpty())
                {
                    foreach (string path in directoryList)
                    {
                        if (path.EndsWith(directoryName))
                        {
                            return true;
                        }
                    }
                }
            }            
            return false;
        }

        private void _CheckFtpAddress()
        {
            if (string.IsNullOrEmpty(FtpAddress))
            {
                throw new Exception("Undefined FTP Address");
            }
        }
        private FtpWebRequest _CreateFtpRequest(string url, string method)
        {
            if (!string.IsNullOrEmpty(url)) 
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = method;
                if (!string.IsNullOrEmpty(Username))
                {
                    request.Credentials = new NetworkCredential(Username, string.IsNullOrEmpty(Password) ? string.Empty : Password);
                }
                return request;
            }
            return null;
        }
        private string _GetAbsoluteUrl(string path)
        {
            string url = FtpAddress;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("/"))
                {
                    url += path;
                }
                else
                {
                    url += "/" + path;
                }
            }

            if (url.EndsWith("/"))
            {
                return url.Substring(0, url.Length - 1);
            }
            return url;
        }
    }
}
