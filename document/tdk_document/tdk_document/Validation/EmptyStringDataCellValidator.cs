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
    public class EmptyStringDataCellValidator: IDataCellValidator
    {
        public const string NAME = "EmptyString";
        public string GetName()
        {
            return NAME;
        }

        public DataCellValidationResult Validate(DataCell cell)
        {
            DataCellValidationResult result = null;
            if (cell != null)
            {
                result = new DataCellValidationResult();
                result.Cell = cell;
                if ((cell.Type == DataCellType.Blank) && string.IsNullOrEmpty((string)cell.Value))
                {
                    result.Status = DataCellValidationStatus.Error;
                    result.Message = "Value cannot be empty";
                }
                else
                {
                    result.Status = DataCellValidationStatus.Passed;
                    result.Message = "PASSED";
                }
            }
            return result;
        }
    }
}
