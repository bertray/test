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

namespace Toyota.Common.Document
{
    public abstract class ExcelTabularDataFileParser: TabularDataFileParser, IDisposable
    {
        public ExcelTabularDataFileParser(string path): base(path) 
        {
            Init();
        }
        public ExcelTabularDataFileParser(Stream stream, string resultPath): base(stream, resultPath) 
        {
            Init();
        }
        private void Init()
        {
            SheetNames = new List<string>();
        }
        
        public IList<string> SheetNames { private set; get; }
        public bool SaveValidationResultToOriginalFile { set; get; }
        protected bool UseAllSheet
        {
            get
            {
                return (SheetNames.Count == 0);
            }
        }
    }
}
