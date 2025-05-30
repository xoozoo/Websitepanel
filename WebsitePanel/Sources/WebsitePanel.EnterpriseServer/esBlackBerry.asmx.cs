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

﻿using System.Web.Services;
using WebsitePanel.EnterpriseServer.Code.HostedSolution;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ResultObjects;
using Microsoft.Web.Services3;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esBlackBerry
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    public class esBlackBerry : WebService
    {

        [WebMethod]
        public ResultObject CreateBlackBerryUser(int itemId, int accountId)
        {
            return BlackBerryController.CreateBlackBerryUser(itemId, accountId);
        }

        [WebMethod]
        public ResultObject DeleteBlackBerryUser(int itemId, int accountId)
        {
            return BlackBerryController.DeleteBlackBerryUser(itemId, accountId);
        }

        [WebMethod]
        public OrganizationUsersPagedResult GetBlackBerryUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return BlackBerryController.GetBlackBerryUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        [WebMethod]
        public IntResult GetBlackBerryUserCount(int itemId, string name, string email)
        {
            return BlackBerryController.GetBlackBerryUsersCount(itemId, name, email);
        }

        [WebMethod]
        public BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId)
        {                        
            return BlackBerryController.GetBlackBerryUserStats(itemId, accountId);
        }


        [WebMethod]
        public ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId)
        {
            return BlackBerryController.DeleteDataFromBlackBerryDevice(itemId, accountId);
        }

        [WebMethod]
        public ResultObject SetEmailActivationPassword(int itemId, int accountId)
        {
            return BlackBerryController.SetEmailActivationPassword(itemId, accountId);
        }


        [WebMethod]
        public ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time)
        {
            return BlackBerryController.SetActivationPasswordWithExpirationTime(itemId, accountId, password, time);
        }


    }
}
