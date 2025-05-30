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
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data;
using WebsitePanel.Portal;

namespace WebsitePanel.WebPortal
{
    public class WebsitePanelAjaxHandler : IHttpHandler
    {

        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            String fullType = context.Request.Params["fullType"];
            if (fullType == "TableSearch")
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string PagedStored = context.Request.Params["PagedStored"];
                string FilterValue = context.Request.Params["FilterValue"];
                string strMaximumRows = context.Request.Params["MaximumRows"];
                int MaximumRows = strMaximumRows != null ? Int32.Parse(strMaximumRows) : 15;
                string strRecursive = context.Request.Params["Recursive"];
                bool Recursive = strRecursive != null ? serializer.Deserialize<Boolean>(strRecursive) : false;
                string strPoolID = context.Request.Params["PoolID"];
                int PoolID = strPoolID != null ? Int32.Parse(strPoolID) : 0;
                string strServerID = context.Request.Params["ServerID"];
                int ServerID = !String.IsNullOrEmpty(strServerID) ? Int32.Parse(strServerID) : 0;
                string strStatusID = context.Request.Params["StatusID"];
                int StatusID = !String.IsNullOrEmpty(strStatusID) ? Int32.Parse(strStatusID) : 0;
                string strPlanID = context.Request.Params["PlanID"];
                int PlanID = !String.IsNullOrEmpty(strPlanID) ? Int32.Parse(strPlanID) : 0;
                string strOrgID = context.Request.Params["OrgID"];
                int OrgID = !String.IsNullOrEmpty(strOrgID) ? Int32.Parse(strOrgID) : 0;
                string ItemTypeName = context.Request.Params["ItemTypeName"];
                string GroupName = context.Request.Params["GroupName"];
                string strPackageID = context.Request.Params["PackageID"];
                int PackageID = !String.IsNullOrEmpty(strPackageID) ? Int32.Parse(strPackageID) : -1;
                string VPSType = context.Request.Params["VPSType"];
                string strRoleID = context.Request.Params["RoleID"];
                int RoleID = !String.IsNullOrEmpty(strRoleID) ? Int32.Parse(strRoleID) : 0;
                string strUserID = context.Request.Params["UserID"];
                int UserID = !String.IsNullOrEmpty(strUserID) ? Int32.Parse(strUserID) : 0;
                string FilterColumns = context.Request.Params["FilterColumns"];

                string RedirectUrl = context.Request.Params["RedirectUrl"];

                DataSet dsObjectItems = ES.Services.Packages.GetSearchTableByColumns(PagedStored, 
                    String.Format("%{0}%", FilterValue), MaximumRows, Recursive, PoolID, ServerID,
                    StatusID, PlanID, OrgID, ItemTypeName, GroupName, PackageID, VPSType, RoleID, UserID,
                    FilterColumns);

                DataTable dt = dsObjectItems.Tables[0];
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                string Redirect = context.Request.Params["Redirect"];
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    DataRow row = dt.Rows[i];
                    Dictionary<string, string> obj = new Dictionary<string, string>();
                    if (!String.IsNullOrEmpty(RedirectUrl) && ((int)row["Count"] == 1))
                        obj["url"] = String.Format(RedirectUrl, row["ItemID"].ToString());
                    obj["TextSearch"] = row["TextSearch"].ToString();
                    obj["ColumnType"] = row["ColumnType"].ToString();
                    dataList.Add(obj);
                }

                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(dataList);
                context.Response.ContentType = "text/plain";
                context.Response.Write(json);
                return;
            }

            String filterValue = context.Request.Params["term"];
            String columnType = context.Request.Params["columnType"];
            String numResults = context.Request.Params["itemCount"];
            int iNumResults = 15;
            if ((numResults != null) && (numResults.Length > 0))
            {
                int num = Int32.Parse(numResults);
                if (num > 0)
                    iNumResults = num;
            }

            if (fullType == "Spaces")
            {
                String strItemType = context.Request.Params["itemType"];
                int itemType = Int32.Parse(strItemType);
                DataSet dsObjectItems = ES.Services.Packages.SearchServiceItemsPaged(PanelSecurity.EffectiveUserId, itemType,
                    String.Format("%{0}%", filterValue),
                   "", 0, iNumResults);
                DataTable dt = dsObjectItems.Tables[1];
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    DataRow row = dt.Rows[i];
                    Dictionary<string, string> obj = new Dictionary<string, string>();
                    obj["ColumnType"] = "PackageName";
                    obj["TextSearch"] = row["PackageName"].ToString();
                    obj["ItemID"] = row["ItemID"].ToString();
                    obj["PackageID"] = row["PackageID"].ToString();
                    obj["FullType"] = "Space";
                    obj["FullTypeLocalized"] = GetTypeDisplayName("Space");
                    obj["AccountID"] = row["AccountID"].ToString();
                    dataList.Add(obj);
                }

                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(dataList);
                context.Response.ContentType = "text/plain";
                context.Response.Write(json);
            }
            else
            {
                DataSet dsObjectItems = ES.Services.Packages.GetSearchObjectQuickFind(PanelSecurity.EffectiveUserId, null,
                    String.Format("%{0}%", filterValue), 0, 0, "", iNumResults, columnType, fullType);
                DataTable dt = dsObjectItems.Tables[2];
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    DataRow row = dt.Rows[i];
                    string type = row["FullType"].ToString();
                    Dictionary<string, string> obj = new Dictionary<string, string>();
                    obj["ColumnType"] = row["ColumnType"].ToString();
                    obj["TextSearch"] = row["TextSearch"].ToString();
                    obj["ItemID"] = row["ItemID"].ToString();
                    obj["PackageID"] = row["PackageID"].ToString();
                    obj["FullType"] = type;
                    obj["FullTypeLocalized"] = GetTypeDisplayName(type);
                    obj["AccountID"] = row["AccountID"].ToString();
                    dataList.Add(obj);
                }

                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(dataList);
                context.Response.ContentType = "text/plain";
                context.Response.Write(json);
            }
        }

        protected const string ModuleName = "WebsitePanel";

        protected string GetTypeDisplayName(string type)
        {
            return PortalUtils.GetSharedLocalizedString(ModuleName, "ServiceItemType." + type)
                ?? PortalUtils.GetSharedLocalizedString(ModuleName, "UserItemType." + type)
                ?? type;
        }
    }
};
