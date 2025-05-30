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
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WebsitePanel.EnterpriseServer.Base.HostedSolution;
using WebsitePanel.Providers;
using WebsitePanel.Providers.DNS;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.SharePoint;

namespace WebsitePanel.EnterpriseServer.Code.SharePoint
{
    /// <summary>
    /// Exposes handful API on hosted SharePoint site collections management.
    /// </summary>
    public class HostedSharePointServerController : IImportController, IBackupController
    {
        private const int FILE_BUFFER_LENGTH = 5000000; // ~5MB

        /// <summary>
        /// Gets site collections in raw form.
        /// </summary>
        /// <param name="packageId">Package to which desired site collections belong.</param>
        /// <param name="organizationId">Organization to which desired site collections belong.</param>
        /// <param name="filterColumn">Filter column name.</param>
        /// <param name="filterValue">Filter value.</param>
        /// <param name="sortColumn">Sort column name.</param>
        /// <param name="startRow">Row index to start from.</param>
        /// <param name="maximumRows">Maximum number of rows to retrieve.</param>
        /// <returns>Site collections that match.</returns>
        public static SharePointSiteCollectionListPaged GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            if (IsDemoMode)
            {
                SharePointSiteCollectionListPaged demoResult = new SharePointSiteCollectionListPaged();
                demoResult.SiteCollections = GetSiteCollections(1, false);
                demoResult.TotalRowCount = demoResult.SiteCollections.Count;
                return demoResult;
            }

            SharePointSiteCollectionListPaged paged = new SharePointSiteCollectionListPaged();
            DataSet result = PackageController.GetRawPackageItemsPaged(packageId,  ResourceGroups.SharepointFoundationServer, typeof(SharePointSiteCollection),
                true, filterColumn, filterValue, sortColumn, startRow, Int32.MaxValue);
            List<SharePointSiteCollection> items = PackageController.CreateServiceItemsList(result, 1).ConvertAll<SharePointSiteCollection>(delegate(ServiceProviderItem item) { return (SharePointSiteCollection)item; });

            if (organizationId > 0)
            {
                items = items.FindAll(delegate(SharePointSiteCollection siteCollection) { return siteCollection.OrganizationId == organizationId; });
            }
            paged.TotalRowCount = items.Count;

            if (items.Count > maximumRows)
            {
                items.RemoveRange(maximumRows, items.Count - maximumRows);
            }

            paged.SiteCollections = items;

            return paged;
        }

        public static List<SharePointSiteCollection> GetSiteCollections(int organizationId)
        {
            Organization org = OrganizationController.GetOrganization(organizationId);

            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(org.PackageId, typeof(SharePointSiteCollection), false);
            items.ConvertAll<SharePointSiteCollection>(delegate(ServiceProviderItem item) { return (SharePointSiteCollection)item; });
            List<SharePointSiteCollection> ret = new List<SharePointSiteCollection>();
            foreach (ServiceProviderItem item in items)
            {
                SharePointSiteCollection siteCollection = item as SharePointSiteCollection;
                if (siteCollection != null && siteCollection.OrganizationId == organizationId)
                {
                    ret.Add(siteCollection);
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets list of supported languages by this installation of SharePoint.
        /// </summary>
        /// <returns>List of supported languages</returns>
        public static int[] GetSupportedLanguages(int packageId)
        {
            if (IsDemoMode)
            {
                return new int[] { 1033 };
            }

            // Log operation.
            TaskManager.StartTask("HOSTEDSHAREPOINT", "GET_LANGUAGES");

            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.SharepointFoundationServer);
            if (serviceId == 0)
            {
                return new int[] { };
            }

            try
            {
                // Create site collection on server.
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);
                return hostedSharePointServer.GetSupportedLanguages();
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        ///  Gets list of SharePoint site collections that belong to the package.
        /// </summary>
        /// <param name="packageId">Package that owns site collections.</param>
        /// <param name="recursive">A value which shows whether nested spaces must be searched as well.</param>
        /// <returns>List of found site collections.</returns>
        public static List<SharePointSiteCollection> GetSiteCollections(int packageId, bool recursive)
        {
            if (IsDemoMode)
            {
                List<SharePointSiteCollection> demoResult = new List<SharePointSiteCollection>();
                SharePointSiteCollection siteCollection1 = new SharePointSiteCollection();
                siteCollection1.Id = 1;
                siteCollection1.OrganizationId = 1;
                siteCollection1.LocaleId = 1033;
                siteCollection1.Name = "http://john.fabrikam.com";
                siteCollection1.OwnerEmail = "john@fabrikam.com";
                siteCollection1.OwnerLogin = "john@fabrikam.com";
                siteCollection1.OwnerName = "John Smith";
                siteCollection1.PhysicalAddress = "http://john.fabrikam.com";
                siteCollection1.Title = "John Smith's Team Site";
                siteCollection1.Url = "http://john.fabrikam.com";
                demoResult.Add(siteCollection1);
                SharePointSiteCollection siteCollection2 = new SharePointSiteCollection();
                siteCollection2.Id = 2;
                siteCollection1.OrganizationId = 1;
                siteCollection2.LocaleId = 1033;
                siteCollection2.Name = "http://mark.contoso.com";
                siteCollection2.OwnerEmail = "mark@contoso.com";
                siteCollection2.OwnerLogin = "mark@contoso.com";
                siteCollection2.OwnerName = "Mark Jonsons";
                siteCollection2.PhysicalAddress = "http://mark.contoso.com";
                siteCollection2.Title = "Mark Jonsons' Blog";
                siteCollection2.Url = "http://mark.contoso.com";
                demoResult.Add(siteCollection2);
                return demoResult;
            }


            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(packageId, typeof(SharePointSiteCollection), recursive);
            return items.ConvertAll<SharePointSiteCollection>(delegate(ServiceProviderItem item) { return (SharePointSiteCollection)item; });
        }

        /// <summary>
        /// Gets SharePoint site collection with given id.
        /// </summary>
        /// <param name="itemId">Site collection id within metabase.</param>
        /// <returns>Site collection or null in case no such item exist.</returns>
        public static SharePointSiteCollection GetSiteCollection(int itemId)
        {
            if (IsDemoMode)
            {
                return GetSiteCollections(1, false)[itemId - 1];
            }

            SharePointSiteCollection item = PackageController.GetPackageItem(itemId) as SharePointSiteCollection;
            return item;
        }

        /// <summary>
        /// Adds SharePoint site collection.
        /// </summary>
        /// <param name="item">Site collection description.</param>
        /// <returns>Created site collection id within metabase.</returns>
        public static int AddSiteCollection(SharePointSiteCollection item)
        {

            // Check account.
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                return accountCheck;
            }

            // Check package.
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0)
            {
                return packageCheck;
            }

            // Check quota.
            OrganizationStatistics orgStats = OrganizationController.GetOrganizationStatisticsByOrganization(item.OrganizationId);
            //QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.HOSTED_SHAREPOINT_SITES);

            if (orgStats.AllocatedSharePointSiteCollections > -1
                && orgStats.CreatedSharePointSiteCollections >= orgStats.AllocatedSharePointSiteCollections)
            {
                return BusinessErrorCodes.ERROR_SHAREPOINT_RESOURCE_QUOTA_LIMIT;
            }

            // Check if stats resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.SharepointFoundationServer);

            if (serviceId == 0)
            {
                return BusinessErrorCodes.ERROR_SHAREPOINT_RESOURCE_UNAVAILABLE;
            }

            StringDictionary hostedSharePointSettings = ServerController.GetServiceSettings(serviceId);
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.HOSTED_SHAREPOINT_USESHAREDSSL);
            Uri rootWebApplicationUri = new Uri(hostedSharePointSettings["RootWebApplicationUri"]);
            Organization org = OrganizationController.GetOrganization(item.OrganizationId);
            string siteName = item.Name;

            if (quota.QuotaAllocatedValue == 1)
            {
                string sslRoot = hostedSharePointSettings["SharedSSLRoot"];


                string defaultDomain = org.DefaultDomain;
                string hostNameBase = string.Empty;

                string[] tmp = defaultDomain.Split('.');
                if (tmp.Length == 2)
                {
                    hostNameBase = tmp[0];
                }
                else
                {
                    if (tmp.Length > 2)
                    {
                        hostNameBase = tmp[0] + tmp[1];
                    }
                }

                int counter = 0;
                item.Name = String.Format("{0}://{1}", rootWebApplicationUri.Scheme, hostNameBase + "-" + counter.ToString() + "." + sslRoot);
                siteName = String.Format("{0}", hostNameBase + "-" + counter.ToString() + "." + sslRoot);                

                while  ( DataProvider. CheckServiceItemExists( serviceId,   item. Name,   "WebsitePanel.Providers.SharePoint.SharePointSiteCollection,   WebsitePanel.Providers.Base"))  
                {
                    counter++;
                    item.Name = String.Format("{0}://{1}", rootWebApplicationUri.Scheme, hostNameBase + "-" + counter.ToString() + "." + sslRoot);
                    siteName = String.Format("{0}", hostNameBase + "-" + counter.ToString() + "." + sslRoot);
                }
            }
            else
                item.Name = String.Format("{0}://{1}", rootWebApplicationUri.Scheme, item.Name);

            if (rootWebApplicationUri.Port > 0 && rootWebApplicationUri.Port != 80 && rootWebApplicationUri.Port != 443)
            {
                item.PhysicalAddress = String.Format("{0}:{1}", item.Name, rootWebApplicationUri.Port);
            }
            else
            {
                item.PhysicalAddress = item.Name;
            }

            if (Utils.ParseBool(hostedSharePointSettings["LocalHostFile"], false))
            {
                item.RootWebApplicationInteralIpAddress = hostedSharePointSettings["RootWebApplicationInteralIpAddress"];
                item.RootWebApplicationFQDN = item.Name.Replace(rootWebApplicationUri.Scheme + "://", "");
            }

            item.MaxSiteStorage = RecalculateMaxSize(org.MaxSharePointStorage, (int)item.MaxSiteStorage);
            item.WarningStorage = item.MaxSiteStorage == -1 ? -1 : Math.Min((int)item.WarningStorage, item.MaxSiteStorage);


            // Check package item with given name already exists.
            if (PackageController.GetPackageItemByName(item.PackageId,  item.Name, typeof(SharePointSiteCollection)) != null)
            {
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_EXISTS;
            }

            // Log operation.
            TaskManager.StartTask("HOSTEDSHAREPOINT", "ADD_SITE_COLLECTION", item.Name);

            try
            {
                // Create site collection on server.
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);

                hostedSharePointServer.CreateSiteCollection(item);

                // Make record in metabase.
                item.ServiceId = serviceId;
                int itemId = PackageController.AddPackageItem(item);

                hostedSharePointServer.SetPeoplePickerOu(item.Name, org.DistinguishedName);

                int dnsServiceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Dns);
                if (dnsServiceId > 0)
                {
                    string[] tmpStr = siteName.Split('.');
                    string hostName = tmpStr[0];
                    string domainName = siteName.Substring(hostName.Length + 1, siteName.Length - (hostName.Length + 1));

                    List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(serviceId);
                    List<DnsRecord> resourceRecords = DnsServerController.BuildDnsResourceRecords(dnsRecords, hostName, domainName, "");
                    DNSServer dns = new DNSServer();

                    ServiceProviderProxy.Init(dns, dnsServiceId);
                    // add new resource records
                    dns.AddZoneRecords(domainName, resourceRecords.ToArray());
                }

                TaskManager.ItemId = itemId;

                return itemId;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        /// Deletes SharePoint site collection with given id.
        /// </summary>
        /// <param name="itemId">Site collection id within metabase.</param>
        /// <returns>?</returns>
        public static int DeleteSiteCollection(int itemId)
        {
            // Check account.
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                return accountCheck;
            }

            // Load original meta item
            SharePointSiteCollection origItem = (SharePointSiteCollection)PackageController.GetPackageItem(itemId);
            if (origItem == null)
            {
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_NOT_FOUND;
            }

            // Get service settings.
            StringDictionary hostedSharePointSettings = ServerController.GetServiceSettings(origItem.ServiceId);
            Uri rootWebApplicationUri = new Uri(hostedSharePointSettings["RootWebApplicationUri"]);
            string siteName = origItem.Name.Replace(String.Format("{0}://", rootWebApplicationUri.Scheme), String.Empty);

            // Log operation.
            TaskManager.StartTask("HOSTEDSHAREPOINT", "DELETE_SITE", origItem.Name, itemId);

            try
            {
                // Delete site collection on server.
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(origItem.ServiceId);
                hostedSharePointServer.DeleteSiteCollection(origItem);
                // Delete record in metabase.
                PackageController.DeletePackageItem(origItem.Id);

                int dnsServiceId = PackageController.GetPackageServiceId(origItem.PackageId, ResourceGroups.Dns);
                if (dnsServiceId > 0)
                {
                    string[] tmpStr = siteName.Split('.');
                    string hostName = tmpStr[0];
                    string domainName = siteName.Substring(hostName.Length + 1, siteName.Length - (hostName.Length + 1));

                    List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(origItem.ServiceId);
                    List<DnsRecord> resourceRecords = DnsServerController.BuildDnsResourceRecords(dnsRecords, hostName, domainName, "");
                    DNSServer dns = new DNSServer();

                    ServiceProviderProxy.Init(dns, dnsServiceId);
                    // add new resource records
                    dns.DeleteZoneRecords(domainName, resourceRecords.ToArray());
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        /// Deletes SharePoint site collections which belong to organization.
        /// </summary>
        /// <param name="organizationId">Site collection id within metabase.</param>
        public static void DeleteSiteCollections(int organizationId)
        {
            Organization org = OrganizationController.GetOrganization(organizationId);
            SharePointSiteCollectionListPaged existentSiteCollections = GetSiteCollectionsPaged(org.PackageId, org.Id, String.Empty, String.Empty, String.Empty, 0, Int32.MaxValue);
            foreach (SharePointSiteCollection existentSiteCollection in existentSiteCollections.SiteCollections)
            {
                DeleteSiteCollection(existentSiteCollection.Id);
            }
        }

        /// <summary>
        /// Backups SharePoint site collection.
        /// </summary>
        /// <param name="itemId">Site collection id within metabase.</param>
        /// <param name="fileName">Backed up site collection file name.</param>
        /// <param name="zipBackup">A value which shows whether back up must be archived.</param>
        /// <param name="download">A value which shows whether created back up must be downloaded.</param>
        /// <param name="folderName">Local folder to store downloaded backup.</param>
        /// <returns>Created backup file name. </returns>
        public static string BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            // Check account.
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                return null;
            }

            // Load original meta item
            SharePointSiteCollection origItem = (SharePointSiteCollection)PackageController.GetPackageItem(itemId);
            if (origItem == null)
            {
                return null;
            }

            // Log operation.
            TaskManager.StartTask("HOSTEDSHAREPOINT", "BACKUP_SITE_COLLECTION", origItem.Name, itemId);

            try
            {
                // Create site collection on server.
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(origItem.ServiceId);
                string backFile = hostedSharePointServer.BackupSiteCollection(origItem.Name, fileName, zipBackup);

                if (!download)
                {
                    // Copy backup files to space folder.
                    string relFolderName = FilesController.CorrectRelativePath(folderName);
                    if (!relFolderName.EndsWith("\\"))
                    {
                        relFolderName = relFolderName + "\\";
                    }

                    // Create backup folder if not exists
                    if (!FilesController.DirectoryExists(origItem.PackageId, relFolderName))
                    {
                        FilesController.CreateFolder(origItem.PackageId, relFolderName);
                    }

                    string packageFile = relFolderName + Path.GetFileName(backFile);

                    // Delete destination file if exists
                    if (FilesController.FileExists(origItem.PackageId, packageFile))
                    {
                        FilesController.DeleteFiles(origItem.PackageId, new string[] { packageFile });
                    }

                    byte[] buffer = null;

                    int offset = 0;
                    do
                    {
                        // Read remote content.
                        buffer = hostedSharePointServer.GetTempFileBinaryChunk(backFile, offset, FILE_BUFFER_LENGTH);

                        // Write remote content.
                        FilesController.AppendFileBinaryChunk(origItem.PackageId, packageFile, buffer);

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);
                }

                return backFile;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        /// Restores SharePoint site collection.
        /// </summary>
        /// <param name="itemId">Site collection id within metabase.</param>
        /// <param name="uploadedFile"></param>
        /// <param name="packageFile"></param>
        /// <returns></returns>
        public static int RestoreSiteCollection(int itemId, string uploadedFile, string packageFile)
        {
            // Check account.
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                return accountCheck;
            }

            // Load original meta item.
            SharePointSiteCollection origItem = (SharePointSiteCollection)PackageController.GetPackageItem(itemId);
            if (origItem == null)
            {
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_NOT_FOUND;
            }

            // Check package.
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0)
            {
                return packageCheck;
            }

            // Log operation.
            TaskManager.StartTask("HOSTEDSHAREPOINT", "BACKUP_SITE_COLLECTION", origItem.Name, itemId);

            try
            {
                // Create site collection on server.
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(origItem.ServiceId);

                string backupFile = null;
                if (!String.IsNullOrEmpty(packageFile))
                {
                    // Copy package files to the remote SharePoint Server.
                    string path = null;
                    byte[] buffer = null;

                    int offset = 0;
                    do
                    {
                        // Read package file.
                        buffer = FilesController.GetFileBinaryChunk(origItem.PackageId, packageFile, offset, FILE_BUFFER_LENGTH);

                        // Write remote backup file
                        string tempPath = hostedSharePointServer.AppendTempFileBinaryChunk(Path.GetFileName(packageFile), path, buffer);
                        if (path == null)
                        {
                            path = tempPath;
                            backupFile = path;
                        }

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);
                }
                else if (!String.IsNullOrEmpty(uploadedFile))
                {
                    // Upladed files.
                    backupFile = uploadedFile;
                }

                // Restore.
                if (!String.IsNullOrEmpty(backupFile))
                {
                    hostedSharePointServer.RestoreSiteCollection(origItem, backupFile);
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        /// Gets binary data chunk of specified size from specified offset.
        /// </summary>
        /// <param name="itemId">Item id to obtain realted service id.</param>
        /// <param name="path">Path to file to get bunary data chunk from.</param>
        /// <param name="offset">Offset from which to start data reading.</param>
        /// <param name="length">Binary data chunk length.</param>
        /// <returns>Binary data chunk read from file.</returns>
        public static byte[] GetBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            // Load original meta item.
            SharePointSiteCollection item = (SharePointSiteCollection)PackageController.GetPackageItem(itemId);
            if (item == null)
            {
                return null;
            }

            HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(item.ServiceId);
            return hostedSharePointServer.GetTempFileBinaryChunk(path, offset, length);
        }

        /// <summary>
        /// Appends supplied binary data chunk to file.
        /// </summary>
        /// <param name="itemId">Item id to obtain realted service id.</param>
        /// <param name="fileName">Non existent file name to append to.</param>
        /// <param name="path">Full path to existent file to append to.</param>
        /// <param name="chunk">Binary data chunk to append to.</param>
        /// <returns>Path to file that was appended with chunk.</returns>
        public static string AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            // Load original meta item.
            SharePointSiteCollection item = (SharePointSiteCollection)PackageController.GetPackageItem(itemId);
            if (item == null)
            {
                return null;
            }

            HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(item.ServiceId);
            return hostedSharePointServer.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        /// <summary>
        /// Initializes a new hosted SharePoint server proxy.
        ///  </summary>
        /// <param name="serviceId">Hosted SharePoint service id.</param>
        /// <returns>Hosted SharePoint server proxy.</returns>
        private static HostedSharePointServer GetHostedSharePointServer(int serviceId)
        {

            HostedSharePointServer sps = new HostedSharePointServer();
            ServiceProviderProxy.Init(sps, serviceId);
            return sps;
        }

        /// <summary>
        /// Gets list of importable items.
        /// </summary>
        /// <param name="packageId">Package that owns importable items.</param>
        /// <param name="itemTypeId">Item type id.</param>
        /// <param name="itemType">Item type.</param>
        /// <param name="group">Item resource group.</param>
        /// <returns>List of importable item names.</returns>
        public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // Get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
            {
                return items;
            }

            HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);
            if (itemType == typeof(SharePointSiteCollection))
            {
                foreach (SharePointSiteCollection siteCollection in hostedSharePointServer.GetSiteCollections())
                {
                    items.Add(siteCollection.Url);
                }
            }

            return items;
        }

        /// <summary>
        /// Imports selected item into metabase.
        /// </summary>
        /// <param name="packageId">Package to which items must be imported.</param>
        /// <param name="itemTypeId">Item type id.</param>
        /// <param name="itemType">Item type.</param>
        /// <param name="group">Item resource group.</param>
        /// <param name="itemName">Item name to import.</param>
        public void ImportItem(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group, string itemName)
        {
            // Get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
            {
                return;
            }

            HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);
            if (itemType == typeof(SharePointSiteCollection))
            {
                SharePointSiteCollection siteCollection = hostedSharePointServer.GetSiteCollection(itemName);
                PackageController.AddPackageItem(siteCollection);
            }
        }

        /// <summary>
        /// Backups service item by serializing it into supplied writer.
        /// </summary>
        /// <param name="tempFolder">Temporary directory path.</param>
        /// <param name="writer">Xml wirter used to store backuped service provider items.</param>
        /// <param name="item">Service provider item to be backed up..</param>
        /// <param name="group">Service provider resource group.</param>
        /// <returns>Resulting code.</returns>
        public int BackupItem(string tempFolder, XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
        {
            SharePointSiteCollection siteCollection = item as SharePointSiteCollection;
            if (siteCollection != null)
            {
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(siteCollection.ServiceId);
                SharePointSiteCollection loadedSiteCollection = hostedSharePointServer.GetSiteCollection(siteCollection.Url);
                // Update item
                siteCollection.Url = loadedSiteCollection.Url;
                siteCollection.OwnerLogin = loadedSiteCollection.OwnerLogin;
                siteCollection.OwnerName = loadedSiteCollection.OwnerName;
                siteCollection.OwnerEmail = loadedSiteCollection.OwnerEmail;
                siteCollection.LocaleId = loadedSiteCollection.LocaleId;
                siteCollection.Title = loadedSiteCollection.Title;
                siteCollection.Description = loadedSiteCollection.Description;
                // Serialize it.
                XmlSerializer serializer = new XmlSerializer(typeof(SharePointSiteCollection));
                serializer.Serialize(writer, siteCollection);

            }
            return 0;
        }

        /// <summary>
        /// Restore backed up previously service provider item.
        /// </summary>
        /// <param name="tempFolder">Temporary directory path.</param>
        /// <param name="itemNode">Serialized service provider item.</param>
        /// <param name="itemId">Service provider item id.</param>
        /// <param name="itemType">Service provider item type.</param>
        /// <param name="itemName">Service provider item name.</param>
        /// <param name="packageId">Service provider item package.</param>
        /// <param name="serviceId">Service provider item service id.</param>
        /// <param name="group">Service provider item resource group.</param>
        /// <returns>Resulting code.</returns>
        public int RestoreItem(string tempFolder, XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
        {
            if (itemType == typeof(SharePointSiteCollection))
            {
                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);
                // Deserialize item.								                 
                XmlSerializer serializer = new XmlSerializer(typeof(SharePointSiteCollection));
                SharePointSiteCollection siteCollection = (SharePointSiteCollection)serializer.Deserialize(new XmlNodeReader(itemNode.SelectSingleNode("SharePointSiteCollection")));
                siteCollection.PackageId = packageId;
                siteCollection.ServiceId = serviceId;

                // Create site collection if needed.
                if (hostedSharePointServer.GetSiteCollection(siteCollection.Url) == null)
                {
                    hostedSharePointServer.CreateSiteCollection(siteCollection);
                }

                // Add metabase record if needed.
                SharePointSiteCollection metaSiteCollection = (SharePointSiteCollection)PackageController.GetPackageItemByName(packageId, itemName, typeof(SharePointSiteCollection));
                if (metaSiteCollection == null)
                {
                    PackageController.AddPackageItem(siteCollection);
                }
            }

            return 0;
        }


        private static int GetHostedSharePointServiceId(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.SharepointFoundationServer);
        }

        private static List<SharePointSiteCollection> GetOrganizationSharePointSiteCollections(int orgId)
        {
            Organization org = OrganizationController.GetOrganization(orgId);

            SharePointSiteCollectionListPaged siteCollections = GetSiteCollectionsPaged(org.PackageId, org.Id, String.Empty, String.Empty, String.Empty, 0, Int32.MaxValue);
            return siteCollections.SiteCollections;
        }

        private static int RecalculateStorageMaxSize(int size, int packageId)
        {
            PackageContext cntx = PackageController.GetPackageContext(packageId);
            QuotaValueInfo quota = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE];

            if (quota.QuotaAllocatedValue == -1)
            {
                if (size == -1)//Unlimited 
                    return -1;
                else
                    return size;
            }
            else
            {
                if (size == -1)
                    return quota.QuotaAllocatedValue;

                return Math.Min(size, quota.QuotaAllocatedValue);
            }
        }

        private static int RecalculateMaxSize(int parentSize, int realSize)
        {
            if (parentSize == -1)
            {
                if (realSize == -1 || realSize == 0)
                    return -1;
                else
                    return realSize;
            }


            if (realSize == -1 || realSize == 0)
                return parentSize;

            return Math.Min(parentSize, realSize);

        }


        public static int SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("HOSTED_SHAREPOINT", "SET_ORG_LIMITS", itemId);

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return 0;

                // set limits
                int realMaxSizeValue = RecalculateStorageMaxSize(maxStorage, org.PackageId);

                org.MaxSharePointStorage = realMaxSizeValue;

                org.WarningSharePointStorage = realMaxSizeValue == -1 ? -1 : Math.Min(warningStorage, realMaxSizeValue);

                // save organization
                UpdateOrganization(org);

                if (applyToSiteCollections)
                {
                    int serviceId = GetHostedSharePointServiceId(org.PackageId);

                    HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);

                    List<SharePointSiteCollection> currentOrgSiteCollection =
                        GetOrganizationSharePointSiteCollections(org.Id);


                    foreach (SharePointSiteCollection siteCollection in currentOrgSiteCollection)
                    {
                        try
                        {
                            SharePointSiteCollection sc = GetSiteCollection(siteCollection.Id);
                            sc.MaxSiteStorage = realMaxSizeValue;
                            sc.WarningStorage = realMaxSizeValue == -1 ? -1 : warningStorage;
                            PackageController.UpdatePackageItem(sc);

                            hostedSharePointServer.UpdateQuotas(siteCollection.PhysicalAddress, realMaxSizeValue,
                                                                warningStorage);
                        }
                        catch (Exception ex)
                        {
                            TaskManager.WriteError(ex);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static SharePointSiteDiskSpace[] CalculateSharePointSitesDiskSpace(int itemId, out int errorCode)
        {
            SharePointSiteDiskSpace[] retDiskSpace = null;
            errorCode = 0;
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                errorCode = accountCheck;
                return null;
            }

            // place log record
            TaskManager.StartTask("HOSTED_SHAREPOINT", "CALCULATE_DISK_SPACE", itemId);

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return null;

                int serviceId = GetHostedSharePointServiceId(org.PackageId);

                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);

                List<SharePointSiteCollection> currentOrgSiteCollection =
                    GetOrganizationSharePointSiteCollections(org.Id);

                List<string> urls = new List<string>();
                foreach (SharePointSiteCollection siteCollection in currentOrgSiteCollection)
                {
                    urls.Add(siteCollection.PhysicalAddress);
                }
                if (urls.Count > 0)
                    retDiskSpace = hostedSharePointServer.CalculateSiteCollectionsDiskSpace(urls.ToArray());
                else
                {
                    retDiskSpace = new SharePointSiteDiskSpace[1];
                    retDiskSpace[0] = new SharePointSiteDiskSpace();
                    retDiskSpace[0].DiskSpace = 0;
                    retDiskSpace[0].Url = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
            return retDiskSpace;
        }

        private static void UpdateOrganization(Organization organization)
        {
            PackageController.UpdatePackageItem(organization);
        }


        public static void UpdateQuota(int itemId, int siteCollectionId, int maxStorage, int warningStorage)
        {
            TaskManager.StartTask("HOSTED_SHAREPOINT", "UPDATE_QUOTA");
            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return;

                int serviceId = GetHostedSharePointServiceId(org.PackageId);

                HostedSharePointServer hostedSharePointServer = GetHostedSharePointServer(serviceId);

                SharePointSiteCollection sc = GetSiteCollection(siteCollectionId);

                int maxSize = RecalculateMaxSize(org.MaxSharePointStorage, maxStorage);
                int warningSize = warningStorage;


                sc.MaxSiteStorage = maxSize;
                sc.WarningStorage = maxSize == -1 ? -1 : Math.Min(warningSize, maxSize);
                PackageController.UpdatePackageItem(sc);

                hostedSharePointServer.UpdateQuotas(sc.PhysicalAddress, maxSize,
                                                    warningStorage);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        /// <summary>
        /// Gets a value if caller is in demo mode.
        /// </summary>
        private static bool IsDemoMode
        {
            get
            {
                return (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);
            }
        }

     }
}
