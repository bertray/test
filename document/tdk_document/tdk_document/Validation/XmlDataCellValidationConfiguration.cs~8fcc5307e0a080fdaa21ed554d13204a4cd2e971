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
    public class XmlDataCellValidationConfiguration: IDataCellValidationConfiguration
    {
        private const string NODE_ROOT = "validation";
        private const string NODE_COLUMN = "column";
        private const string ATTRIBUTE_COLUMN_NAME = "name";
        private const string ATTRIBUTE_COLUMN_VALIDATOR = "validator";
        private const string ATTRIBUTE_COLUMN_MANDATORY = "mandatory";
        private const string ATTRIBUTE_COLUMN_IGNORED = "ignore";

        private IDictionary<string, IDataCellValidator> validators;
        private IDictionary<string, DataCellValidation> validations;
        private string name;

        public XmlDataCellValidationConfiguration(string name, string path) : this(name, File.OpenRead(path)) { }
        public XmlDataCellValidationConfiguration(string name, Stream stream)
        {
            validators = new Dictionary<string, IDataCellValidator>();
            validations = new Dictionary<string, DataCellValidation>();
            Load(stream);
        }
        
        public string GetName()
        {
            return name;
        }
        public void SetName(string name)
        {
            this.name = name;
        }

        private void Load(Stream stream)
        {            
            XmlDocument document = new XmlDocument();
            document.Load(stream);

            XmlNodeList rootNodes = document.GetElementsByTagName(NODE_ROOT);
            if ((rootNodes != null) && (rootNodes.Count > 0))
            {
                XmlNode rootNode = rootNodes.Item(0);
                validations = new Dictionary<string, DataCellValidation>();
                DataCellValidation validation;
                XmlAttribute attribute;
                if (rootNode.HasChildNodes)
                {
                    foreach (XmlNode columnNode in rootNode.ChildNodes)
                    {
                        if (columnNode.Name.Equals(NODE_COLUMN))
                        {
                            validation = new DataCellValidation();
                            attribute = columnNode.Attributes[ATTRIBUTE_COLUMN_NAME];
                            if (attribute != null)
                            {
                                validation.Column = attribute.Value.Trim();
                            }
                            attribute = columnNode.Attributes[ATTRIBUTE_COLUMN_VALIDATOR];
                            if (attribute != null)
                            {
                                validation.Validator = attribute.Value.Trim();
                            }
                            attribute = columnNode.Attributes[ATTRIBUTE_COLUMN_MANDATORY];
                            if (attribute != null)
                            {
                                validation.Mandatory = Convert.ToBoolean(attribute.Value.Trim());
                            }
                            attribute = columnNode.Attributes[ATTRIBUTE_COLUMN_IGNORED];
                            if (attribute != null)
                            {
                                validation.Ignored = Convert.ToBoolean(attribute.Value.Trim());
                            }

                            if (!string.IsNullOrEmpty(validation.Column))
                            {
                                validations.Add(validation.Column, validation);
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
                    validators.Add(name, validator);
                }                
            }
        }
        public void RemoveValidator(string name)
        {
            if (validators.ContainsKey(name))
            {
                validators.Remove(name);
            }
        }
        public void RemoveValidator(IDataCellValidator validator)
        {
            if (validator != null)
            {
                RemoveValidator(validator.GetName());
            }
        }
        public IDataCellValidator GetValidator(string name)
        {
            if (validators.ContainsKey(name))
            {
                return validators[name];
            }
            return null;
        }

        public DataCellValidationResult Validate(DataCell cell)
        {
            if (cell != null)
            {
                string columnName = cell.Column.Name;
                if (validations.ContainsKey(columnName))
                {
                    DataCellValidation validation = validations[columnName];
                    IDataCellValidator validator = validators[validation.Validator];
                    return validator.Validate(cell);
                }
            }
            return null;
        }
        public IDictionary<string, DataCellValidation> GetValidations()
        {
            return validations;
        }
        public IList<IDataCellValidator> GetValidators()
        {
            return validators.Values.ToList().AsReadOnly();
        }

        public IDataCellValidator GetColumnValidator(string columnName)
        {
            if ((validations != null) && (validations.ContainsKey(columnName)))
            {
                DataCellValidation validation = validations[columnName];
                return GetValidator(validation.Validator);
            }
            return null;
        }

        public DataCellValidation GetValidation(string columnName)
        {
            if ((validations != null) && (validations.ContainsKey(columnName)))
            {
                return validations[columnName];
            }
            return null;
        }
    }
}
