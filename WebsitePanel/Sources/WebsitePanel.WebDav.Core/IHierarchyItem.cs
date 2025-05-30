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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace WebsitePanel.WebDav.Core
{
    namespace Client
    {
        public interface IHierarchyItem : IConnectionSettings
        {
            string Comment { get; }
            DateTime CreationDate { get; }
            string CreatorDisplayName { get; }
            string DisplayName { get; }
            bool IsRootItem { get; set; }
            Uri Href { get; }
            ItemType ItemType { get;}
            DateTime LastModified { get; }
            Property[] Properties { get; }

            Property[] GetAllProperties();
            PropertyName[] GetPropertyNames();
            Property[] GetPropertyValues(PropertyName[] names);
            void Delete();
        }

        public class WebDavHierarchyItem : WebDavConnectionSettings, IHierarchyItem
        {
            private Uri _baseUri;
            private string _comment = "";
            private DateTime _creationDate = new DateTime(0);
            private string _creatorDisplayName = "";
            protected ICredentials _credentials = new NetworkCredential();
            private Uri _href;
            private ItemType _itemType;
            private DateTime _lastModified = new DateTime(0);
            private Property[] _properties = {};

            public string Comment
            {
                get { return _comment; }
            }

            public DateTime CreationDate
            {
                get { return _creationDate; }
            }

            public string CreatorDisplayName
            {
                get { return _creatorDisplayName; }
            }

            public string DisplayName
            {
                get
                {
                    var href = HttpUtility.UrlDecode(_href.AbsoluteUri);
                    var baseUri = HttpUtility.UrlDecode(_baseUri.AbsoluteUri);

                    string displayName = href.Replace(baseUri, "");
                    displayName = Regex.Replace(displayName, "\\/$", "");
                    Match displayNameMatch = Regex.Match(displayName, "([\\/]+)$");
                    if (displayNameMatch.Success)
                    {
                        displayName = displayNameMatch.Groups[1].Value;
                    }
                    return HttpUtility.UrlDecode(displayName);
                }
            }

            public bool IsRootItem { get; set; }

            public Uri Href
            {
                get { return _href; }
                set { SetHref(value.ToString(), new Uri(value.Scheme + "://" + value.Host + value.Segments[0] + value.Segments[1])); }
            }

            public ItemType ItemType
            {
                get { return _itemType; }
                set { SetItemType(value); }
            }

            public DateTime LastModified
            {
                get { return _lastModified; }
            }

            public Property[] Properties
            {
                get { return _properties; }
            }

            public Property[] GetAllProperties()
            {
                return _properties;
            }

            public PropertyName[] GetPropertyNames()
            {
                return _properties.Select(p => p.Name).ToArray();
            }

            public Property[] GetPropertyValues(PropertyName[] names)
            {
                return (from p in _properties from pn in names where pn.Equals(p.Name) select p).ToArray();
            }

            public void Delete()
            {
                var credentials = (NetworkCredential) _credentials;
                string auth = "Basic " +
                              Convert.ToBase64String(
                                  Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                WebRequest webRequest = WebRequest.Create(Href);
                webRequest.Method = "DELETE";
                webRequest.Credentials = credentials;
                webRequest.Headers.Add("Authorization", auth);
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (Stream responseStream = webResponse.GetResponseStream())
                    {
                        var buffer = new byte[8192];
                        string result = "";
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                result += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            }
                        } while (bytesRead > 0);
                    }
                }
            }

            public void Delete(LockUriTokenPair[] lockTokens)
            {
            }

            public void Delete(string lockToken)
            {
            }

            public void SetComment(string comment)
            {
                _comment = comment;
            }

            public void SetCreationDate(string creationDate)
            {
                _creationDate = DateTime.Parse(creationDate);
            }
            
            public void SetCreationDate(DateTime creationDate)
            {
                _creationDate = creationDate;
            }

            public void SetCreatorDisplayName(string creatorDisplayName)
            {
                _creatorDisplayName = creatorDisplayName;
            }

            public void SetItemType(ItemType itemType)
            {
                _itemType = itemType;
            }

            public void SetHref(string href, Uri baseUri)
            {
                _href = new Uri(href);
                _baseUri = baseUri;
            }

            public void SetLastModified(string lastModified)
            {
                _lastModified = DateTime.Parse(lastModified);
            }

            public void SetLastModified(DateTime lastModified)
            {
                _lastModified = lastModified;
            }

            public void SetProperty(Property property)
            {
                if (property.Name.Name == "resourcetype" && property.StringValue != String.Empty)
                {
                    var XmlDoc = new XmlDocument();
                    try
                    {
                        if (property.StringValue == "collection")
                        {
                            _itemType = ItemType.Folder;
                        }
                        else
                        {
                            XmlDoc.LoadXml(property.StringValue);
                            property.StringValue = XmlDoc.DocumentElement.LocalName;
                            switch (property.StringValue)
                            {
                                case "collection":
                                    _itemType = ItemType.Folder;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    catch (XmlException e)
                    {
                    }
                }

                bool propertyFound = false;
                foreach (Property prop in _properties)
                {
                    if (prop.Name.Equals(property.Name))
                    {
                        prop.StringValue = property.StringValue;
                        propertyFound = true;
                    }
                }

                if (!propertyFound)
                {
                    var newProperties = new Property[_properties.Length + 1];
                    for (int i = 0; i < _properties.Length; i++)
                    {
                        newProperties[i] = _properties[i];
                    }
                    newProperties[_properties.Length] = property;
                    _properties = newProperties;
                }
            }

            public void SetProperty(PropertyName propertyName, string value)
            {
                SetProperty(new Property(propertyName, value));
            }

            public void SetProperty(string name, string nameSpace, string value)
            {
                SetProperty(new Property(name, nameSpace, value));
            }

            public void SetCredentials(ICredentials credentials)
            {
                _credentials = credentials;
            }
        }
    }
}
