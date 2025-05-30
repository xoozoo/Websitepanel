// Copyright (c) 2019, WebsitePanel-Support.net.
// Distributed by websitepanel-support.net
// Build and fixed by Key4ce - IT Professionals
// https://www.key4ce.com
// 
// Original source:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebsitePanel.Providers;

namespace WebsitePanel.EnterpriseServer.Extensions
{
    public static class LogExtension
    {
        private const string OLD_PREFIX = "Old ";
        private const string NEW_PREFIX = "New ";

        /// <summary>
        /// Log properties of the object which have [LogProperty] or [LogParentPropery] attributes
        /// </summary>
        public static void WriteObject<T>(T obj)
            where T : class
        {
            if (obj == null)
                return;

            try
            {
                // Log Parent Properties Attribute
                var logPropertyAttributes = GetAttributes<T, LogParentPropertyAttribute>();
                foreach (var logPropertyAttribute in logPropertyAttributes)
                {
                    TaskManager.Write(logPropertyAttribute.GetLogString(obj));
                }

                // Log Properties Attribute
                var properties =
                    typeof (T).GetProperties().Where(prop => prop.IsDefined(typeof (LogPropertyAttribute), false));
                foreach (var property in properties)
                {
                    var attributes = GetAttributes<LogPropertyAttribute>(property);
                    foreach (var logPropertyAttribute in attributes)
                    {
                        TaskManager.Write(logPropertyAttribute.GetLogString(obj, property));
                    }
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        /// <summary>
        /// Log properties of the object which have [LogProperty] or [LogParentPropery] attributes
        /// </summary>
        public static void WriteObject<T, TU>(T obj, params Expression<Func<T, TU>>[] additionalProperties)
            where T : class
        {
            WriteObject(obj);

            LogProperties(obj, additionalProperties);
        }

        /// <summary>
        /// Log a value of the property from expression parameter
        /// </summary>
        public static T LogProperty<T, TU>(this T obj, Expression<Func<T, TU>> expression, string nameInLog = null)
            where T : class
        {
            try
            {
                var property = ObjectUtils.GetProperty(obj, expression);

                var propertyName = string.IsNullOrEmpty(nameInLog)
                    ? LogExtensionHelper.DecorateName(property.Name)
                    : nameInLog;
                var propertyValue = LogExtensionHelper.GetString(property.GetValue(obj, null));

                TaskManager.Write(LogExtensionHelper.CombineString(propertyName, propertyValue));

            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            return obj;
        }

        /// <summary>
        /// Log a value of the property from expression parameter
        /// </summary>
        public static void LogProperties<T, TU>(this T obj, params Expression<Func<T, TU>>[] expressions)
            where T : class
        {
            try
            {
                foreach (var expression in expressions)
                {
                    LogProperty(obj, expression);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        /// <summary>
        /// Log changed values of the all properties of the object
        /// </summary>
        public static void LogPropertiesIfChanged<T>(T oldObj, T newObj, bool withOld = true) where T : class
        {
            if (oldObj == null || newObj == null)
                return;

            try
            {
                foreach (var property in typeof (T).GetProperties())
                {
                    object newValue;
                    try
                    {
                        newValue = property.GetValue(newObj, null);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.Write("Can't get {0} property", property.Name);
                        continue;
                    }
                    LogPropertyIfChanged(oldObj, property, newValue, withOld);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        /// <summary>
        /// Log a value of the property only when it is different from newValue parameter. Also the old value can be written.
        /// </summary>
        public static T LogPropertyIfChanged<T, TU>(this T obj, Expression<Func<T, TU>> expression, object newValue,
            bool withOld = true) where T : class
        {
            if (obj == null || expression == null)
                return obj;

            try
            {
                var property = ObjectUtils.GetProperty(obj, expression);

                return LogPropertyIfChanged(obj, property, newValue, withOld);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            return obj;
        }

        /// <summary>
        /// Log values of variables which will be written with variable name
        /// </summary>
        /// <param name="variablesObs">Dynamic object with variables instead of properties like: new {varA, varB}</param>
        /// <param name="prefix">Add the string before a variable name</param>
        public static void WriteVariables(object variablesObs, string prefix = "")
        {
            try
            {
                if (variablesObs == null)
                    throw new ArgumentException("variablesObs"); 
                
                foreach (var property in variablesObs.GetType().GetProperties())
                {
                    var parameterName = LogExtensionHelper.DecorateName(property.Name);
                    var parameterValue = LogExtensionHelper.GetString(property.GetValue(variablesObs, null));

                    TaskManager.Write(prefix + LogExtensionHelper.CombineString(parameterName, parameterValue));
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        /// <summary>
        /// Write log in the format "name: value"
        /// </summary>
        public static void WriteVariable(string name, string value)
        {
            try
            {
                TaskManager.Write(LogExtensionHelper.CombineString(name, value));
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        /// <summary>
        /// Set item name for current Task
        /// </summary>
        /// <param name="itemName"></param>
        public static void SetItemName(string itemName)
        {
            try
            {
                TaskManager.ItemName = itemName;
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        #region Private methods

        private static TAttr[] GetAttributes<TObj, TAttr>()
            where TObj : class
            where TAttr : class
        {
            return (TAttr[]) typeof (TObj).GetCustomAttributes(typeof (TAttr), false);
        }

        private static T[] GetAttributes<T>(ICustomAttributeProvider member) where T : class
        {
            return (T[]) member.GetCustomAttributes(typeof (T), false);
        }

        private static T LogPropertyIfChanged<T>(T obj, PropertyInfo property, object newValue, bool withOld = true)
            where T : class
        {
            if (property == null)
                return obj;

            object oldValue;
            try
            {
                oldValue = property.GetValue(obj, null);
            }
            catch (Exception ex)
            {
                TaskManager.Write("Cant get {0} property", property.Name);
                return obj;
            }

            var oldValueStr = LogExtensionHelper.GetString(oldValue);
            var newValueStr = LogExtensionHelper.GetString(newValue);

            if (oldValue == null || newValue == null)
            {
                if (oldValue != newValue)
                {
                    WriteChangedProperty(obj, property, oldValueStr, newValueStr, withOld);
                }
            }
            else if (!oldValueStr.Equals(newValueStr))
            {
                WriteChangedProperty(obj, property, oldValueStr, newValueStr, withOld);
            }

            return obj;
        }

        private static void WriteChangedProperty(object obj, PropertyInfo property, string oldValue, string newValue,
            bool withOld = true)
        {
            var attributes = GetAttributes<LogPropertyAttribute>(property);

            if (attributes != null && attributes.Length > 0)
            {
                foreach (var logPropertyAttribute in attributes)
                {
                    if (logPropertyAttribute == null)
                        continue;
                    
                    if (withOld)
                        TaskManager.Write(OLD_PREFIX + logPropertyAttribute.GetLogString(obj, property));

                    TaskManager.Write(NEW_PREFIX + logPropertyAttribute.GetLogString(property.Name, newValue));
                }
            }
            else
            {
                var name = LogExtensionHelper.DecorateName(property.Name);

                if (withOld)
                    TaskManager.Write(OLD_PREFIX + LogExtensionHelper.CombineString(name, oldValue));

                TaskManager.Write(NEW_PREFIX + LogExtensionHelper.CombineString(name, newValue));
            }

        }

        private static void WriteException(Exception ex)
        {
            TaskManager.WriteWarning("Extension Log Error: " + ex.Message + ex.StackTrace);
        }

        #endregion

    }
}
