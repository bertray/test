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
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Service
{
    public class ServiceCommandPool
    {
        private IDictionary<string, IServiceCommand> CommandMap { set; get; }

        public ServiceCommandPool()
        {
            CommandMap = new Dictionary<string, IServiceCommand>();
        }

        public void AddCommand(IServiceCommand command)
        {
            if (command != null)
            {
                string name = command.Name;
                if (CommandMap.ContainsKey(name))
                {
                    CommandMap[name] = command;
                }
                else
                {
                    CommandMap.Add(name, command);
                }
            }
        }
        public void RemoveCommand(string name)
        {
            if (CommandMap.ContainsKey(name))
            {
                CommandMap.Remove(name);
            }
        }

        public IServiceCommand GetCommand(string name)
        {
            if (CommandMap.ContainsKey(name))
            {
                return CommandMap[name];
            }
            return null;
        }
        public ICollection<IServiceCommand> Commands
        {
            get
            {
                return CommandMap.Values;
            }
        }

        public void Clear()
        {
            if (!CommandMap.IsNull())
            {
                CommandMap.Clear();
            }
        }
    }
}
