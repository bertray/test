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
    public class DataCellValidation
    {
        public string Column { set; get; }
        public string Validator { set; get; }
        public bool Mandatory { set; get; }
        public bool Ignored { set; get; }
    }
}
