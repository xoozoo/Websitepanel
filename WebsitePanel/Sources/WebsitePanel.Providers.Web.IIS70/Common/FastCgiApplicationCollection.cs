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

namespace WebsitePanel.Providers.Web.Iis.Common
{
    using Microsoft.Web.Administration;
    using Utility;
    using System;
    using System.Reflection;

    internal sealed class FastCgiApplicationCollection : ConfigurationElementCollectionBase<FastCgiApplication>
    {
        public FastCgiApplication Add(string fullPath, string arguments)
        {
            FastCgiApplication element = base.CreateElement();
            element.FullPath = fullPath;
            if (ConfigurationUtility.ShouldPersist(element.Arguments, arguments))
            {
                element.Arguments = arguments;
            }
            return base.Add(element);
        }

        protected override FastCgiApplication CreateNewElement(string elementTagName)
        {
            return new FastCgiApplication();
        }

        public FastCgiApplication this[string fullPath, string arguments]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    FastCgiApplication application = base[i];
                    if (string.Equals(application.FullPath, fullPath, StringComparison.OrdinalIgnoreCase) && string.Equals(application.Arguments, arguments, StringComparison.OrdinalIgnoreCase))
                    {
                        return application;
                    }
                }
                return null;
            }
        }
    }
}

