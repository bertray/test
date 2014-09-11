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
using Toyota.Common.Document;

namespace Toyota.Common.Document
{
    public abstract class TabularDataFileParser
    {
        public TabularDataFileParser(string path) : this(File.OpenRead(path), path.Substring(0, path.LastIndexOf('\\'))) 
        {
            FileName = path.Substring(path.LastIndexOf('\\') + 1);
        }
        public TabularDataFileParser(Stream stream, string resultPath)
        {
            IgnoreSymbol = "~";
            MandatorySymbol = "*";
            HeaderRow = 0;
            DataStartRow = 1;
            ValidationResultFileSuffix = "ValidationResult";
            FileStream = stream;
            ResultPath = resultPath;
        }

        public string ValidationResultFileSuffix { set; get; }
        public string IgnoreSymbol { set; get; }
        public string MandatorySymbol { set; get; }
        public int HeaderRow { set; get; }
        public int DataStartRow { set; get; }
        public string FilePath { private set; get; }
        public string ResultPath { set; get; }
        protected string FileName { private set; get; }
        protected Stream FileStream { private set; get; }
        public void Dispose()
        {
            if (FileStream != null)
            {
                FileStream.Close();
            }
        }

        public IDataCellValidationConfiguration ValidationConfiguration { set; get; }
        public IDictionary<int, IList<DataCellValidationResult>> ErrorValidationResult { protected set; get; }
        public IDictionary<int, IList<DataCellValidationResult>> WarningValidationResult { protected set; get; }
        public bool HasError 
        {
            get
            {
                return (ErrorValidationResult != null) && (ErrorValidationResult.Count > 0);
            }
        }
        public bool HasWarning 
        {
            get
            {
                return (WarningValidationResult != null) && (WarningValidationResult.Count > 0);
            }
        }

        public abstract IDictionary<int, DataColumn> GetHeaders();
        public abstract IDictionary<int, DataRow> GetDataRows();
        public abstract DataRow GetDataRow(int rowIndex);
        public abstract IList<DataCell> GetDataCells(string columnName);
        public abstract DataCell GetDataCell(string columnName);
        protected abstract void MarkValidationResults(IDictionary<int, IList<DataCellValidationResult>> validationResults);
        protected abstract void SaveValidationResult();

        protected IDictionary<int, DataColumn> FilterColumns(IDictionary<int, DataColumn> columns)
        {
            if ((columns != null) && (columns.Count > 0))
            {
                string columnName;
                foreach (DataColumn column in columns.Values)
                {
                    columnName = column.Name;
                    if(!string.IsNullOrEmpty(columnName) && columnName.EndsWith(MandatorySymbol)) 
                    {
                        columnName = columnName.Substring(0, columnName.Length - MandatorySymbol.Length);
                    }
                    column.Name = columnName.Trim();
                }

                IDictionary<string, DataCellValidation> validations = null;
                if (ValidationConfiguration != null)
                {
                    validations = ValidationConfiguration.GetValidations();
                    IDictionary<int, DataColumn> tempColumns = new Dictionary<int, DataColumn>();
                    bool matched;
                    DataCellValidation columnValidation = null;
                    foreach (DataColumn column in columns.Values)
                    {
                        matched = false;
                        foreach (DataCellValidation validation in validations.Values)
                        {
                            if (validation.Column.Equals(column.Name))
                            {
                                matched = true;
                                columnValidation = validation;
                                break;
                            }
                        }
                        if (matched && !columnValidation.Ignored)
                        {
                            tempColumns.Add(column.Index, column);
                        }
                    }
                    if (tempColumns.Count > 0)
                    {
                        return tempColumns;
                    }
                }
            }
            return columns;
        }
        protected void ValidateDataRows(IDictionary<int, DataRow> dataRows)
        {
            if ((dataRows != null) && (ValidationConfiguration != null))
            {
                ErrorValidationResult = new Dictionary<int, IList<DataCellValidationResult>>();
                WarningValidationResult = new Dictionary<int, IList<DataCellValidationResult>>();

                IDataCellValidator cellValidator;
                DataCellValidationResult validationResult;
                bool mandatoryCheckFailed;
                IDataCellValidator emptyCellValidator = new EmptyStringDataCellValidator();

                foreach (DataRow dataRow in dataRows.Values)
                {
                    foreach (DataCell dataCell in dataRow.Cells.Values)
                    {
                        validationResult = null;
                        mandatoryCheckFailed = false;
                        if ((dataCell.Type == DataCellType.String) ||  (dataCell.Type == DataCellType.Blank))
                        {
                            validationResult = emptyCellValidator.Validate(dataCell);
                            mandatoryCheckFailed = (validationResult.Status == DataCellValidationStatus.Error);
                        }

                        if (!mandatoryCheckFailed)
                        {
                            cellValidator = ValidationConfiguration.GetColumnValidator(dataCell.Column.Name);
                            if (cellValidator == null)
                            {
                                continue;
                            }

                            validationResult = cellValidator.Validate(dataCell);
                        }                        
                        
                        if (validationResult != null)
                        {
                            validationResult.Cell = dataCell;
                            switch (validationResult.Status)
                            {
                                case DataCellValidationStatus.Error:
                                    if (!ErrorValidationResult.ContainsKey(dataRow.Index))
                                    {
                                        ErrorValidationResult.Add(dataRow.Index, new List<DataCellValidationResult>());
                                    }
                                    ErrorValidationResult[dataRow.Index].Add(validationResult);
                                    break;
                                case DataCellValidationStatus.Passed_With_Warning:
                                    if (!WarningValidationResult.ContainsKey(dataRow.Index))
                                    {
                                        WarningValidationResult.Add(dataRow.Index, new List<DataCellValidationResult>());
                                    }
                                    WarningValidationResult[dataRow.Index].Add(validationResult);
                                    break;
                                default: break;
                            }
                        }
                    }
                }

                if (ErrorValidationResult.Count == 0)
                {
                    ErrorValidationResult = null;
                }
                if (WarningValidationResult.Count == 0)
                {
                    WarningValidationResult = null;
                }
            }
        }
    }
}
