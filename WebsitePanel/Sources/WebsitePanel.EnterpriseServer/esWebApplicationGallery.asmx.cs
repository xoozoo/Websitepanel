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
using System.Web.Services;
using WebsitePanel.Providers.WebAppGallery;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esWebApplicationGallery
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class esWebApplicationGallery : System.Web.Services.WebService
    {
        [WebMethod]
        public void InitFeeds(int packageId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
        }

        [WebMethod]
        public void SetResourceLanguage(int packageId, string resourceLanguage)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
            WebAppGalleryController.SetResourceLanguage(packageId, resourceLanguage);
        }
        

        [WebMethod]
        public GalleryLanguagesResult GetGalleryLanguages(int packageId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
            return WebAppGalleryController.GetGalleryLanguages(packageId);
        }


		[WebMethod]
		public GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId)
		{
            WebAppGalleryController.InitFeedsByServiceId(SecurityContext.User.UserId, serviceId);
			return WebAppGalleryController.GetGalleryApplicationsByServiceId(serviceId);
		}

        [WebMethod]
        public GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
            return WebAppGalleryController.GetGalleryApplications(packageId, categoryId);
        }

        [WebMethod]
        public GalleryApplicationsResult GetInstaledApplications(int packageId, string categoryId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
            return WebAppGalleryController.GetGalleryApplications(packageId, categoryId);
        }


        [WebMethod]
        public GalleryApplicationsResult GetGalleryApplicationsFiltered(int packageId, string pattern)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
            return WebAppGalleryController.GetGalleryApplicationsFiltered(packageId, pattern);
        }

        [WebMethod]
        public GalleryCategoriesResult GetGalleryCategories(int packageId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
			return WebAppGalleryController.GetGalleryCategories(packageId);
        }

        [WebMethod]
        public GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
			return WebAppGalleryController.GetGalleryApplicationDetails(packageId, applicationId);
        }

        [WebMethod]
		public DeploymentParametersResult GetGalleryApplicationParams(int packageId, string applicationId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
			return WebAppGalleryController.GetGalleryApplicationParams(packageId, applicationId);
        }

        [WebMethod]
        public StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, List<DeploymentParameter> parameters, string languageId)
        {
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
            return WebAppGalleryController.Install(packageId, webAppId, siteName, virtualDir, parameters, languageId);
        }

		[WebMethod(Description="Returns Web Application Gallery application status, such as Downloaded, Downloading, Failed or NotDownloaded. Throws an ApplicationException if WAG module is not available on the target server.")]
		public GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId)
		{
            WebAppGalleryController.InitFeeds(SecurityContext.User.UserId, packageId);
			return WebAppGalleryController.GetGalleryApplicationStatus(packageId, webAppId);
		}
    }
}
