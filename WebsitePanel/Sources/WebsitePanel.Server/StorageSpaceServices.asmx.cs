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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.Web.Services3;
using WebsitePanel.Providers;
using WebsitePanel.Providers.EnterpriseStorage;
using WebsitePanel.Providers.OS;
using WebsitePanel.Providers.StorageSpaces;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for StorageSpace
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class StorageSpaceServices : HostingServiceProviderWebService, IStorageSpace
    {
        private IStorageSpace StorageSpaceProvider
        {
            get { return (IStorageSpace)Provider; }
        }

        [WebMethod, SoapHeader("settings")]
        public List<SystemFile> GetAllDriveLetters()
        {
            try
            {
                Log.WriteStart("'{0}' GetAllDriveLetters", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.GetAllDriveLetters();
                Log.WriteEnd("'{0}' GetAllDriveLetters", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAllDriveLetters", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<SystemFile> GetSystemSubFolders(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetSystemFolders", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.GetSystemSubFolders(path);
                Log.WriteEnd("'{0}' GetSystemFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSystemFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateStorageSettings", ProviderSettings.ProviderName);
                StorageSpaceProvider.UpdateStorageSettings(fullPath, qouteSizeBytes, type);
                Log.WriteEnd("'{0}' UpdateStorageSettings", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateStorageSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ClearStorageSettings(string fullPath, string uncPath)
        {
            try
            {
                Log.WriteStart("'{0}' ClearStorageSettings", ProviderSettings.ProviderName);
                StorageSpaceProvider.ClearStorageSettings(fullPath, uncPath);
                Log.WriteEnd("'{0}' ClearStorageSettings", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ClearStorageSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateFolderQuota", ProviderSettings.ProviderName);
                StorageSpaceProvider.UpdateFolderQuota(fullPath, qouteSizeBytes, type);
                Log.WriteEnd("'{0}' UpdateFolderQuota", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateFolderQuota", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateFolder(string fullPath)
        {
            try
            {
                Log.WriteStart("'{0}' CreateFolder", ProviderSettings.ProviderName);
                StorageSpaceProvider.CreateFolder(fullPath);
                Log.WriteEnd("'{0}' CreateFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public StorageSpaceFolderShare ShareFolder(string fullPath, string shareName)
        {
            try
            {
                Log.WriteStart("'{0}' ShareFolder", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.ShareFolder(fullPath, shareName);
                Log.WriteEnd("'{0}' ShareFolder", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShareFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Quota GetFolderQuota(string fullPath)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolderQuota", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.GetFolderQuota(fullPath);
                Log.WriteEnd("'{0}' GetFolderQuota", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolderQuota", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFolder(string fullPath)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFolder", ProviderSettings.ProviderName);
                StorageSpaceProvider.DeleteFolder(fullPath);
                Log.WriteEnd("'{0}' DeleteFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool RenameFolder(string originalPath, string newName)
        {
            try
            {
                Log.WriteStart("'{0}' RenameFolder", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.RenameFolder(originalPath, newName);
                Log.WriteEnd("'{0}' RenameFolder", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool FileOrDirectoryExist(string fullPath)
        {
            try
            {
                Log.WriteStart("'{0}' FileOrDirectoryExist", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.FileOrDirectoryExist(fullPath);
                Log.WriteEnd("'{0}' FileOrDirectoryExist", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' FileOrDirectoryExist", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            try
            {
                Log.WriteStart("'{0}' SetFolderNtfsPermissions", ProviderSettings.ProviderName);
                StorageSpaceProvider.SetFolderNtfsPermissions(fullPath, permissions, isProtected, preserveInheritance);
                Log.WriteEnd("'{0}' SetFolderNtfsPermissions", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetFolderNtfsPermissions", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] Search(string[] searchPaths, string searchText, bool recursive)
        {
            try
            {
                Log.WriteStart("'{0}' Search", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.Search(searchPaths, searchText, recursive);
                Log.WriteEnd("'{0}' Search", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' Search", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            try
            {
                Log.WriteStart("'{0}' GetFileBinaryChunk", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.GetFileBinaryChunk(path, offset, length);
                Log.WriteEnd("'{0}' GetFileBinaryChunk", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFileBinaryChunk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RemoveShare(string fullPath)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveShare", ProviderSettings.ProviderName);
                StorageSpaceProvider.RemoveShare(fullPath);
                Log.WriteEnd("'{0}' RemoveShare", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveShare", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ShareSetAbeState(string path, bool enabled)
        {
            try
            {
                Log.WriteStart("'{0}' ShareSetAbeState", ProviderSettings.ProviderName);
                StorageSpaceProvider.ShareSetAbeState(path, enabled);
                Log.WriteEnd("'{0}' ShareSetAbeState", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShareSetAbeState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ShareSetEncyptDataAccess(string path, bool enabled)
        {
            try
            {
                Log.WriteStart("'{0}' ShareSetEncyptDataAccess", ProviderSettings.ProviderName);
                StorageSpaceProvider.ShareSetEncyptDataAccess(path, enabled);
                Log.WriteEnd("'{0}' ShareSetEncyptDataAccess", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShareSetEncyptDataAccess", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public bool ShareGetEncyptDataAccessStatus(string path)
        {
            try
            {
                Log.WriteStart("'{0}' ShareGetEncyptDataAccessStatus", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.ShareGetEncyptDataAccessStatus(path);
                Log.WriteEnd("'{0}' ShareGetEncyptDataAccessStatus", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShareGetEncyptDataAccessStatus", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool ShareGetAbeState(string path)
        {
            try
            {
                Log.WriteStart("'{0}' ShareGetAbeState", ProviderSettings.ProviderName);
                var result = StorageSpaceProvider.ShareGetAbeState(path);
                Log.WriteEnd("'{0}' ShareGetAbeState", ProviderSettings.ProviderName);

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShareGetAbeState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
    }
}
