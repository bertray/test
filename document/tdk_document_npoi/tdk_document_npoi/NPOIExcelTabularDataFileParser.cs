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
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Toyota.Common.Document;
using NPOI.HSSF.Util;
using System.IO;

namespace Toyota.Common.Document.NPOI
{
    public class NPOIExcelTabularDataFileParser: ExcelTabularDataFileParser
    {
        public NPOIExcelTabularDataFileParser(string path) : base(path) 
        {
            Init();
        }
        public NPOIExcelTabularDataFileParser(Stream stream, string resultPath) : base(stream, resultPath) 
        {
            Init();
        }
        private void Init()
        {
            FixBlankCellMissingBorder = true;
        }

        public IFont ErrorCellFont { set; get; }        
        public short ErrorCellBackgroundColor { set; get; }
        public FillPatternType ErrorCellFillPattern { set; get; }
        public IFont WarningCellFont { set; get; }
        public short WarningCellBackgroundColor { set; get; }
        public FillPatternType WarningCellFillPattern { set; get; }
        public IFont CellFont { set; get; }
        public short CellBackgroundColor { set; get; }
        public FillPatternType CellFillPattern { set; get; }

        public bool FixBlankCellMissingBorder { set; get; }
        private int MessageColumnIndex { set; get; }

        public override IDictionary<int, DataColumn> GetHeaders()
        {
            IRow headerRow = CurrentSheet.GetRow(HeaderRow);
            if (headerRow != null)
            {
                headerRow = CurrentSheet.GetRow(HeaderRow);
                IDictionary<int, DataColumn> columns = new Dictionary<int, DataColumn>();
                int lastCellIndex = headerRow.LastCellNum;
                MessageColumnIndex = lastCellIndex;
                ICell cell;
                DataColumn dataColumn;
                for (int i = 0; i < lastCellIndex; i++)
                {
                    cell = headerRow.GetCell(i);
                    if (cell == null)
                    {
                        break;
                    }
                    else
                    {
                        dataColumn = new DataColumn()
                        {
                            Index = i,
                            Name = cell.StringCellValue
                        };                        
                        columns.Add(dataColumn.Index, dataColumn);
                    }
                }

                return columns;
            }

            return null;
        }
        public override IDictionary<int, DataRow> GetDataRows()
        {
            if ((Workbook == null) || (!UseAllSheet && (SheetNames.Count == 0)))
            {
                return null;
            }

            IDictionary<int, DataRow> resultRows = new Dictionary<int, DataRow>();
            IDictionary<int, DataRow> tempRows;
            if (UseAllSheet)
            {
                int sheetCount = Workbook.NumberOfSheets;
                if (sheetCount > 0)
                {
                    for (int i = 0; i < sheetCount; i++)
                    {
                        CurrentSheet = Workbook.GetSheetAt(i);
                        tempRows = GetSheetRows();
                        if (tempRows != null)
                        {
                            foreach (int rowIndex in tempRows.Keys)
                            {
                                resultRows.Add(rowIndex, tempRows[rowIndex]);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (string sheetName in SheetNames)
                {
                    CurrentSheet = Workbook.GetSheet(sheetName);
                    tempRows = GetSheetRows();
                    if (tempRows != null)
                    {
                        foreach (int rowIndex in tempRows.Keys)
                        {
                            resultRows.Add(rowIndex, tempRows[rowIndex]);
                        }
                    }
                }
            }

            if (resultRows.Count > 0)
            {
                return resultRows;
            }
            return null;
        }
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

        private XSSFWorkbook workbook;
        private XSSFWorkbook Workbook
        {
            get
            {
                if ((workbook == null) & (FileStream != null))
                {
                    workbook = new XSSFWorkbook(FileStream);
                    SetDefaultStyles();             
                }
                return workbook;
            }
        }
        private ISheet CurrentSheet { set; get; }
        private void SetDefaultStyles()
        {
            if (Workbook != null)
            {
                CellFont = Workbook.CreateFont();
                CellFont.Color = HSSFColor.BLACK.index;
                CellBackgroundColor = HSSFColor.WHITE.index;
                CellFillPattern = FillPatternType.SOLID_FOREGROUND;
                ErrorCellFont = Workbook.CreateFont();
                ErrorCellFont.Color = HSSFColor.WHITE.index;
                ErrorCellBackgroundColor = HSSFColor.RED.index;
                ErrorCellFillPattern = FillPatternType.SOLID_FOREGROUND;
                WarningCellFont = Workbook.CreateFont();
                WarningCellFont.Color = HSSFColor.BLACK.index;
                WarningCellBackgroundColor = HSSFColor.YELLOW.index;
                WarningCellFillPattern = FillPatternType.SOLID_FOREGROUND;
            }
        }
        private DataRow CreateDataRow(IRow row, IDictionary<int, DataColumn> columns)
        {
            DataRow dataRow = new DataRow()
            {
                Index = row.RowNum,
                Cells = new Dictionary<int, DataCell>(),
                Columns = columns
            };

            foreach (DataColumn dataColumn in columns.Values)
            {                
                DataCell dataCell = new DataCell();
                dataCell.Column = dataColumn;

                ICell messageCell;
                ICell cell = row.GetCell(dataColumn.Index);
                if (cell != null)
                {                    
                    switch (cell.CellType)
                    {
                        case CellType.BLANK:
                            dataCell.Type = DataCellType.Blank;
                            break;
                        case CellType.BOOLEAN:
                            dataCell.Type = DataCellType.Boolean;
                            dataCell.Value = cell.BooleanCellValue;
                            break;
                        case CellType.ERROR:
                            dataCell.Type = DataCellType.Error;
                            dataCell.Value = cell.RichStringCellValue;
                            break;
                        case CellType.FORMULA:
                            dataCell.Type = DataCellType.Formula;
                            dataCell.Value = cell.CellFormula;
                            break;
                        case CellType.NUMERIC:
                            dataCell.Type = DataCellType.Numeric;
                            dataCell.Value = cell.NumericCellValue;
                            break;
                        case CellType.STRING:
                            dataCell.Type = DataCellType.String;
                            dataCell.Value = cell.StringCellValue;
                            break;
                        default:
                            dataCell.Type = DataCellType.Unknown;
                            break;
                    }                    
                }
                else
                {
                    cell = row.CreateCell(dataColumn.Index);
                    dataCell.Value = null;
                    dataCell.Type = DataCellType.Blank;
                }
                
                dataCell.Row = dataRow;
                dataRow.Cells.Add(dataColumn.Index, dataCell);

                /*
                 * Reset cells
                 */
                cell.CellStyle.FillForegroundColor = CellBackgroundColor;
                cell.CellStyle.SetFont(CellFont);
                if (FixBlankCellMissingBorder)
                {
                    SetCellBorder(cell);
                }
                messageCell = row.GetCell(MessageColumnIndex);
                if (messageCell != null)
                {
                    row.RemoveCell(messageCell);
                }
            }

            return dataRow;
        }
        protected override void MarkValidationResults(IDictionary<int, IList<DataCellValidationResult>> validationResults)
        {
            if ((validationResults != null) && (Workbook != null) && (CurrentSheet != null))
            {
                ICellStyle cellStyle = Workbook.CreateCellStyle();
                cellStyle.FillForegroundColor = CellBackgroundColor;
                cellStyle.FillPattern = CellFillPattern;
                ICellStyle errorCellStyle = Workbook.CreateCellStyle();
                errorCellStyle.FillForegroundColor = ErrorCellBackgroundColor;
                errorCellStyle.FillPattern = ErrorCellFillPattern;
                errorCellStyle.SetFont(ErrorCellFont);
                ICellStyle warningCellStyle = Workbook.CreateCellStyle();
                warningCellStyle.FillForegroundColor = WarningCellBackgroundColor;
                warningCellStyle.FillPattern = WarningCellFillPattern;
                warningCellStyle.SetFont(WarningCellFont);
                
                IRow row;
                ICell cell;
                ICell messageCell;
                IList<DataCellValidationResult> resultList;
                string messageCode;
                StringBuilder messageBuilder;
                foreach (int rowIndex in validationResults.Keys)
                {
                    resultList = validationResults[rowIndex];
                    row = CurrentSheet.GetRow(rowIndex);
                    if ((row != null) && (resultList.Count > 0))
                    {
                        messageCell = row.CreateCell(MessageColumnIndex);
                        messageBuilder = new StringBuilder();

                        foreach (DataCellValidationResult result in resultList)
                        {
                            messageCode = "INFO";
                            cell = row.GetCell(result.Cell.Column.Index);
                            if (cell != null)
                            {
                                switch (result.Status)
                                {
                                    case DataCellValidationStatus.Error:
                                        cell.CellStyle = errorCellStyle;
                                        if (FixBlankCellMissingBorder)
                                        {
                                            SetCellBorder(cell);
                                        }
                                        messageCode = "ERR";
                                        break;
                                    case DataCellValidationStatus.Passed_With_Warning:
                                        cell.CellStyle = warningCellStyle;
                                        if (FixBlankCellMissingBorder)
                                        {
                                            SetCellBorder(cell);
                                        }
                                        messageCode = "WRN";
                                        break;
                                    default:
                                        cell.CellStyle = cellStyle;
                                        break;
                                }
                            }

                            if (messageBuilder.Length == 0)
                            {
                                messageBuilder.Append(string.Format("{0}: \"{1}\" -> {2}", messageCode, result.Cell.Column.Name, result.Message.Trim()));
                            }
                            else
                            {
                                messageBuilder.Append(string.Format(" | {0}: \"{1}\" -> {2}", messageCode, result.Cell.Column.Name, result.Message.Trim()));
                            }                            
                        }
                        
                        messageCell.SetCellValue(messageBuilder.ToString());
                    }                    
                }                
            }
        }
        protected override void SaveValidationResult()
        {            
            if ((Workbook != null) && (CurrentSheet != null))
            {
                string resultFileName = FileName;
                if ((!UseAllSheet && !SaveValidationResultToOriginalFile) || string.IsNullOrEmpty(resultFileName))
                {
                    resultFileName = CurrentSheet.SheetName;
                }
                if (!string.IsNullOrEmpty(ValidationResultFileSuffix))
                {
                    resultFileName = CurrentSheet.SheetName + "_" + ValidationResultFileSuffix;
                }
                if(!resultFileName.EndsWith(".xlsx")) 
                {
                    resultFileName += ".xlsx";
                }
                Stream resultStream = new FileStream(ResultPath + "\\" + resultFileName, FileMode.Create);
                Workbook.Write(resultStream);
                resultStream.Close();
            }            
        }
        private IDictionary<int, DataRow> GetSheetRows()
        {
            if (CurrentSheet != null)
            {
                IDictionary<int, DataColumn> columns = FilterColumns(GetHeaders());

                int lastRowIndex = CurrentSheet.LastRowNum;
                int lastCellIndex = columns.Count;
                IRow row;
                DataRow dataRow;
                IDictionary<int, DataRow> resultRows = new Dictionary<int, DataRow>();

                for (int i = DataStartRow; i <= lastRowIndex; i++)
                {
                    row = CurrentSheet.GetRow(i);
                    if (row != null)
                    {
                        dataRow = CreateDataRow(row, columns);
                        resultRows.Add(dataRow.Index, dataRow);
                    }
                }

                if (resultRows.Count > 0)
                {
                    ValidateDataRows(resultRows);
                    if (HasError)
                    {
                        MarkValidationResults(ErrorValidationResult);
                    }
                    if (HasWarning)
                    {
                        MarkValidationResults(WarningValidationResult);
                    }
                    SaveValidationResult();
                    return resultRows;
                }
            }
            return null;
        }
        private void SetCellBorder(ICell cell)
        {
            cell.CellStyle.BorderBottom = BorderStyle.THIN;
            cell.CellStyle.BorderLeft = BorderStyle.THIN;
            cell.CellStyle.BorderRight = BorderStyle.THIN;
            cell.CellStyle.BorderTop = BorderStyle.THIN;
        }
    }
}
