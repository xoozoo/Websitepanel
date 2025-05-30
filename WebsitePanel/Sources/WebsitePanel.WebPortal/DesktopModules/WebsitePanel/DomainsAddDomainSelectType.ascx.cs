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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    public partial class DomainsAddDomainSelectType : WebsitePanelControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindControls();
        }

        private void BindControls()
        {
            // set navigate URLs
            DomainLink.NavigateUrl = GetAddDomainLink("Domain");
            SubDomainLink.NavigateUrl = GetAddDomainLink("SubDomain");
            ProviderSubDomainLink.NavigateUrl = GetAddDomainLink("ProviderSubDomain");
            DomainPointerLink.NavigateUrl = GetAddDomainLink("DomainPointer");

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            DomainLink.Enabled = (cntx.Quotas.ContainsKey(Quotas.OS_DOMAINS) && !cntx.Quotas[Quotas.OS_DOMAINS].QuotaExhausted);

            if (DomainLink.Enabled)
            {
                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

                if (user != null)
                {
                    if (user.Role == UserRole.User)
                    {
                        DomainLink.Enabled = !Utils.CheckQouta(Quotas.OS_NOTALLOWTENANTCREATEDOMAINS, cntx);
                    }
                }
            }

            

            DomainInfo[] myDomains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId);
            bool enableSubDomains = false;
            foreach(DomainInfo domain in myDomains)
            {
                if(!domain.IsSubDomain && !domain.IsInstantAlias && !domain.IsDomainPointer)
                {
                    enableSubDomains = true;
                    break;
                }
            }
            SubDomainLink.Enabled = (cntx.Quotas.ContainsKey(Quotas.OS_SUBDOMAINS) && !cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted
                && enableSubDomains);

            ProviderSubDomainPanel.Visible = (cntx.Quotas.ContainsKey(Quotas.OS_SUBDOMAINS) && !cntx.Quotas[Quotas.OS_SUBDOMAINS].QuotaExhausted
                && ES.Services.Servers.GetResellerDomains(PanelSecurity.PackageId).Length > 0);

            DomainPointerLink.Enabled = (cntx.Quotas.ContainsKey(Quotas.OS_DOMAINPOINTERS) && !cntx.Quotas[Quotas.OS_DOMAINPOINTERS].QuotaExhausted);
        }

        private string GetAddDomainLink(string domainType)
        {
            string returnUrl = (ViewState["ReturnURL"] != null) ? Server.UrlEncode(ViewState["ReturnURL"].ToString()) : "";

            return EditUrl("DomainType", domainType, "add_domain_step2",
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ReturnURL=" + returnUrl);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectBack();
        }

        private void RedirectBack()
        {
            if (ViewState["ReturnURL"] != null)
                Response.Redirect((string)ViewState["ReturnURL"]);
            else
                RedirectSpaceHomePage();
        }
    }
}
