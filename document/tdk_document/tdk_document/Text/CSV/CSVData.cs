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

namespace Toyota.Common.Document.Text
{
    public class CSVData
    {
        public int Index { set; get; }
        public string Text { set; get; }
        public CSVRow Row { set; get; }
    }
}
