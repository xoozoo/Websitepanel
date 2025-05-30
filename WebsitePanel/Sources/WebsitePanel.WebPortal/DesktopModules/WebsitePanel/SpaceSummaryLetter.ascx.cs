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
    public partial class SpaceSummaryLetter : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindLetter();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SPACE_LETTER_GET", ex);
                    return;
                }
            }
        }

        private void BindLetter()
        {
            string body = null;

            try
            {
                body = ES.Services.Packages.GetEvaluatedPackageTemplateBody(PanelSecurity.PackageId);
            }
            catch (Exception ex)
            {
                body = ex.ToString();
            }
            litContent.Text = body != null ? body : "Your reseller has not setup Hosting Space Summary Letter";

            // bind user details
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package == null)
                RedirectSpaceHomePage();

            // load user details
            UserInfo user = ES.Services.Users.GetUserById(package.UserId);
            txtTo.Text = user.Email;
        }

        private void SendLetter()
        {
            try
            {
                int result = ES.Services.Packages.SendPackageSummaryLetter(PanelSecurity.PackageId,
                    txtTo.Text.Trim(), txtCC.Text.Trim());

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("SPACE_LETTER_SEND");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SPACE_LETTER_SEND", ex);
                return;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SendLetter();
        }
    }
}
