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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class Common_IPAddressesList : WebsitePanelControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            string sips = settings["ListeningIPAddresses"];
            if (sips != null)
            {
                string[] ips = sips.Split(',');


                foreach (string ip in ips)
                {
                    ListItem li = CreateIPListItem(Utils.ParseInt(ip, 0));
                    if (li != null)
                        lbAddresses.Items.Add(li);
                }
            }
        }

        public void SaveSettings(StringDictionary settings)
        {
            string[] ips = new string[lbAddresses.Items.Count];
            for (int i = 0; i < lbAddresses.Items.Count; i++)
                ips[i] = lbAddresses.Items[i].Value;

            settings["ListeningIPAddresses"] = String.Join(",", ips);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbAddresses.SelectedIndex != -1)
            {
                lbAddresses.Items.RemoveAt(lbAddresses.SelectedIndex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // check if the item is already added
            int addressId = ipAddress.AddressId;
            if (addressId == 0)
                return;

            foreach (ListItem li in lbAddresses.Items)
            {
                if (li.Value == addressId.ToString())
                    return;
            }

            lbAddresses.Items.Add(CreateIPListItem(addressId));
        }

        private ListItem CreateIPListItem(int addressId)
        {
            IPAddressInfo addr = ES.Services.Servers.GetIPAddress(addressId);
            if (addr != null)
            {
                string fullIP = addr.ExternalIP;
                if (addr.InternalIP != null &&
                    addr.InternalIP != "" &&
                    addr.InternalIP != addr.ExternalIP)
                    fullIP += " (" + addr.InternalIP + ")";

                return new ListItem(fullIP, addr.AddressId.ToString());
            }
            return null;
        }
    }
}
