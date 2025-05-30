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

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class Organizations_Settings : WebsitePanelControlBase, IHostingServiceProviderSettings
    {
        public const string RootOU = "RootOU";
        public const string PrimaryDomainController = "PrimaryDomainController";
        public const string TemporyDomainName = "TempDomain";
        public const string UserNameFormat = "UserNameFormat";
        public const string ArchiveStoragePath = "ArchiveStoragePath";
        public const string UseStorageSpaces = "UseStorageSpaces";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }      

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtPrimaryDomainController.Text = settings[PrimaryDomainController];  
            txtRootOU.Text = settings[RootOU];
            txtTemporyDomainName.Text = settings[TemporyDomainName];

            if (settings.ContainsKey(UserNameFormat))
            {
                UserNameFormatDropDown.SelectedValue =
                    UserNameFormatDropDown.Items.FindByText(settings[UserNameFormat]).Value;
            }

            txtArchiveStorageSpace.Text = settings[ArchiveStoragePath];
            chkUseStorageSpaces.Checked = Utils.ParseBool(settings[UseStorageSpaces], false);

            chkUseStorageSpaces_StateChanged(null, null);
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[RootOU] = txtRootOU.Text.Trim();
            settings[PrimaryDomainController] = txtPrimaryDomainController.Text.Trim();
            settings[TemporyDomainName] = txtTemporyDomainName.Text.Trim();
            settings[UserNameFormat] = UserNameFormatDropDown.SelectedItem.Text;
            settings[ArchiveStoragePath] = txtArchiveStorageSpace.Text.Trim();
            settings[UseStorageSpaces] = chkUseStorageSpaces.Checked.ToString();
        }

        protected void chkUseStorageSpaces_StateChanged(object sender, EventArgs e)
        {
            txtArchiveStorageSpace.Visible = !chkUseStorageSpaces.Checked;
        }
    }
}
