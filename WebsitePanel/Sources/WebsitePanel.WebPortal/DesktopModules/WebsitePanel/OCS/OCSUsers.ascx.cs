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
﻿using WebsitePanel.EnterpriseServer.Base.HostedSolution;
﻿using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal.OCS
{
    public partial class OCSUsers : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindStats();
        }

        private void BindStats()
        {            
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            int allocatedOCSUsers = stats.AllocatedOCSUsers;
            int usedUsers = stats.CreatedOCSUsers;
            usersQuota.QuotaUsedValue = usedUsers;
            usersQuota.QuotaValue = allocatedOCSUsers;

            if (stats.AllocatedOCSUsers != -1) usersQuota.QuotaAvailable = stats.AllocatedOCSUsers - stats.CreatedOCSUsers;
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_new_ocs_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage()
        {

            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId, string instanceId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_ocs_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID, 
                    "InstanceID=" + instanceId);
        }



        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("OCS_GET_USERS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    ResultObject res  = ES.Services.OCS.DeleteOCSUser(PanelRequest.ItemID, e.CommandArgument.ToString());

                    messageBox.ShowMessage(res, "DELETE_OCS_USER", "OCS");
                    
                    // rebind grid
                    gvUsers.DataBind();
                    BindStats();
                    
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("DELETE_OCS_USERS", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsers.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);

            // rebind grid
            gvUsers.DataBind();

            // bind stats
            BindStats();

        }

    }
}
