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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal.ExchangeServer.UserControls
{
    public partial class EnterpriseStorageOwaUsersList : WebsitePanelControlBase
    {
        public const string DirectionString = "DirectionString";

        protected enum SelectedState
        {
            All,
            Selected,
            Unselected
        }

        public void SetUsers(OrganizationUser[] users)
        {
            BindAccounts(users, false);
        }

        public OrganizationUser[] GetUsers()
        {
            return GetGridViewUsers(EnterpriseStorageOwaUsersList.SelectedState.All).ToArray();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // register javascript
            if (!Page.ClientScript.IsClientScriptBlockRegistered("SelectAllCheckboxes"))
            {
                string script = @"    function SelectAllCheckboxes(box)
                {
		            var state = box.checked;
                    var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName(""INPUT"");
                    for(i = 0; i < elm.length; i++)
                        if(elm[i].type == ""checkbox"" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
		                    elm[i].checked = state;
                }";
                Page.ClientScript.RegisterClientScriptBlock(typeof(EnterpriseStorageOwaUsersList), "SelectAllCheckboxes",
                    script, true);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // bind all accounts
            BindPopupAccounts();

            // show modal
            AddAccountsModal.Show();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<OrganizationUser> selectedAccounts = GetGridViewUsers(EnterpriseStorageOwaUsersList.SelectedState.Unselected);

            BindAccounts(selectedAccounts.ToArray(), false);
        }

        protected void btnAddSelected_Click(object sender, EventArgs e)
        {
            List<OrganizationUser> selectedAccounts = GetGridViewAccounts();

            BindAccounts(selectedAccounts.ToArray(), true);

        }

        public string GetAccountImage(int accountTypeId)
        {
            string imgName = string.Empty;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            switch (accountType)
            {
                case ExchangeAccountType.Room:
                    imgName = "room_16.gif";
                    break;
                case ExchangeAccountType.Equipment:
                    imgName = "equipment_16.gif";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }

        protected void BindPopupAccounts()
        {
            ExchangeAccount[] accounts = ES.Services.EnterpriseStorage.SearchESAccounts(PanelRequest.ItemID,
                ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "");

            accounts = accounts.Where(x => !GetUsers().Select(p => p.AccountName).Contains(x.AccountName)).ToArray();

            Array.Sort(accounts, CompareAccount);

            if (Direction == SortDirection.Ascending)
            {
                Array.Reverse(accounts);
                Direction = SortDirection.Descending;
            }
            else
                Direction = SortDirection.Ascending;

            gvPopupAccounts.DataSource = accounts;
            gvPopupAccounts.DataBind();
        }

        protected void BindAccounts(OrganizationUser[] newUsers, bool preserveExisting)
        {
            // get binded addresses
            List<OrganizationUser> users = new List<OrganizationUser>();
            if (preserveExisting)
                users.AddRange(GetGridViewUsers(EnterpriseStorageOwaUsersList.SelectedState.All));

            // add new accounts
            if (newUsers != null)
            {
                foreach (OrganizationUser newUser in newUsers)
                {
                    // check if exists
                    bool exists = false;
                    foreach (OrganizationUser user in users)
                    {
                        if (String.Compare(user.AccountName, newUser.AccountName, true) == 0)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (exists)
                        continue;

                    users.Add(newUser);
                }
            }

            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }

        protected List<OrganizationUser> GetGridViewUsers(EnterpriseStorageOwaUsersList.SelectedState state)
        {
            List<OrganizationUser> users = new List<OrganizationUser>();
            for (int i = 0; i < gvUsers.Rows.Count; i++)
            {
                GridViewRow row = gvUsers.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                OrganizationUser user = new OrganizationUser();
                user.AccountName = (string)gvUsers.DataKeys[i][0];
                user.DisplayName = ((Literal)row.FindControl("litAccount")).Text;
                user.AccountId = Convert.ToInt32(((HiddenField)row.FindControl("hdnAccountId")).Value);

                if (state == EnterpriseStorageOwaUsersList.SelectedState.All ||
                    (state == EnterpriseStorageOwaUsersList.SelectedState.Selected && chkSelect.Checked) ||
                    (state == EnterpriseStorageOwaUsersList.SelectedState.Unselected && !chkSelect.Checked))
                    users.Add(user);
            }

            return users;
        }

        protected List<OrganizationUser> GetGridViewAccounts()
        {
            List<OrganizationUser> accounts = new List<OrganizationUser>();
            for (int i = 0; i < gvPopupAccounts.Rows.Count; i++)
            {
                GridViewRow row = gvPopupAccounts.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                if (chkSelect.Checked)
                {
                    accounts.Add(new OrganizationUser
                    {
                        AccountName = (string)gvPopupAccounts.DataKeys[i][0],
                        DisplayName = ((Literal)row.FindControl("litDisplayName")).Text,
                        AccountId = Convert.ToInt32(((HiddenField)row.FindControl("hdnAccountId")).Value)
                    });
                }
            }

            return accounts;

        }

        protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindPopupAccounts();
        }

        protected SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        protected static int CompareAccount(ExchangeAccount user1, ExchangeAccount user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }
    }
}
