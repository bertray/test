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

namespace Toyota.Common.Document.Text
{
    public class SimpleCSVDocument: CSVDocument
    {
        public SimpleCSVDocument(string filePath) : base(filePath) { }

        public override IDictionary<int, CSVHeader> GetHeader()
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                FileStream stream = File.OpenRead(FilePath);
                StreamReader reader = new StreamReader(stream);
                if (!reader.EndOfStream)
                {
                    string headerLine = reader.ReadLine();
                    if (!string.IsNullOrEmpty(headerLine))
                    {
                        string[] headerFractions = headerLine.Split(Separator);
                        if (headerFractions != null)
                        {
                            string headerText;
                            for (int i = 0; i < headerFractions.Length; i++)
                            {
                                headerText = headerFractions[i].Trim();

                            }
                        }
                    }
                }
                reader.Close();
                stream.Close();
            }            
            return null;
        }

        public override IDictionary<int, CSVRow> GetRows()
        {
            throw new NotImplementedException();
        }
    }
}
