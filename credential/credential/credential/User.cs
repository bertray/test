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
using Toyota.Common.Generalist;

namespace Toyota.Common.Credential
{    
    public class User
    {
        public User()
        {
            PhoneNumbers = new List<PhoneNumber>();
            Emails = new List<string>();
            Roles = new List<Role>();
            Systems = new List<UserSystem>();
            CostCenters = new List<UserCostCenter>();
            Plants = new List<PlantOrganization>();
            Organizations = new List<OrganizationStructure>();
            Gender = GenderCategory.Unknown;
            Employment = EmploymentStatus.Contract;

            SessionTimeout = 20;
            LockTimeout = 10;
            MaximumConcurrentLogin = 3;
        }

        public string Id { set; get; }
        public string RegistrationNumber { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }        
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Name
        {
            get
            {
                string name = FirstName != null ? FirstName : String.Empty;
                name += LastName != null ? " " + LastName : String.Empty;
                return name.Trim();
            }
        }
        public DateTime PasswordExpirationDate { set; get; }        
        public virtual GenderCategory Gender { set; get; }
        public DateTime BirthDate { set; get; }
        
        [Obsolete("This property is obsolete, please don't use it")]
        public string Street { set; get; }
        [Obsolete("This property is obsolete, please don't use it")]
        public virtual UserState State { set; get; }
        [Obsolete("This property is obsolete, please don't use it")]
        public virtual UserCity City { set; get; }
        [Obsolete("This property is obsolete, please don't use it")]
        public virtual UserProvince Province { set; get; }
        [Obsolete("This property is obsolete, please don't use it")]
        public virtual UserCountry Country { set; get; }
        [Obsolete("This property is obsolete, please don't use it")]
        public string Zip { set; get; }
        
        public string Address { set; get; }
        public virtual IList<string> Emails { set; get; }        
        public virtual IList<PhoneNumber> PhoneNumbers { set; get; }

        public UserCompany Company { set; get; }
        public UserLocation Location { set; get; }
        public UserClass Class { set; get; }
        public IList<UserCostCenter> CostCenters { set; get; }
        public IList<PlantOrganization> Plants { set; get; }
        public IList<OrganizationStructure> Organizations { set; get; }
        public EmploymentStatus Employment { set; get; }

        private int sessionTimeout;
        public int? SessionTimeout
        {
            set
            {
                if (value.HasValue)
                {
                    sessionTimeout = value.Value;
                }
            }
            get
            {
                int timeout = sessionTimeout;
                if (!Roles.IsNullOrEmpty())
                {                    
                    foreach (Role role in Roles)
                    {
                        if (timeout < role.SessionTimeout)
                        {
                            timeout = role.SessionTimeout.Value;
                        }
                    }
                }
                return timeout;
            }
        }
        public int? LockTimeout { set; get; }
        public byte MaximumConcurrentLogin { set; get; }
        public IList<Role> Roles { set; get; }

        public IList<UserSystem> Systems { set; get; }
        public UserSystem DefaultSystem
        {
            get
            {
                if (!Systems.IsNullOrEmpty())
                {
                    foreach (UserSystem sys in Systems)
                    {
                        if (sys.IsDefault)
                        {
                            return sys;
                        }
                    }
                }
                return null;
            }
        }
        public DateTime AccountValidityDate { set; get; }
        public bool IsActive { set; get; }
    }
}
