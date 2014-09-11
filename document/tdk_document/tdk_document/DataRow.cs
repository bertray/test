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
using System.Dynamic;

namespace Toyota.Common.Document
{
    public class DataRow
    {
        public int Index { set; get; }
        public IDictionary<int, DataColumn> Columns { set; get; }
        public IDictionary<int, DataCell> Cells { set; get; }

        public dynamic CellValueMap
        {
            get
            {
                var map = new ExpandoObject() as IDictionary<string, object>;
                if ((Cells != null) && (Cells.Count > 0))
                {
                    foreach (DataCell cell in Cells.Values)
                    {
                        map.Add(cell.Column.Name.Replace(' ', '_'), cell.Value);
                    }
                }
                return map;
            }
        }
    }
}
