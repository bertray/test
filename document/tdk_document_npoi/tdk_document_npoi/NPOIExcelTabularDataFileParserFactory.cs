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
using Toyota.Common.Document.NPOI;
using Toyota.Common.Document;
using System.IO;

namespace Toyota.Common.Document.NPOI
{
    public class NPOIExcelTabularDataFileParserFactory: IExcelTabularDataFileParserFactory
    {
        private string resultPath;
        private string resultSuffix;

        public NPOIExcelTabularDataFileParserFactory(string resultPath, string resultSuffix)
        {
            this.resultPath = resultPath;
            this.resultSuffix = resultSuffix;
        }

        public TabularDataFileParser Create(System.IO.Stream stream)
        {
            TabularDataFileParser dataTable = new NPOIExcelTabularDataFileParser(stream, resultPath);
            dataTable.ValidationResultFileSuffix = resultSuffix;
            return dataTable;
        }

        public TabularDataFileParser Create(byte[] dataBytes)
        {
            TabularDataFileParser dataTable = new NPOIExcelTabularDataFileParser(new MemoryStream(dataBytes), resultPath);
            dataTable.ValidationResultFileSuffix = resultSuffix;
            return dataTable;
        }

        public TabularDataFileParser Create(string path)
        {
            TabularDataFileParser dataTable = new NPOIExcelTabularDataFileParser(path);
            dataTable.ResultPath = resultPath;
            dataTable.ValidationResultFileSuffix = resultSuffix;
            return dataTable;
        }
    }
}
