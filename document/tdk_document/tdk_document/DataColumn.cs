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
    public class DataColumn
    {
        public int Index { set; get; }
        public string Name { set; get; }

        public IDataCellValidator CellValidator { set; get; }
    }
}
