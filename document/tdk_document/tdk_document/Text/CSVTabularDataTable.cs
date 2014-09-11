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
    public class CSVTabularDataTable: TabularDataFileParser
    {
        public CSVTabularDataTable(string filePath) : base(filePath) { }

        public override DataRow GetDataRow(int rowIndex)
        {
            throw new NotImplementedException();
        }

        public override IList<DataCell> GetDataCells(string columnName)
        {
            throw new NotImplementedException();
        }

        public override DataCell GetDataCell(string columnName)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, DataColumn> GetHeaders()
        {
            throw new NotImplementedException();
        }

        protected override void MarkValidationResults(IDictionary<int, IList<DataCellValidationResult>> validationResults)
        {
            throw new NotImplementedException();
        }

        protected override void SaveValidationResult()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, DataRow> GetDataRows()
        {
            throw new NotImplementedException();
        }
    }
}
