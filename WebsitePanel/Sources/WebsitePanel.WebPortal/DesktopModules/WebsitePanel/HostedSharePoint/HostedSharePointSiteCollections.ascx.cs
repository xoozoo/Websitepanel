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
using System.Collections.Generic;
using WebsitePanel.EnterpriseServer.Base.HostedSolution;
using WebsitePanel.Providers.SharePoint;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal
{
	public partial class HostedSharePointSiteCollections :  WebsitePanelModuleBase
	{
 
		protected void Page_Load(object sender, EventArgs e)
		{
			this.BindStats();
		}

		private void BindStats()
		{
			// quota values
			OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

			siteCollectionsQuota.QuotaUsedValue = stats.CreatedSharePointSiteCollections;
			siteCollectionsQuota.QuotaValue = stats.AllocatedSharePointSiteCollections;
            if (stats.AllocatedSharePointSiteCollections != -1) siteCollectionsQuota.QuotaAvailable = stats.AllocatedSharePointSiteCollections - stats.CreatedSharePointSiteCollections;
		}

		protected void btnCreateSiteCollection_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "sharepoint_edit_sitecollection", "SpaceID=" + PanelSecurity.PackageId.ToString()));
		}

		public string GetSiteCollectionEditUrl(string siteCollectionId)
		{
			return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_edit_sitecollection",
					"SiteCollectionID=" + siteCollectionId,
					"ItemID=" + PanelRequest.ItemID.ToString());
		}

		protected void odsSharePointSiteCollectionPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.Exception != null)
			{
				messageBox.ShowErrorMessage("HOSTEDSHAREPOINT_GET_SITECOLLECTIONS", e.Exception);
				e.ExceptionHandled = true;
			}
		}

		protected void gvSiteCollections_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "DeleteItem")
			{
				int siteCollectionId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

				try
				{
					int result = ES.Services.HostedSharePointServers.DeleteSiteCollection(siteCollectionId);
					if (result < 0)
					{
						messageBox.ShowResultMessage(result);
						return;
					}

					gvSiteCollections.DataBind();
					this.BindStats();
				}
				catch (Exception ex)
				{
					messageBox.ShowErrorMessage("HOSTEDSHAREPOINT_DELETE_SITECOLLECTION", ex);
				}
			}
		}
	}
}
