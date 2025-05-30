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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebsitePanel.Portal.ReportingServices
{
	/// <summary>
	/// This class is used to load resource string related to current module.
	/// </summary>
	public class WebsitePanelModuleResourceStorage : IResourceStorage
	{
		/// <summary>
		/// The module (control) currently being displayed.
		/// </summary>
		private WebsitePanelModuleBase module;

		/// <summary>
		/// Cunstructs the instance.
		/// </summary>
		/// <param name="module">Module containing report viewer component.</param>
		/// <exception cref="ArgumentNullException">Whem <paramref name="module"/> is null.</exception>
		public WebsitePanelModuleResourceStorage(WebsitePanelModuleBase module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}

			this.module = module;
		}

		#region IResourceStorage Members
		/// <summary>
		/// Returns string located in module resource file.
		/// </summary>
		/// <param name="resourceKey">The key, which is used to load string.</param>
		/// <returns>String stored in module resource file.</returns>
		public string GetString(string resourceKey)
		{
			return this.module.GetLocalizedString(resourceKey);
		}

		/// <summary>
		/// Returns shared string located in the global module file.
		/// </summary>
		/// <param name="resourceKey">Key, which will be used to find string in resource file.</param>
		/// <returns>String stored in shared (global) resource file.</returns>
		public string GetSharedString(string resourceKey)
		{
			return this.module.GetSharedLocalizedString(resourceKey);
		}
		#endregion
	}
}
