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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Providers
{
	/// <summary>
	/// Summary description for ServiceProviderItem.
	/// </summary>
	[Serializable]
	public abstract class ServiceProviderItem 
	{
		private int id;
        private int typeId = 0;
		private int packageId = -1;
		private int serviceId = -1;
		private string name;
        private string[] properties;
        private string groupName;

        private StringDictionary propsHash = null;

		public ServiceProviderItem()
		{
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

		public int PackageId
		{
			get { return packageId; }
			set { packageId = value; }
		}

		public int ServiceId
		{
			get { return serviceId; }
			set { serviceId = value; }
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

        public string GroupName
        {
            get { return this.groupName; }
            set { this.groupName = value; }
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }
        
        public string[] Properties
        {
            get
            {
                if (propsHash == null)
                    return null;

                properties = new string[propsHash.Count];
                int i = 0;
                foreach (string key in propsHash.Keys)
                    properties[i++] = key + "=" + propsHash[key];

                return properties;
            }
            set
            {
                if (value == null)
                    return;

                properties = value;

                // fill hash
                propsHash = new StringDictionary();
                foreach (string pair in value)
                {
                    int idx = pair.IndexOf('=');
                    string name = pair.Substring(0, idx);
                    string val = pair.Substring(idx + 1);
                    propsHash.Add(name, val);
                }
            }
        }

        [XmlIgnore]
        public string this[string propertyName]
        {
            get
            {
                if (propsHash == null)
                    propsHash = new StringDictionary();

                return propsHash[propertyName];
            }
            set
            {
                if (propsHash == null)
                    propsHash = new StringDictionary();

                propsHash[propertyName] = value;
            }
        }

		public T GetValue<T>(string propertyName)
		{
			string strValue = this[propertyName];
			//
			if (String.IsNullOrEmpty(strValue))
				return default(T);
			//
			return (T)Convert.ChangeType(strValue, typeof(T));
		}

		public void SetValue<T>(string propertyName, T propertyValue)
		{
			this[propertyName] = Convert.ToString(propertyValue);
		}
	}
}
