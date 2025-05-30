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
using System.Web.UI.WebControls;

namespace WebsitePanel.Portal.Lync.UserControls
{
    public partial class LyncUserPlanSelector : WebsitePanelControlBase
    {

        private string planToSelect;

        public string planId
        {
                        
            get {
                if (ddlPlan.Items.Count == 0) return "";
                return ddlPlan.SelectedItem.Value; 
            }
            set
            {
                planToSelect = value;
                foreach(ListItem li in ddlPlan.Items)
                {
                    if (li.Value == value)
                    {
                        ddlPlan.ClearSelection();
                        li.Selected = true;
                        break;
                    }
                }
            }
        }

        public int plansCount
		{
			get
			{
                return this.ddlPlan.Items.Count;
			}
		}


        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
                BindPlans();
			}
        }

        public WebsitePanel.Providers.HostedSolution.LyncUserPlan plan
        {
            get
            {
                WebsitePanel.Providers.HostedSolution.LyncUserPlan[] plans = ES.Services.Lync.GetLyncUserPlans(PanelRequest.ItemID);
                foreach (WebsitePanel.Providers.HostedSolution.LyncUserPlan planitem in plans)
                {
                    if (planitem.LyncUserPlanId.ToString() == planId) return planitem;
                }
                return null;
            }
        }

        private void BindPlans()
		{
            WebsitePanel.Providers.HostedSolution.LyncUserPlan[] plans = ES.Services.Lync.GetLyncUserPlans(PanelRequest.ItemID);

            foreach (WebsitePanel.Providers.HostedSolution.LyncUserPlan plan in plans)
			{
				ListItem li = new ListItem();
                li.Text = plan.LyncUserPlanName;
                li.Value = plan.LyncUserPlanId.ToString();
                li.Selected = plan.IsDefault;
                ddlPlan.Items.Add(li);
			}

            foreach (ListItem li in ddlPlan.Items)
            {
                if (li.Value == planToSelect)
                {
                    ddlPlan.ClearSelection();
                    li.Selected = true;
                    break;
                }
            }

		}
    }
}
