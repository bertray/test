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
    public class CSVRow
    {
        public CSVRow()
        {
            Data = new Dictionary<int, CSVCell>();
        }

        public int Index { set; get; }
        public IDictionary<int, CSVCell> Data { set; get; }
    }
}
