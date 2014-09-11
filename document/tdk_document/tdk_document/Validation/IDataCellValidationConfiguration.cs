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
    public interface IDataCellValidationConfiguration
    {
        void SetName(string name);
        string GetName();

        DataCellValidation GetValidation(string columnName);
        IDictionary<string, DataCellValidation> GetValidations();

        void AddValidator(IDataCellValidator validator);
        IDataCellValidator GetValidator(string name);        
        void RemoveValidator(string name);
        void RemoveValidator(IDataCellValidator validator);
        IList<IDataCellValidator> GetValidators();

        IDataCellValidator GetColumnValidator(string columnName);
    }
}
