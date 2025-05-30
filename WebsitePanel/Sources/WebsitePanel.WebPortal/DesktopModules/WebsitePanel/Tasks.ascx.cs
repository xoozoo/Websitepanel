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
    public partial class Tasks : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void odsTasks_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                e.ExceptionHandled = true;
            }
        }

        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // get data item
            BackgroundTask task = (BackgroundTask)e.Row.DataItem;
            if (task == null)
                return;

            // find controls
            HyperLink lnkTaskName = (HyperLink)e.Row.FindControl("lnkTaskName");
            Literal litTaskDuration = (Literal)e.Row.FindControl("litTaskDuration");
            Panel pnlProgressIndicator = (Panel)e.Row.FindControl("pnlProgressIndicator");
            LinkButton cmdStop = (LinkButton)e.Row.FindControl("cmdStop");

            // bind controls
            lnkTaskName.Text = GetAuditLogTaskName(task.Source, task.TaskName);
            lnkTaskName.NavigateUrl = EditUrl("TaskID", task.TaskId, "view_details");

            // duration
            TimeSpan duration = (TimeSpan)(DateTime.Now - task.StartDate);
            litTaskDuration.Text = String.Format("{0}:{1}:{2}",
                duration.Hours.ToString().PadLeft(2, '0'),
                duration.Minutes.ToString().PadLeft(2, '0'),
                duration.Seconds.ToString().PadLeft(2, '0'));

            // progress
            int percent = 0;
            if (task.IndicatorMaximum > 0)
                percent = task.IndicatorCurrent * 100 / task.IndicatorMaximum;
            pnlProgressIndicator.Width = Unit.Percentage(percent);

            // stop button
            cmdStop.CommandArgument = task.TaskId;
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "stop")
            {
                // stop task
                ES.Services.Tasks.StopTask(e.CommandArgument.ToString());

                // rebind grid
                gvTasks.DataBind();
            }
        }
    }
}
