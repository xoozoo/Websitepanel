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
using System.Collections;
using System.Collections.Generic;
using WebsitePanel.Providers.DNS;
using WebsitePanel.Providers.DomainLookup;

namespace WebsitePanel.Providers.OS
{
    /// <summary>
    /// Summary description for IOperationSystem.
    /// </summary>
    public interface IOperatingSystem
    {
        // files
        string CreatePackageFolder(string initialPath);
        bool FileExists(string path);
        bool DirectoryExists(string path);
        SystemFile GetFile(string path);
        SystemFile[] GetFiles(string path);
        SystemFile[] GetDirectoriesRecursive(string rootFolder, string path);
        SystemFile[] GetFilesRecursive(string rootFolder, string path);
        SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern);
        byte[] GetFileBinaryContent(string path);
		byte[] GetFileBinaryContentUsingEncoding(string path, string encoding);
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        string GetFileTextContent(string path);
        void CreateFile(string path);
        void CreateDirectory(string path);
        void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime);
        void DeleteFile(string path);
        void DeleteFiles(string[] files);
        void DeleteEmptyDirectories(string[] directories);
        void UpdateFileBinaryContent(string path, byte[] content);
		void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding);
        void AppendFileBinaryContent(string path, byte[] chunk);
        void UpdateFileTextContent(string path, string content);
        void MoveFile(string sourcePath, string destinationPath);
        void CopyFile(string sourcePath, string destinationPath);
        void ZipFiles(string zipFile, string rootPath, string[] files);
        string[] UnzipFiles(string zipFile, string destFolder);

        UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU);
        void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions);

        // ODBC DSNs
        string[] GetInstalledOdbcDrivers();
        string[] GetDSNNames();
        SystemDSN GetDSN(string dsnName);
        void CreateDSN(SystemDSN dsn);
        void UpdateDSN(SystemDSN dsn);
        void DeleteDSN(string dsnName);

        // Access databases
        void CreateAccessDatabase(string databasePath);

        // Synchronizing
        FolderGraph GetFolderGraph(string path);
        void ExecuteSyncActions(FileSyncAction[] actions);

        void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword);
        Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword);
        void DeleteDirectoryRecursive(string rootPath);

        // File Services
        bool CheckFileServicesInstallation();
        bool InstallFsrmService();
    }
}
