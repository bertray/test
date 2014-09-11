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

namespace Toyota.Common.Document
{
    public class DataCell
    {
        public object Value { set; get; }
        public DataCellType Type { set; get; }
        public DataColumn Column { set; get; }
        public DataRow Row { set; get; }
    }
}
