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
using Toyota.Common.Document;
using System.IO;

namespace Toyota.Common.Document
{
    public interface ITabularDataFileParserFactory
    {
        TabularDataFileParser Create(Stream stream);
        TabularDataFileParser Create(byte[] dataBytes);
        TabularDataFileParser Create(string path);
    }
}
