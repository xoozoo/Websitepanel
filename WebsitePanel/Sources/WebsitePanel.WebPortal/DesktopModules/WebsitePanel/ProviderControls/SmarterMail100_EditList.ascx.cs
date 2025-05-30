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
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Mail;
using WebsitePanel.WebPortal.Code.Controls;

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class SmarterMail100_EditList : WebsitePanelControlBase, IMailEditListControl
    {
        private string selectedModerator = null;
        private string selectedListToAddress = null;
        private string selectedListFromAddress = null;
        private string selectedListReplyToAddress = null;
        private string itemName = null;
        private MailEditAddress ctrl = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes["value"] = txtPassword.Text;
            BindListModerators();
        }

        public void BindItem(MailList item)
        {
            itemName = item.Name;
            txtDescription.Text = item.Description;
            if (String.IsNullOrEmpty(item.ModeratorAddress))
            {
                Utils.SelectListItem(ddlListModerators, GetLocalizedString("Text.SelectModerator"));
                selectedModerator = GetLocalizedString("Text.SelectModerator");
            }
            else
            {
                Utils.SelectListItem(ddlListModerators, item.ModeratorAddress);
                selectedModerator = item.ModeratorAddress;
            }

            chkReplyToList.Checked = (item.ReplyToMode == ReplyTo.RepliesToList);
            Utils.SelectListItem(ddlPostingMode, item.PostingMode);
            Utils.SelectListItem(ddlListToAddress, item.ListToAddress);
            selectedListToAddress = item.ListToAddress;
            Utils.SelectListItem(ddlListFromAddress, item.ListFromAddress);
            selectedListFromAddress = item.ListFromAddress;
            Utils.SelectListItem(ddlListReplyToAddress, item.ListReplyToAddress);
            selectedListReplyToAddress = item.ListReplyToAddress;
            txtPassword.Text = item.Password;
            chkPasswordEnabled.Checked = item.RequirePassword;
            cbDigestMode.Checked = item.DigestMode;
            cbSendSubcsribe.Checked = item.SendSubscribe;
            cbSendUnsubscribe.Checked = item.SendUnsubscribe;
            cbAllowUnsubscribe.Checked = item.AllowUnsubscribe;
            cbDisableListcommand.Checked = !item.DisableListcommand;
            cbDisableSubscribecommand.Checked = !item.DisableSubscribecommand;
            txtSubjectPrefix.Text = item.SubjectPrefix;
            chkSubjectPrefixEnabled.Checked = item.EnableSubjectPrefix;
            txtMaxMessageSize.Text = item.MaxMessageSize.ToString();
            txtMaxRecipients.Text = item.MaxRecipientsPerMessage.ToString();

            // members
            mailEditItems.Items = item.Members;
        }

        public void SaveItem(MailList item)
        {
            item.Description = txtDescription.Text;
            if (ddlListModerators.SelectedValue == GetLocalizedString("Text.SelectModerator"))
            {
                item.ModeratorAddress = null;
            }
            else
            {
                item.ModeratorAddress = ddlListModerators.SelectedValue;
            }

            item.ReplyToMode = chkReplyToList.Checked ? ReplyTo.RepliesToList : ReplyTo.RepliesToSender;
            item.PostingMode = (PostingMode)Enum.Parse(typeof(PostingMode), ddlPostingMode.SelectedValue, true);
            item.ListToAddress = ddlListToAddress.SelectedValue;
            item.ListFromAddress = ddlListFromAddress.SelectedValue;
            item.ListReplyToAddress = ddlListReplyToAddress.SelectedValue;
            item.Password = txtPassword.Text;
            item.RequirePassword = chkPasswordEnabled.Checked;
            item.SubjectPrefix = txtSubjectPrefix.Text;
            item.EnableSubjectPrefix = chkSubjectPrefixEnabled.Checked;
            item.DigestMode = cbDigestMode.Checked;
            item.SendSubscribe = cbSendSubcsribe.Checked;
            item.SendUnsubscribe = cbSendUnsubscribe.Checked;
            item.AllowUnsubscribe = cbAllowUnsubscribe.Checked;
            item.DisableListcommand = !cbDisableListcommand.Checked;
            item.DisableSubscribecommand = !cbDisableSubscribecommand.Checked;

            item.MaxMessageSize = Int32.Parse(txtMaxMessageSize.Text);
            item.MaxRecipientsPerMessage = Int32.Parse(txtMaxRecipients.Text);
            item.Members = mailEditItems.Items;
            ctrl = null;
        }

        public void BindListModerators()
        {

            string domainName = null;
            if (!String.IsNullOrEmpty(itemName))
            {
                domainName = GetDomainName(itemName);


                MailAccount[] moderators = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
                ddlListModerators.Items.Clear();
                ddlListModerators.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectModerator"), ""));

                if (moderators != null)
                    foreach (MailAccount account in moderators)
                    {
                        if (GetDomainName(account.Name) == domainName)
                        {
                            if (ddlListModerators != null)
                            {
                                ddlListModerators.Items.Add(new ListItem(account.Name));
                                ddlListToAddress.Items.Add(new ListItem(account.Name));
                                ddlListFromAddress.Items.Add(new ListItem(account.Name));
                                ddlListReplyToAddress.Items.Add(new ListItem(account.Name));
                            }
                        }
                    }

                Utils.SelectListItem(ddlListModerators, selectedModerator);
                Utils.SelectListItem(ddlListToAddress, selectedListToAddress);
                Utils.SelectListItem(ddlListFromAddress, selectedListFromAddress);
                Utils.SelectListItem(ddlListReplyToAddress, selectedListReplyToAddress);

            }
            else
            {

                MailAccount[] moderators = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
                ddlListModerators.Items.Clear();
                ddlListModerators.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectModerator"), ""));

                if (moderators != null)
                    foreach (MailAccount account in moderators)
                    {
                        if (ddlListModerators != null)
                        {
                            ddlListModerators.Items.Add(new ListItem(account.Name));
                            ddlListToAddress.Items.Add(new ListItem(account.Name));
                            ddlListFromAddress.Items.Add(new ListItem(account.Name));
                            ddlListReplyToAddress.Items.Add(new ListItem(account.Name));
                        }
                    }

                Utils.SelectListItem(ddlListModerators, selectedModerator);
                Utils.SelectListItem(ddlListToAddress, selectedListToAddress);
                Utils.SelectListItem(ddlListFromAddress, selectedListFromAddress);
                Utils.SelectListItem(ddlListReplyToAddress, selectedListReplyToAddress);
            }
        }

        private string GetDomainName(string email)
        {
            return email.Substring(email.IndexOf("@") + 1);
        }

        protected void ctxValDomain_EvaluatingContext(object sender, DesktopValidationEventArgs e)
        {
            if (Parent != null) ctrl = (MailEditAddress)Parent.Parent.FindControl("emailAddress");

            string moderator = ddlListModerators.SelectedValue;

            if (ctrl != null)
            {
                if (String.Equals(GetDomainName(moderator), GetDomainName(ctrl.Email), StringComparison.InvariantCultureIgnoreCase))
                {
                    e.ContextIsValid = true;
                    return;
                }
            }
            e.ContextIsValid = false;
        }

    }

}

