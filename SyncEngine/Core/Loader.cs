using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SyncEngine.Core;
using SyncEngine.Core.Configuration; 
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SyncEngine.Core
{
    public class Loader : Disposer
    {
        private readonly ILogger<Loader> logger;

        public Loader(ILogger<Loader> logger)
        {
            this.logger = logger;
        }

        public void Load(object target, string[] fields)
        {
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();
            
            foreach (PropertyInfo property in properties)  
            {
                if (property.CanWrite)                      
                    {
                        object[] attributes = property.GetCustomAttributes(typeof(PositionAttribute), false);
                        
                        if (attributes.Length > 0)
                        {
                            PositionAttribute positionAttr = 
                                (PositionAttribute)attributes[0];
                                        
                            int position = positionAttr.Position;

                            try
                            {
                                object data = fields[position];
                                if (positionAttr.DataTransform != string.Empty)
                                {
                                    MethodInfo method = targetType.GetMethod(positionAttr.DataTransform);
                                    data = method.Invoke(target, new object[] { data });
                                }
                                
                                if (data != null)
                                {
                                    var propertyType = property.PropertyType;
                                    var convertedData = Convert.ChangeType(data, propertyType);
                                    property.SetValue(target, convertedData, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message != NullObjectError)
                                    logger.LogError("ERROR LOADING FILE :: " + ex.Message.ToString());
                            }        
                        }
                }    
            }
        }

        private static string NullObjectError = "Null object cannot be converted to a value type.";

        public X LoadNew<X>(string[] fields)
        {   
            X tempObj = (X) Activator.CreateInstance(typeof(X));
            Load(tempObj, fields);
            return tempObj;
        }
    }
}