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
using System.Xml;

namespace Toyota.Common.Document
{
    public class XmlDataCellValidator: IDataCellValidator
    {
        private const string NODE_ROOT = "validation";
        private const string NODE_COLUMN = "column";
        private const string ATTRIBUTE_COLUMN_NAME = "name";
        private const string ATTRIBUTE_COLUMN_VALIDATOR = "validator";

        private IDictionary<string, IDataCellValidator> validators;

        public XmlDataCellValidator(string path) : this(File.OpenRead(path)) { }
        public XmlDataCellValidator(Stream stream)
        {
            validators = new Dictionary<string, IDataCellValidator>();
            Load(stream);
        }

        public IDictionary<string, XmlDataCellValidation> Validations { set; get; }
        public string Path { set; get; }
        private void Load(Stream stream)
        {            
            XmlDocument document = new XmlDocument();
            document.Load(stream);

            XmlNodeList rootNodes = document.GetElementsByTagName(NODE_ROOT);
            if ((rootNodes != null) && (rootNodes.Count > 0))
            {
                XmlNode rootNode = rootNodes.Item(0);
                Validations = new Dictionary<string, XmlDataCellValidation>();
                XmlDataCellValidation validation;
                XmlAttribute attribute;
                if (rootNode.HasChildNodes)
                {
                    foreach (XmlNode columnNode in rootNode.ChildNodes)
                    {
                        if (columnNode.Name.Equals(NODE_COLUMN))
                        {
                            validation = new XmlDataCellValidation();
                            attribute = columnNode.Attributes[ATTRIBUTE_COLUMN_NAME];
                            if (attribute != null)
                            {
                                validation.Column = attribute.Value;
                            }
                            attribute = columnNode.Attributes[ATTRIBUTE_COLUMN_VALIDATOR];
                            if (attribute != null)
                            {
                                validation.Validator = attribute.Value;
                            }

                            if (!string.IsNullOrEmpty(validation.Column))
                            {
                                Validations.Add(validation.Column, validation);
                            }
                        }                        
                    }
                }
            }

            stream.Close();
        }

        public void AddValidator(IDataCellValidator validator)
        {
            if (validator != null)
            {
                string name = validator.GetName();
                if (validators.ContainsKey(name))
                {
                    validators[name] = validator;
                }
                else
                {
                }
                validators.Add(name, validator);
            }
        }
        public void RemoveValidator(string name)
        {
            if (validators.ContainsKey(name))
            {
                validators.Remove(name);
            }
        }

        public IList<IDataCellValidator> Validators
        {
            get
            {
                return validators.Values.ToList().AsReadOnly();
            }
        }

        public DataCellValidationResult Validate(DataCell cell)
        {
            if (cell != null)
            {
                string columnName = cell.Column.Name;
                if (Validations.ContainsKey(columnName))
                {
                    XmlDataCellValidation validation = Validations[columnName];
                    IDataCellValidator validator = validators[validation.Validator];
                    return validator.Validate(cell);
                }
            }
            return null;
        }

        public string GetName()
        {
            return "XmlDataCellValidator";
        }
    }
}
