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
using System.Text;

using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;

namespace WebsitePanel.EnterpriseServer
{
    public class EnterpriseServerProxyConfigurator
    {
        private string enterpriseServerUrl;
        private string username;
        private string password;

        public string EnterpriseServerUrl
        {
            get { return this.enterpriseServerUrl; }
            set { this.enterpriseServerUrl = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public void Configure(WebServicesClientProtocol proxy)
        {
            // set proxy URL
            string serverUrl = enterpriseServerUrl.Trim();
            if (serverUrl.Length == 0)
                throw new Exception("Enterprise Server URL could not be empty");

            int idx = proxy.Url.LastIndexOf("/");

            // strip the last slash if any
            if (serverUrl[serverUrl.Length - 1] == '/')
                serverUrl = serverUrl.Substring(0, serverUrl.Length - 1);

            proxy.Url = serverUrl + proxy.Url.Substring(idx);

            // set timeout
            proxy.Timeout = 900000; //15 minutes // System.Threading.Timeout.Infinite;

            if (!String.IsNullOrEmpty(username))
            {
                // create assertion
                UsernameAssertion assert = new UsernameAssertion(username, password);

                // apply policy
                Policy policy = new Policy();
                policy.Assertions.Add(assert);

                proxy.SetPolicy(policy);
            }
        }
    }
}
