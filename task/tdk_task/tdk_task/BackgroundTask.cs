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
using Toyota.Common.Credential;

namespace Toyota.Common.Task
{
    public class BackgroundTask
    {
        public const string PARAMETER_ID = "tID";
        public const string PARAMETER_NAME = "tNAME";

        public BackgroundTask()
        {
            Parameters = new BackgroundTaskParameter();
        }

        private string id;
        public string Id 
        {
            set
            {
                id = value;
                Parameters.Add(PARAMETER_ID, id);
            }
            get
            {
                return id;
            }
        }

        private string name;
        public string Name 
        {
            set
            {
                name = value;
                Parameters.Add(PARAMETER_NAME, name);
            }
            get
            {
                return name;
            }
        }
        public string Description { set; get; }        
        public User Submitter { set; get; }
        public string FunctionName { set; get; }
        public BackgroundTaskParameter Parameters { set; get; }
        public TaskType Type { set; get; }
        public TaskStatus Status { set; get; }
        public byte? Progress { set; get; }
        public BackgroundTaskExecutionDescriptor ExecutionDescriptor { set; get; }
    }
}
