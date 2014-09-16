///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
///
/// <modified>
///     <author>yudha - yudha_hyp@yahoo.com (28-nov-2013)</date> 
///     <summary>
///         ~ add code Fetch Organization, IterateOrganization
///         ~ add code IsUserAuthentic - Validate Credential AD
///     </summary>   
///     <author>yudha - yudha_hyp@yahoo.com (6-dec-2013)</date> 
///     <summary>
///         ~ add code Fetch Plant
///     </summary>    
/// </modified>
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Toyota.Common.Database;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using Toyota.Common.Configuration;
using Toyota.Common.Configuration.Binder;
using Toyota.Common.Utilities;
using System.Dynamic;
using System.Data.SqlTypes;
using Toyota.Common.Generalist;

namespace Toyota.Common.Credential.TMMIN
{
    public class TmminUserProvider: IUserProvider
    {
        public IDBContextManager dbManager;
        private ISqlLoader sqlLoader;
        private string contextName;

        private string ReleasePhase { set; get; }
        private IConfigurationBinder SystemConfiguration { set; get; }
        private IConfigurationBinder ActiveDirectoryConfiguration { set; get; }

        public TmminUserProvider(IDBContextManager dbManager) : this(dbManager, null) { }
        public TmminUserProvider(IDBContextManager dbManager, string contextName)
        {
            this.dbManager = dbManager;
            this.contextName = contextName;
            this.sqlLoader = new AssemblyFileSqlLoader(GetType().Assembly, "Toyota.Common.Credential.TMMIN.SQL");
            this.dbManager.AddSqlLoader(sqlLoader);

            SystemConfiguration = new AssemblyXmlFileConfigurationBinder("System", "Toyota.Common.Credential.TMMIN.Configuration", GetType().Assembly);
            SystemConfiguration.Load();
            ActiveDirectoryConfiguration = new AssemblyXmlFileConfigurationBinder("ActiveDirectory", "Toyota.Common.Credential.TMMIN.Configuration", GetType().Assembly);
            ActiveDirectoryConfiguration.Load();

            ConfigurationItem config = SystemConfiguration.GetConfiguration("Phase");
            if (config != null)
            {
                ReleasePhase = config.Value;
            }
            else
            {
                ReleasePhase = "DEV";
            }
        }

        public void Delete(string username)
        {           
            IDBContext db = _CreateDBContext();
            db.Execute("User_Delete", new { Username = username });
            db.Close();
        }
        public void Save(User user)
        {
            if ((user != null) && (user is TmminUser))
            {
                TmminUser tUser = (TmminUser)user;
                if (tUser.RegistrationNumber.IsNullOrEmpty())
                {
                    tUser.RegistrationNumber = tUser.Id;
                }
                DateTime today = DateTime.Now;
                dynamic param = new ExpandoObject();
                param.Username = tUser.Username;
                param.RegistrationNumber = tUser.RegistrationNumber;
                param.PersonalId = tUser.Id;
                param.CompanyCode = !tUser.Company.IsNull() ? tUser.Company.Id : null;
                param.Password = tUser.Password;
                param.PasswordExpirationDate = SqlDateTime.MinValue;
                if (!tUser.PasswordExpirationDate.IsNull() && (tUser.PasswordExpirationDate > DateTime.MinValue))
                {
                    param.PasswordExpirationDate = tUser.PasswordExpirationDate;
                }
                param.AccountValidityDate = SqlDateTime.MinValue;
                if (!tUser.AccountValidityDate.IsNull() && (tUser.AccountValidityDate > DateTime.MinValue))
                {
                    param.AccountValidityDate = tUser.AccountValidityDate;
                }
                param.FirstName = tUser.FirstName;
                param.LastName = tUser.LastName;
                param.FullName = tUser.Name;

                param.PhoneNumber = DBNull.Value;
                param.ExtensionNumber = DBNull.Value;
                param.MobileNumber = DBNull.Value;
                if (!tUser.PhoneNumbers.IsNullOrEmpty())
                {
                    PhoneNumber phoneNum = tUser.PhoneNumbers.FindElement<PhoneNumber>(num =>
                    {
                        if (num.Category == PhoneNumberCategory.Work)
                        {
                            return num;
                        }
                        return null;
                    });
                    if (!phoneNum.IsNull())
                    {
                        param.PhoneNumber = phoneNum.Number;
                    }

                    phoneNum = tUser.PhoneNumbers.FindElement<PhoneNumber>(num =>
                    {
                        if (num.Category == PhoneNumberCategory.Extension)
                        {
                            return num;
                        }
                        return null;
                    });
                    if (!phoneNum.IsNull())
                    {
                        param.ExtensionNumber = phoneNum.Number;
                    }

                    phoneNum = tUser.PhoneNumbers.FindElement<PhoneNumber>(num =>
                    {
                        if (num.Category == PhoneNumberCategory.Mobile)
                        {
                            return num;
                        }
                        return null;
                    });
                    if (!phoneNum.IsNull())
                    {
                        param.MobileNumber = phoneNum.Number;
                    }
                }

                param.Email = DBNull.Value;
                if (!tUser.Emails.IsNullOrEmpty())
                {
                    param.Email = tUser.Emails.First();
                }

                param.ClassId = tUser.Class.IsNull() ? null : tUser.Class.Id;
                param.JobFunction = tUser.JobFunction;
                param.CCCode = tUser.CostCenters.IsNullOrEmpty() ? null : tUser.CostCenters.First().Id;
                param.LocationId = tUser.Location.IsNull() ? null : tUser.Location.Id;
                param.MaxLogon = tUser.MaximumConcurrentLogin;
                param.DefaultSystem = tUser.DefaultSystem.IsNull() ? null : tUser.DefaultSystem.Id;
                param.ProductionFlag = tUser.InProduction;
                param.ActiveDirectoryFlag = tUser.InActiveDirectory;
                param.Shift = tUser.Shift;
                param.ActiveFlag = tUser.IsActive;
                param.EmploymentStatus = !tUser.Employment.IsNull() ? tUser.Employment.Value : null;
                
                IDBContext db = _CreateDBContext();
                db.BeginTransaction();

                User existingUser = GetUser(param.Username);                
                if (existingUser.IsNull())
                {                    
                    db.Execute("User_Insert", new { 
                        Username = param.Username
                       ,RegistrationNumber = param.RegistrationNumber
                       ,PersonalId = param.PersonalId
                       ,CompanyCode = param.CompanyCode
                       ,Password = param.Password
                       ,PasswordExpirationDate = param.PasswordExpirationDate
                       ,AccountValidityDate = param.AccountValidityDate
                       ,FirstName = param.FirstName
                       ,LastName = param.LastName
                       ,FullName = param.FullName
                       ,PhoneNumber = param.PhoneNumber
                       ,ExtensionNumber = param.ExtensionNumber
                       ,MobileNumber = param.MobileNumber
                       ,Email = param.Email
                       ,ClassId = param.ClassId
                       ,JobFunction = param.JobFunction
                       ,CCCode = param.CCCode
                       ,LocationId = param.LocationId
                       ,MaxLogon = param.MaxLogon
                       ,DefaultSystem = param.DefaultSystem
                       ,ProductionFlag = param.ProductionFlag
                       ,ActiveDirectoryFlag = param.ActiveDirectoryFlag
                       ,Shift = param.Shift
                       ,ActiveFlag = param.ActiveFlag
                       ,EmploymentStatus = param.EmploymentStatus
                    });                    
                } else {
                     db.Execute("User_Update", new { 
                        Username = param.Username
                       ,RegistrationNumber = param.RegistrationNumber
                       ,PersonalId = param.PersonalId
                       ,CompanyCode = param.CompanyCode
                       ,Password = param.Password
                       ,PasswordExpirationDate = param.PasswordExpirationDate
                       ,AccountValidityDate = param.AccountValidityDate
                       ,FirstName = param.FirstName
                       ,LastName = param.LastName
                       ,FullName = param.FullName
                       ,PhoneNumber = param.PhoneNumber
                       ,ExtensionNumber = param.ExtensionNumber
                       ,MobileNumber = param.MobileNumber
                       ,Email = param.Email
                       ,ClassId = param.ClassId
                       ,JobFunction = param.JobFunction
                       ,CCCode = param.CCCode
                       ,LocationId = param.LocationId
                       ,MaxLogon = param.MaxLogon
                       ,DefaultSystem = param.DefaultSystem
                       ,ProductionFlag = param.ProductionFlag
                       ,ActiveDirectoryFlag = param.ActiveDirectoryFlag
                       ,Shift = param.Shift
                       ,ActiveFlag = param.ActiveFlag
                       ,EmploymentStatus = param.EmploymentStatus
                    });              
                }

                if (!tUser.Roles.IsNullOrEmpty())
                {
                    IList<Role> _xroles;
                    IList<UserSystem> _xsystems;
                    IList<TmminArea> _xarea;
                    IList<AuthorizationFunction> _xfunctions;
                    IList<AuthorizationFeature> _xfeatures;
                    IList<RoleFeatureModel> _xroleQualifiers;
                    IList<RoleFunctionModel> _xroleFunctions;
                    IList<RoleFeatureModel> _xroleFeatures;

                    foreach (TmminRole role in tUser.Roles)
                    {
                        _xsystems = db.Fetch<UserSystem>("System_SelectById", new { SystemId = role.System.Id });
                        if (_xsystems.IsNullOrEmpty())
                        {
                            db.Execute("System_Insert", new {
                                Id = role.System.Id,
                                Name = role.System.Name,
                                Description = role.System.Description,
                                Url = role.System.Url,
                                CreatedBy = "system",
                                CreatedDate = today
                            });
                        }

                        _xarea = db.Fetch<TmminArea>("Area_SelectById", new { Id = role.Area.Id });
                        if (_xarea.IsNullOrEmpty())
                        {
                            db.Execute("Area_Insert", new {
                                Id = role.Area.Id,
                                Name = role.Area.Name,
                                CreatedBy = "system",
                                CreatedDate = today
                            });
                        }

                        _xroles = db.Fetch<Role>("Role_SelectById", new { Id = role.Id });
                        if (_xroles.IsNullOrEmpty())
                        {
                            db.Execute("Role_Insert", new {
                                SystemId = role.System.Id,
                                Id = role.Id,    
                                Name = role.Name,
                                Description = role.Description,
                                AreaId = role.Area.Id,
                                SessionTimeout = role.SessionTimeout,
                                CreatedBy = "system",
                                CreatedDate = today
                            });
                        }

                        if (!role.Functions.IsNullOrEmpty())
                        {
                            foreach (AuthorizationFunction func in role.Functions)
                            {
                                _xfunctions = db.Fetch<AuthorizationFunction>("Function_SelectByIdAndSystem", new { FunctionId = func.Id, SystemId = role.System.Id });
                                if (_xfunctions.IsNullOrEmpty())
                                {
                                    db.Execute("Function_Insert", new {
                                        Id = func.Id,
                                        SystemId = role.System.Id,
                                        ModuleId = string.Empty,
                                        Name = func.Name,
                                        Description = func.Description,
                                        CreatedBy = "system",
                                        CreatedDate = today
                                    });
                                }

                                _xroleFunctions = db.Fetch<RoleFunctionModel>("Role_Function_SelectByRoleId", new { RoleId = role.Id });
                                if (_xroleFunctions.IsNullOrEmpty())
                                {
                                    db.Execute("Role_Function_Insert", new { 
                                        RoleId = role.Id,
                                        FunctionId = func.Id,
                                        CreatedBy = "system",
                                        CreatedDate = today
                                    });
                                }

                                if (!func.Features.IsNullOrEmpty())
                                {
                                    foreach(AuthorizationFeature feat in func.Features) 
                                    {
                                        _xfeatures = db.Fetch<AuthorizationFeature>("Feature_SelectById", new { Id = feat.Id });
                                        if (_xfeatures.IsNullOrEmpty())
                                        {
                                            db.Execute("Feature_Insert", new { 
                                                Id = feat.Id,
                                                Name = feat.Name,
                                                CreatedBy = "system",
                                                CreatedDate = today
                                            });
                                        }

                                        _xroleFeatures = db.Fetch<RoleFeatureModel>("Role_Feature_SelectById", new {
                                            RoleId = role.Id,
                                            FunctionId = func.Id,
                                            FeatureId = feat.Id
                                        });
                                        if (_xroleFeatures.IsNullOrEmpty())
                                        {
                                            db.Execute("Role_Feature_Insert", new
                                            {
                                                RoleId = role.Id,
                                                FunctionId = func.Id,
                                                FeatureId = feat.Id,
                                                CreatedBy = "system",
                                                CreatedDate = today
                                            });
                                        }

                                        if (!feat.Qualifiers.IsNullOrEmpty())
                                        {
                                            foreach (AuthorizationFeatureQualifier qf in feat.Qualifiers)
                                            {
                                                _xroleQualifiers = db.Fetch<RoleFeatureModel>("Qualifier_SelectByKey", new { 
                                                    RoleId = role.Id,
                                                    FunctionId = func.Id,
                                                    FeatureId = feat.Id,
                                                    Key = qf.Key
                                                });

                                                if (_xroleQualifiers.IsNullOrEmpty())
                                                {
                                                    db.Execute("Qualifier_Insert", new {
                                                        RoleId = role.Id,
                                                        FunctionId = func.Id,
                                                        FeatureId = feat.Id,
                                                        Key = qf.Key,
                                                        Qualifier = qf.Qualifier,
                                                        CreatedBy = "system",
                                                        CreatedDate = today
                                                    });
                                                }
                                            }
                                        }
                                    }                                    
                                }
                            }
                        }
                    }
                }

                db.CommitTransaction();
                db.Close();
            }            
        } 
        public IList<User> GetUsers()
        {
            IDBContext db = _CreateDBContext();
            IList<TmminUserModel> models = db.Fetch<TmminUserModel>("User_Select");
            List<TmminUser> users = _NormalizedUserModel(models, db);
            
            db.Close();

            return users.Cast<User>().ToList();
        }
        public IList<User> GetUsers(long pageNumber, long pageSize)
        {
            IDBContext db = _CreateDBContext();
            IPagedData<TmminUserModel> pagedModels = db.FetchByPage<TmminUserModel>("User_Select", pageNumber, pageSize);
            List<TmminUser> users = _NormalizedUserModel(pagedModels.GetData(), db);
            db.Close();

            return users.Cast<User>().ToList();
        }
        public long GetUserCount()
        {
            IDBContext db = _CreateDBContext();
            long total = db.ExecuteScalar<long>("User_Count");
            db.Close();

            return total;
        }
        public User GetUser(string username)
        {
            IDBContext db = _CreateDBContext();
            IList<TmminUserModel> mUsers = db.Fetch<TmminUserModel>("User_Select_ByUsername", new { Username = username });
            List<TmminUser> users = _NormalizedUserModel(mUsers, db);
            db.Close();

            if (!mUsers.IsNullOrSizeLessThan(1))
            {
                return mUsers[0];
            }
            return null;
        }

        public IList<User> Search(UserSearchCriteria criteria, object key)
        {
            IList<User> result = null;

            switch (criteria)
            {
                case UserSearchCriteria.AccountValidityDate: return _SearchUserByAccountValidity((DateTime) key);
                case UserSearchCriteria.Address: throw new NotImplementedException();
                case UserSearchCriteria.BirthDate: throw new NotImplementedException();
                case UserSearchCriteria.City: throw new NotImplementedException();
                case UserSearchCriteria.Class: throw new NotImplementedException();
                case UserSearchCriteria.Company: throw new NotImplementedException();
                case UserSearchCriteria.CostCenter: throw new NotImplementedException();
                case UserSearchCriteria.Country: throw new NotImplementedException();
                case UserSearchCriteria.Email: throw new NotImplementedException();
                case UserSearchCriteria.FirstName: return _SearchUserByFirstName((string)key);
                case UserSearchCriteria.LastName: return _SearchUserByLastName((string) key);
                case UserSearchCriteria.Gender: throw new NotImplementedException();
                case UserSearchCriteria.Id: throw new NotImplementedException();
                case UserSearchCriteria.Location: throw new NotImplementedException();
                case UserSearchCriteria.LockTimeout: throw new NotImplementedException();
                case UserSearchCriteria.MaximumConcurrentLogin: throw new NotImplementedException();             
                case UserSearchCriteria.Name: return _SearchUserByName((string) key);
                case UserSearchCriteria.Organization: throw new NotImplementedException();
                case UserSearchCriteria.PasswordExpirationDate: throw new NotImplementedException();
                case UserSearchCriteria.Phone: throw new NotImplementedException();
                case UserSearchCriteria.Plant: throw new NotImplementedException();
                case UserSearchCriteria.Province: throw new NotImplementedException();
                case UserSearchCriteria.RegistrationNumber: throw new NotImplementedException();
                case UserSearchCriteria.Role: throw new NotImplementedException();
                case UserSearchCriteria.SessionTimeout: throw new NotImplementedException();
                case UserSearchCriteria.State: throw new NotImplementedException();
                case UserSearchCriteria.Street: throw new NotImplementedException();
                case UserSearchCriteria.System: throw new NotImplementedException();
                case UserSearchCriteria.Zip: throw new NotImplementedException();
                default: break;
            }
            return result;
        }
        public IPagedData<User> Search(UserSearchCriteria criteria, long pageNumber, long pageSize, object key)
        {
            switch (criteria)
            {
                case UserSearchCriteria.AccountValidityDate: return _SearchUserByAccountValidity((DateTime)key, pageNumber, pageSize);
                case UserSearchCriteria.Address: throw new NotImplementedException();
                case UserSearchCriteria.BirthDate: throw new NotImplementedException();
                case UserSearchCriteria.City: throw new NotImplementedException();
                case UserSearchCriteria.Class: throw new NotImplementedException();
                case UserSearchCriteria.Company: throw new NotImplementedException();
                case UserSearchCriteria.CostCenter: throw new NotImplementedException();
                case UserSearchCriteria.Country: throw new NotImplementedException();
                case UserSearchCriteria.Email: break;
                case UserSearchCriteria.FirstName: return _SearchUserByFirstName((string)key, pageNumber, pageSize);
                case UserSearchCriteria.LastName: return _SearchUserByLastName((string) key, pageNumber, pageSize);
                case UserSearchCriteria.Gender: throw new NotImplementedException();
                case UserSearchCriteria.Id: throw new NotImplementedException();
                case UserSearchCriteria.Location: throw new NotImplementedException();
                case UserSearchCriteria.LockTimeout: throw new NotImplementedException();
                case UserSearchCriteria.MaximumConcurrentLogin: throw new NotImplementedException();
                case UserSearchCriteria.Name: return _SearchUserByName((string)key, pageNumber, pageSize);
                case UserSearchCriteria.Organization: throw new NotImplementedException();
                case UserSearchCriteria.PasswordExpirationDate: throw new NotImplementedException();
                case UserSearchCriteria.Phone: throw new NotImplementedException();
                case UserSearchCriteria.Plant: throw new NotImplementedException();
                case UserSearchCriteria.Province: throw new NotImplementedException();
                case UserSearchCriteria.RegistrationNumber: throw new NotImplementedException();
                case UserSearchCriteria.Role: throw new NotImplementedException();
                case UserSearchCriteria.SessionTimeout: throw new NotImplementedException();
                case UserSearchCriteria.State: throw new NotImplementedException();
                case UserSearchCriteria.Street: throw new NotImplementedException();
                case UserSearchCriteria.System: throw new NotImplementedException();
                case UserSearchCriteria.Zip: throw new NotImplementedException();
                default: break;
            }
            return null;
        }        

        public void Dispose()
        {
            dbManager.RemoveSqlLoader(sqlLoader);
        }        

        public void FetchAuthorization(User user)
        {
            IDBContext db = _CreateDBContext();
            IList<AuthorizationModel> roleList = db.Fetch<AuthorizationModel>("Authorization_SelectByUsername", new { Username = user.Username });
            if ((roleList != null) && (roleList.Count > 0))
            {
                IDictionary<string, IList<TmminRole>> roleMasterMap = new Dictionary<string, IList<TmminRole>>();

                IList<TmminRole> roles;
                IList<AuthorizationFunction> functions;
                IList<AuthorizationFeature> features;
                IList<AuthorizationFeatureQualifier> qualifiers;
                foreach (AuthorizationModel authModel in roleList)
                {
                    if (authModel.System.IsNull())
                    {
                        continue;
                    }

                    if (!roleMasterMap.ContainsKey(authModel.System.Id))
                    {
                        roles = db.Fetch<TmminRole>("Role_SelectBySystem", new { SystemId = authModel.System.Id });
                        roleMasterMap.Add(authModel.System.Id, roles);
                    }

                    roles = roleMasterMap[authModel.System.Id];
                    foreach (TmminRole role in roles)
                    {
                        if (role.Id.Equals(authModel.RoleId))
                        {
                            user.Roles.Add(new TmminRole() { 
                                Area = role.Area,
                                Description = role.Description,
                                Functions = role.Functions,
                                Id = role.Id,
                                Name = role.Name,
                                SessionTimeout = role.SessionTimeout,
                                System = role.System
                                //DivisionCode = authModel.DivisionCode
                            });
                        }
                    }

                    if ((user.Roles != null) && (user.Roles.Count > 0))
                    {
                        foreach (TmminRole role in user.Roles)
                        {
                            functions = db.Fetch<AuthorizationFunction>("Role_Function_SelectByRoleId", new { RoleId = authModel.RoleId });
                            if ((functions != null) && (functions.Count > 0))
                            {
                                role.Functions = functions;
                                foreach (AuthorizationFunction function in role.Functions)
                                {
                                    features = db.Fetch<AuthorizationFeature>("Feature_SelectByRoleId", new { RoleId = authModel.RoleId, FunctionId = function.Id });
                                    if ((features != null) && (features.Count > 0))
                                    {
                                        function.Features = features;
                                        foreach (AuthorizationFeature feature in function.Features)
                                        {
                                            qualifiers = db.Fetch<AuthorizationFeatureQualifier>("Qualifier_SelectByRoleId", new { RoleId = authModel.RoleId, FeatureId = feature.Id });
                                            if ((qualifiers != null) && (qualifiers.Count > 0))
                                            {
                                                feature.Qualifiers = qualifiers;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                db.Close();
            }
        }
        public void FetchAuthorization(IList<User> users)
        {
            foreach (User user in users)
            {
                FetchAuthorization(user);
            }
        }                  
        public void FetchOrganization(User user)
        {
            IDBContext db = _CreateDBContext();
            IList<OrganizationStructure> userPosition = db.Fetch<OrganizationStructure>("Position_SelectByUsername", new { Username = user.Username });
            {
                if ((userPosition != null) && (userPosition.Count > 0))
                {
                    Queue<string> sqlQueue;

                    IList<OrganizationStructure> finalOrg = new List<OrganizationStructure>();
                    IList<OrganizationStructure> bufferOrg;
                    OrganizationStructure outPosition;
                    foreach (OrganizationStructure position in userPosition)
                    {
                        sqlQueue = new Queue<string>();
                        sqlQueue.Enqueue("Group_SelectByGroupCode");
                        sqlQueue.Enqueue("Line_SelectByLineCode");
                        sqlQueue.Enqueue("Section_SelectByUnitCode");
                        sqlQueue.Enqueue("Department_SelectByDepartmentCode");
                        sqlQueue.Enqueue("Directorate_SelectByDivisionCode");
                        sqlQueue.Enqueue("Company_SelectByDirectorateCode");

                        bufferOrg = new List<OrganizationStructure>();
                        outPosition = position;

                        _IterateOrganization(db, outPosition, bufferOrg, sqlQueue, position.Id);
                        finalOrg.Add(outPosition);
                    }
                    user.Organizations.Merge(finalOrg);
                }
            }
            db.Close();  
        }
        public void FetchOrganization(IList<User> users)
        {
            foreach (User user in users)
            {
                FetchOrganization(user);
            }
        }
        public void FetchPlant(User user)
        {
            if (user == null)
            {
                return;
            }
            
            IDBContext db = _CreateDBContext();

            IDictionary<string, TMMINPlantOrganization> plantMap = new Dictionary<string, TMMINPlantOrganization>();
            IList<Gentani> gentanis = db.Fetch<Gentani>("Gentani_SelectByUsername", new { Username = user.Username });
            if ((gentanis != null) && (gentanis.Count > 0))
            {
                TMMINPlantOrganization plant;
                TMMINShopOrganization shop;
                TMMINLineOrganization line;
                ShiftOrganization shift;

                string plantCode;
                string shopCode;
                string lineCode;
                ShiftOrganizationType shiftType;
                bool newPlant;
                foreach (Gentani g in gentanis)
                {
                    newPlant = false;
                    plantCode = g.PlantCode;
                    shopCode = g.ShopCode;
                    lineCode = g.LineCode;
                    shiftType = (ShiftOrganizationType)Enum.Parse(typeof(ShiftOrganizationType), g.Shift);

                    shop = new TMMINShopOrganization();
                    shop.Id = g.ShopCode;
                    line = new TMMINLineOrganization();
                    line.Id = g.LineCode;
                    shift = new ShiftOrganization();
                    shift.Type = shiftType;
                    shift.Code = g.SectionCode;
                    shift.Name = g.SectionDescription;

                    if (!plantMap.ContainsKey(plantCode))
                    {
                        newPlant = true;
                        plantMap.Add(plantCode, new TMMINPlantOrganization());
                    }

                    plant = plantMap[plantCode];
                    if (newPlant)
                    {
                        plant.Id = plantCode;
                        line.AddShift(shift);
                        shop.AddLine(line);                        
                        plant.AddShop(shop);
                    }
                    else
                    {
                        if (plant.HasShop(g.ShopCode))
                        {
                            shop = (TMMINShopOrganization)plant.GetShop(shopCode);
                            if (shop.HasLine(lineCode))
                            {
                                line = (TMMINLineOrganization)shop.GetLine(lineCode);
                                if (!line.HasShift(shiftType))
                                {
                                    shift = new ShiftOrganization();
                                    shift.Type = shiftType;
                                    line.AddShift(shift);
                                }
                            }
                            else
                            {
                                shop.AddLine(line);
                            }
                        }
                        else
                        {
                            plant.AddShop(shop);
                        }
                    }
                }

                if (plantMap.Count > 0)
                {
                    IList<PlantOrganization> plants = new List<PlantOrganization>(plantMap.Count);
                    foreach (TMMINPlantOrganization p in plantMap.Values)
                    {
                        plants.Add(p);
                    }
                    user.Plants.Merge(plants);
                }
            }
            db.Close();
        }
        public void FetchPlant(IList<User> users)
        {
            foreach (User user in users)
            {
                FetchOrganization(user);
            }
        }

        public User IsUserAuthentic(string username, string password)
        {
            User user = GetUser(username);
            if (user != null)
            {
                TmminUser tUser = (TmminUser)user;
                if (tUser.InActiveDirectory)
                {
                    string domain = null;
                    string container = null;

                    ConfigurationItem config = ActiveDirectoryConfiguration.GetConfiguration(ReleasePhase);
                    if (config != null)
                    {
                        CompositeConfigurationItem compositeConfig = (CompositeConfigurationItem)config;
                        ConfigurationItem compositeConfigItem = compositeConfig.GetItem("Container");
                        if (compositeConfigItem != null)
                        {
                            container = compositeConfigItem.Value;
                        }
                        compositeConfigItem = compositeConfig.GetItem("Domain");
                        if (compositeConfigItem != null)
                        {
                            domain = compositeConfigItem.Value;
                        }
                    }
                    
                    if ((string.IsNullOrEmpty(domain) || (string.IsNullOrEmpty(container))))
                    {
                        throw new ArgumentNullException("the Domain or Container value cannot be null, please check Database Configuration.");
                    }
                    PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, container);
                    if (context.ValidateCredentials(username, password))
                    {
                        return null;
                    }
                }
                else
                {
                    if (user.Password.Equals(password))
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        private void _IterateOrganization(IDBContext db, OrganizationStructure position, IList<OrganizationStructure> bufferOrganization, Queue<string> sqlQueue, string unitCode)
        {
            if (sqlQueue.Count == 0) return;
            bufferOrganization = db.Fetch<OrganizationStructure>(sqlQueue.Dequeue(), new { Code = unitCode });
            if ((bufferOrganization != null) && (bufferOrganization.Count > 0))
            {
                position.Organizations = bufferOrganization;
                foreach (OrganizationStructure org in bufferOrganization)
                {
                    _IterateOrganization(db, org, bufferOrganization, sqlQueue, org.ParentId);
                }
            }
            else
            {
                _IterateOrganization(db, position, bufferOrganization, sqlQueue, position.Id);
            }
        }
        private TmminUser _NormalizedUserModel(TmminUserModel model, IDBContext db)
        {
            if (!model.IsNull())
            {
                TmminUser user = new TmminUser();
                user.Id = model.Id;
                user.RegistrationNumber = model.RegistrationNumber;
                user.Username = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PasswordExpirationDate = model.PasswordExpirationDate;
                user.BirthDate = model.BirthDate;
                if (!string.IsNullOrEmpty(model._Email))
                {
                    user.Emails.Add(model._Email);
                }
                if (!string.IsNullOrEmpty(model._PhoneNumber))
                {
                    user.PhoneNumbers.Add(new PhoneNumber() { Category = PhoneNumberCategory.Work, Number = model._PhoneNumber });
                }
                if (!string.IsNullOrEmpty(model._MobilePhoneNumber))
                {
                    user.PhoneNumbers.Add(new PhoneNumber() { Category = PhoneNumberCategory.Mobile, Number = model._MobilePhoneNumber });
                }
                if (!string.IsNullOrEmpty(model._ExtensionPhoneNumber))
                {
                    user.PhoneNumbers.Add(new PhoneNumber() { Category = PhoneNumberCategory.Extension, Number = model._ExtensionPhoneNumber });
                }
                if (!string.IsNullOrEmpty(model._CompanyId))
                {
                    _FetchCompany(model, db);
                    user.Company = model.Company;
                }
                if (!string.IsNullOrEmpty(model._LocationId))
                {
                    _FetchLocation(model, db);
                    user.Location = model.Location;
                }
                if (!string.IsNullOrEmpty(model._ClassId))
                {
                    _FetchClass(model, db);
                    user.Class = model.Class;
                }
                if (!string.IsNullOrEmpty(model._CostCenterCode))
                {
                    user.CostCenters.Add(new UserCostCenter() {
                        Id = model._CostCenterCode,
                        Name = model._CostCenterCode
                    });
                }

                return user;
            }
            return null;
        }
        private List<TmminUser> _NormalizedUserModel(IList<TmminUserModel> models, IDBContext db)
        {
            if (!models.IsNullOrEmpty())
            {
                List<TmminUser> result = new List<TmminUser>();
                foreach (TmminUserModel model in models)
                {
                    result.Add(_NormalizedUserModel(model, db));
                }
                return result;
            }
            return null;
        }
        private void _FetchClass(TmminUserModel mUser, IDBContext db)
        {
            IList<NormalizedData> mClasses = db.Fetch<NormalizedData>("Class_Select_ById", new { Id = mUser._ClassId, CompanyId = mUser._CompanyId });
            if (!mClasses.IsNullOrEmpty())
            {
                NormalizedData clazz = mClasses.First();
                mUser.Class = new UserClass() { 
                    Id = clazz.Id,
                    Name = clazz.Name,
                    Description = clazz.Description                    
                };
            }
        }
        private void _FetchLocation(TmminUserModel mUser, IDBContext db)
        {
            IList<NormalizedData> mLocations = db.Fetch<NormalizedData>("Location_Select_ById", new { Id = mUser._LocationId, CompanyId = mUser._CompanyId });
            if (!mLocations.IsNullOrEmpty())
            {
                NormalizedData mLocation = mLocations.First();
                mUser.Location = new UserLocation() { 
                    Id = mLocation.Id,
                    Name = mLocation.Name,
                    Description = mLocation.Description
                };
            }
        }
        private void _FetchCompany(TmminUserModel mUser, IDBContext db)
        {
            IList<TmminCompanyModel> mCompanies = db.Fetch<TmminCompanyModel>("Company_Select_ById", new { Id = mUser._CompanyId });
            if (!mCompanies.IsNullOrEmpty())
            {
                TmminCompanyModel mCompany = mCompanies.First();
                mUser.Company = new UserCompany() { 
                    Alias = mCompany.Alias,
                    Description = mCompany.Description,
                    Id = mCompany.Id,
                    IsDefault = mCompany.IsDefault,
                    Name = mCompany.Name
                };
            }
        }
        private IList<User> _CastModel(IList<TmminUser> mUsers)
        {
            if (!mUsers.IsNullOrEmpty())
            {
                IList<User> users = new List<User>(mUsers.Count);
                foreach (TmminUser model in mUsers)
                {
                    
                }
                return users;
            }

            return null;
        }
        private IDBContext _CreateDBContext()
        {
            IDBContext db;
            if (string.IsNullOrEmpty(contextName))
            {
                db = dbManager.GetContext();
            }
            else
            {
                db = dbManager.GetContext(contextName);
            }
            db.SetExecutionMode(DBContextExecutionMode.ByName);

            return db;
        }

        public IList<User> _SearchUserByName(string name)
        {
            IDBContext db = _CreateDBContext();
            IList<TmminUserModel> tmminUsers = db.Fetch<TmminUserModel>("User_Select_ByName", new { Name = name });
            IList<TmminUser> users = _NormalizedUserModel(tmminUsers, db);
            db.Close();

            if (!users.IsNull())
            {
                return users.Cast<User>().ToList();
            }
            return null;
        }
        public IPagedData<User> _SearchUserByName(string name, long pageNumber, long pageSize)
        {
            IDBContext db = _CreateDBContext();
            IPagedData<TmminUserModel> mUsers = db.FetchByPage<TmminUserModel>("User_Select_ByName", pageNumber, pageSize, new { Name = name });
            IList<TmminUser> users = _NormalizedUserModel(mUsers.GetData(), db);
            IPagedData<User> result = null;
            if (!users.IsNullOrEmpty())
            {
                result = mUsers.Clone<User>(users.Cast<User>().ToList());
            }            
            db.Close();

            return result;
        }
        public IList<User> _SearchUserByFirstName(string firstName)
        {
            IDBContext db = _CreateDBContext();
            IList<TmminUserModel> mUsers = db.Fetch<TmminUserModel>("User_Select_ByFirstName", new { FirstName = firstName });
            IList<TmminUser> users = _NormalizedUserModel(mUsers, db);
            db.Close();

            if (!users.IsNull())
            {
                return users.Cast<User>().ToList();
            }
            return null;
        }
        public IPagedData<User> _SearchUserByFirstName(string firstName, long pageNumber, long pageSize)
        {
            IDBContext db = _CreateDBContext();
            IPagedData<TmminUserModel> mUsers = db.FetchByPage<TmminUserModel>("User_Select_ByFirstName", pageNumber, pageSize, new { FirstName = firstName });
            IList<TmminUser> users = _NormalizedUserModel(mUsers.GetData(), db);
            IPagedData<User> result = null;
            if (!users.IsNull())
            {
                result = mUsers.Clone<User>(users.Cast<User>().ToList());
            }            
            db.Close();

            return result;
        }
        public IList<User> _SearchUserByLastName(string lastName)
        {
            IDBContext db = _CreateDBContext();
            IList<TmminUserModel> mUsers = db.Fetch<TmminUserModel>("User_Select_ByLastName", new { LastName = lastName });
            IList<TmminUser> users = _NormalizedUserModel(mUsers, db);
            db.Close();

            if (!users.IsNull())
            {
                return users.Cast<User>().ToList();
            }
            return null;
        }
        public IPagedData<User> _SearchUserByLastName(string lastName, long pageNumber, long pageSize)
        {
            IDBContext db = _CreateDBContext();
            IPagedData<TmminUserModel> mUsers = db.FetchByPage<TmminUserModel>("User_Select_ByLastName", pageNumber, pageSize, new { LastName = lastName });
            IList<TmminUser> users = _NormalizedUserModel(mUsers.GetData(), db);
            IPagedData<User> result = null;
            if (!users.IsNull())
            {
                result = mUsers.Clone<User>(users.Cast<User>().ToList());
            }            
            db.Close();

            return result;
        }
        public IList<User> _SearchUserByAccountValidity(DateTime validDate)
        {
            dynamic param = new ExpandoObject();
            param.ValidDate = SqlDateTime.MinValue;
            if (!validDate.IsNull() && (validDate != DateTime.MinValue))
            {
                param.ValidDate = validDate;
            }

            IDBContext db = _CreateDBContext();
            IList<TmminUserModel> mUsers = db.Fetch<TmminUserModel>("User_Select_ByAccountValidity", new { ValidDate = param.ValidDate });
            IList<TmminUser> users = _NormalizedUserModel(mUsers, db);
            db.Close();

            if (!users.IsNull())
            {
                return users.Cast<User>().ToList();
            }
            return null;
        }
        public IPagedData<User> _SearchUserByAccountValidity(DateTime validDate, long pageNumber, long pageSize)
        {
            dynamic param = new ExpandoObject();
            param.ValidDate = SqlDateTime.MinValue;
            if (!validDate.IsNull() && (validDate != DateTime.MinValue))
            {
                param.ValidDate = validDate;
            }

            IDBContext db = _CreateDBContext();
            IPagedData<TmminUserModel> mUsers = db.FetchByPage<TmminUserModel>("User_Select_ByAccountValidity", pageNumber, pageSize, new { ValidDate = param.ValidDate });
            IList<TmminUser> users = _NormalizedUserModel(mUsers.GetData(), db);
            IPagedData<User> result = null;
            if (!users.IsNull())
            {
                result = mUsers.Clone<User>(users.Cast<User>().ToList());
            }            
            db.Close();

            return result;
        }
    }
}
