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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    public partial class Quota : WebsitePanelControlBase
    {
        public string QuotaName
        {
            get { return (ViewState["QuotaName"] != null) ? (string)ViewState["QuotaName"] : ""; }
            set { ViewState["QuotaName"] = value; }
        }

        public bool DisplayGauge
        {
            get { return quotaViewer.DisplayGauge; }
            set { quotaViewer.DisplayGauge = value; }
        }

        public int QuotaAllocatedValue
        {
            set { quotaViewer.QuotaValue = value; }
        }

        public QuotaViewer Viewer
        {
            get { return quotaViewer; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindQuota();
            }
        }

        public void BindQuota()
        {
            try
            {
                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                // get quota
                if (cntx.Quotas.ContainsKey(QuotaName))
                {
                    QuotaValueInfo quota = cntx.Quotas[QuotaName];
                    quotaViewer.QuotaTypeId = quota.QuotaTypeId;
                    quotaViewer.QuotaUsedValue = quota.QuotaUsedValue;
                    quotaViewer.QuotaValue = quota.QuotaAllocatedValue;
                    quotaViewer.QuotaAvailable = -1;
                	//this.Visible = quota.QuotaAllocatedValue != 0;
                }
                else
                {
                	this.Visible = false;
                    quotaViewer.QuotaTypeId = 1; // bool
                    quotaViewer.QuotaUsedValue = 0;
                    quotaViewer.QuotaValue = 0;
                    quotaViewer.QuotaAvailable = -1;
                }
            }
            catch
            {
                /* do nothing */
            }
        }
    }
}
