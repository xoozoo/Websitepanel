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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    /// <summary>
    /// Summary description for PresentationServices
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/webportal")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.Web.Script.Services.ScriptService]
    public class TaskManager : System.Web.Services.WebService
    {
        [WebMethod]
        public BackgroundTask GetTask(string taskId)
        {
            BackgroundTask task = ES.Services.Tasks.GetTask(taskId);
            return task;
        }

        [WebMethod]
        public BackgroundTask GetTaskWithLogRecords(string taskId, DateTime startLogTime)
        {
            return ES.Services.Tasks.GetTaskWithLogRecords(taskId, startLogTime);
        }

        [WebMethod]
        public int GetTasksNumber()
        {
            return ES.Services.Tasks.GetTasksNumber();
        }

        [WebMethod]
        public BackgroundTask[] GetUserTasks(int userId)
        {
            return ES.Services.Tasks.GetUserTasks(userId);
        }

        [WebMethod]
        public BackgroundTask[] GetCompletedTasks()
        {
            return ES.Services.Tasks.GetUserCompletedTasks(PanelSecurity.LoggedUserId);
        }

        [WebMethod]
        public void SetTaskNotifyOnComplete(string taskId)
        {
            ES.Services.Tasks.SetTaskNotifyOnComplete(taskId);
        }
    }
}
