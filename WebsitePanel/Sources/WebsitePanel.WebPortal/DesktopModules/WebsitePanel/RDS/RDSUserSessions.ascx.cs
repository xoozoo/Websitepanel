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

using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.RemoteDesktopServices;

namespace WebsitePanel.Portal.RDS
{
    public partial class RDSUserSessions : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            buttonPanel.ButtonSaveVisible = false;            
      
            if (!IsPostBack)
            {
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                litCollectionName.Text = collection.DisplayName;
                BindGrid();
            }
        }

        protected void gvRDSCollections_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LogOff")
            {
                var arguments = e.CommandArgument.ToString().Split(';');
                string unifiedSessionId = arguments[0];
                string hostServer = arguments[1];

                try
                {
                    ES.Services.RDS.LogOffRdsUser(PanelRequest.ItemID, unifiedSessionId, hostServer);                    
                    BindGrid();
                    ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("REMOTE_DESKTOP_SERVICES_LOG_OFF_USER", ex);
                }
            }
            else if (e.CommandName == "View")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    ES.Services.RDS.ShadowSession(PanelRequest.ItemID, args[0], false, args[1]);
                    ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("REMOTE_DESKTOP_SERVICES_VIEW_SESSION", ex);
                }
            }
            else if (e.CommandName == "Control")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    ES.Services.RDS.ShadowSession(PanelRequest.ItemID, args[0], true, args[1]);
                    ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("REMOTE_DESKTOP_SERVICES_CONTROL_SESSION", ex);
                }
            }
            else if (e.CommandName == "SendMessage")
            {
                ViewState["SendMessageUsers"] = e.CommandArgument;
                ShowMessageEditor();
            }
        }

        protected void btnAddMessage_Click(object sender, EventArgs e)
        {            
            string[] sendMessageInfo = ViewState["SendMessageUsers"].ToString().Split(':');
            string serverName = sendMessageInfo[0];
            string userName = sendMessageInfo[1];
            string sessionId = sendMessageInfo[2];
            List<RdsMessageRecipient> recipients = new List<RdsMessageRecipient>();

            if (userName != "ALL")
            {
                recipients.Add(new RdsMessageRecipient
                {
                    ComputerName = serverName,
                    SessionId = sessionId
                });

                ES.Services.RDS.SendMessage(recipients.ToArray(), txtMessage.Text, PanelRequest.ItemID, PanelRequest.CollectionID, userName);
            }
            else
            {
                var userSessions = ES.Services.RDS.GetRdsUserSessions(PanelRequest.CollectionID).ToList();

                foreach(var userSession in userSessions)
                {
                    recipients.Add(new RdsMessageRecipient
                    {
                        ComputerName = userSession.HostServer,
                        SessionId = userSession.UnifiedSessionId
                    });
                }

                ES.Services.RDS.SendMessage(recipients.ToArray(), txtMessage.Text, PanelRequest.ItemID, PanelRequest.CollectionID, "ALL");
            }                       
        }

        protected void btnRecentMessages_Click(object sender, EventArgs e)
        {
            var recentMessages = ES.Services.RDS.GetRdsMessagesByCollectionId(PanelRequest.CollectionID);
            gvMessagesHistory.DataSource = recentMessages;
            gvMessagesHistory.DataBind();
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
            MessagesHistoryModal.Show();
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            ViewState["SendMessageUsers"] = string.Format(":ALL:");
            ShowMessageEditor();
        }

        private void ShowMessageEditor()
        {
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
            EnterMessageModal.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            BindGrid();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            BindGrid();
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
        }

        private void BindGrid()
        {
            var userSessions = new List<RdsUserSession>();

            try
            {
                userSessions = ES.Services.RDS.GetRdsUserSessions(PanelRequest.CollectionID).ToList();
            }
            catch(Exception ex)
            {
                ShowErrorMessage("REMOTE_DESKTOP_SERVICES_USER_SESSIONS", ex);
            }

            foreach(var userSession in userSessions)
            {
                var states = userSession.SessionState.Split('_');

                if (states.Length == 2)
                {
                    userSession.SessionState = states[1];
                }
            }

            gvRDSUserSessions.DataSource = userSessions;
            gvRDSUserSessions.DataBind();
        }

        public string GetAccountImage(bool vip)
        {
            if (vip)
            {
                return GetThemedImage("Exchange/vip_user_16.png");
            }

            return GetThemedImage("Exchange/accounting_mail_16.png");
        }
    }
}
