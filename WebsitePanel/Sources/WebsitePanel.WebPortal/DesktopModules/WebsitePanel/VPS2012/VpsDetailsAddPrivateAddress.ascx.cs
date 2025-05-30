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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal.VPS2012
{
    public partial class VpsDetailsAddPrivateAddress : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControls();
            }

            ToggleControls();
        }

        private void BindControls()
        {
            // load adapter details
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetPrivateNetworkAdapterDetails(PanelRequest.ItemID);

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER))
            {
                // set max number
                QuotaValueInfo privQuota = cntx.Quotas[Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER];
                int maxPrivate = privQuota.QuotaAllocatedValue;
                if (maxPrivate == -1)
                    maxPrivate = 10;

                maxPrivate -= nic.IPAddresses.Length;

                txtPrivateAddressesNumber.Text = maxPrivate.ToString();
                litMaxPrivateAddresses.Text = String.Format(GetLocalizedString("litMaxPrivateAddresses.Text"), maxPrivate);
                btnAdd.Enabled = maxPrivate > 0;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int number = Utils.ParseInt(txtPrivateAddressesNumber.Text.Trim(), 0);
            string[] privIps = Utils.ParseDelimitedString(txtPrivateAddressesList.Text, '\n', '\r', ' ', '\t');

            try
            {
                ResultObject res = ES.Services.VPS2012.AddVirtualMachinePrivateIPAddresses(PanelRequest.ItemID,
                    radioPrivateRandom.Checked, number, privIps);

                if (res.IsSuccess)
                {
                    RedirectBack();
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_ADDING_IP_ADDRESS", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_ADDING_IP_ADDRESS", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_network",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        private void ToggleControls()
        {
            // private network
            PrivateAddressesNumberRow.Visible = radioPrivateRandom.Checked;
            PrivateAddressesListRow.Visible = radioPrivateSelected.Checked;
        }
    }
}
