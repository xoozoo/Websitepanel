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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Portal.UserControls
{
    public partial class PackagePhoneNumbers : WebsitePanelControlBase
    {
        private bool spaceOwner;

        private IPAddressPool pool;
        public IPAddressPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        private string editItemControl;
        public string EditItemControl
        {
            get { return editItemControl; }
            set { editItemControl = value; }
        }

        private string spaceHomeControl;
        public string SpaceHomeControl
        {
            get { return spaceHomeControl; }
            set { spaceHomeControl = value; }
        }

        private string allocateAddressesControl;
        public string AllocateAddressesControl
        {
            get { return allocateAddressesControl; }
            set { allocateAddressesControl = value; }
        }

        public bool ManageAllowed
        {
            get { return ViewState["ManageAllowed"] != null ? (bool)ViewState["ManageAllowed"] : false; }
            set { ViewState["ManageAllowed"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.IPAddress"));
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();

            bool isUserSelected = PanelSecurity.SelectedUser.Role == WebsitePanel.EnterpriseServer.UserRole.User;
            bool isUserLogged = PanelSecurity.EffectiveUser.Role == WebsitePanel.EnterpriseServer.UserRole.User;
            spaceOwner = PanelSecurity.EffectiveUserId == PanelSecurity.SelectedUserId;

            gvAddresses.Columns[3].Visible = !isUserSelected; // space
            gvAddresses.Columns[4].Visible = !isUserSelected; // user

            // managing external network permissions
            gvAddresses.Columns[0].Visible = !isUserLogged && ManageAllowed;
            btnAllocateAddress.Visible = !isUserLogged && !spaceOwner && ManageAllowed && !String.IsNullOrEmpty(AllocateAddressesControl);
            btnDeallocateAddresses.Visible = !isUserLogged && ManageAllowed;
        }

        public string GetItemEditUrl(string itemID)
        {
            return HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), EditItemControl,
                    "ItemID=" + itemID);
        }

        public string GetSpaceHomeUrl(string spaceId)
        {
            return HostModule.EditUrl("SpaceID", spaceId, SpaceHomeControl);
        }

        protected void odsExternalAddressesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOXES", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAllocateAddress_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), AllocateAddressesControl,
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }

        protected void gvAddresses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PackageIPAddress item = e.Row.DataItem as PackageIPAddress;
            if (item != null)
            {
                // checkbox
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                chkSelect.Enabled = (!spaceOwner || (PanelSecurity.PackageId != item.PackageId)) && item.ItemId == 0;
            }
        }

        protected void btnDeallocateAddresses_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();

            try
            {
                List<int> items = new List<int>();
                for (int i = 0; i < gvAddresses.Rows.Count; i++)
                {
                    GridViewRow row = gvAddresses.Rows[i];
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                        items.Add((int)gvAddresses.DataKeys[i].Value);
                }

                // check if at least one is selected
                if (items.Count == 0)
                {
                    messageBox.ShowWarningMessage("PHONE_EDIT_LIST_EMPTY_ERROR");
                    return;
                }

                ResultObject res = ES.Services.Servers.DeallocatePackageIPAddresses(PanelSecurity.PackageId, items.ToArray());
                messageBox.ShowMessage(res, "DEALLOCATE_SPACE_PHONE_NUMBER", "VPS");
                gvAddresses.DataBind();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("DEALLOCATE_SPACE_PHONE_NUMBER", ex);
            }
        }

        protected void odsExternalAddressesPaged_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["pool"] = Pool;
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'PackageIPAddresses'");
            res.Append(", RedirectUrl: '" + GetItemEditUrl("{0}").Substring(2) + "'");
            res.Append(", PackageID: " + (String.IsNullOrEmpty(Request["SpaceID"]) ? "0" : Request["SpaceID"]));
            res.Append(", OrgID: " + (String.IsNullOrEmpty(Request["ItemID"]) ? "0" : Request["ItemID"]));
            res.Append(", PoolID: " + Pool != null ? Pool.ToString() : "0");
            res.Append(", Recursive: true");
            return res.ToString();
        }
    }
}
