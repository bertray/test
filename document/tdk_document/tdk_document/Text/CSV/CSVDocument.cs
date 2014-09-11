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

namespace Toyota.Common.Document.Text
{
    public abstract class CSVDocument: IDisposable
    {
        public CSVDocument(string filePath)
        {
            HeaderRow = 0;
            DataStartRow = 1;
            Headerless = false;
            Separator = ',';
            FilePath = filePath;
        }

        public abstract IDictionary<int, CSVHeader> GetHeader();
        public abstract IDictionary<int, CSVRow> GetRows();

        public string FilePath { set; get; }
        public int HeaderRow { set; get; }
        public int DataStartRow { set; get; }
        public bool Headerless { set; get; }
        public char Separator { set; get; }

        public void Dispose() { }
    }
}
