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
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;

using WebsitePanel.Providers;
using WebsitePanel.Providers.Virtualization;
using WebsitePanel.Server.Utils;
using System.Collections.Generic;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for VirtualizationServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
	public class VirtualizationServerForPrivateCloud : HostingServiceProviderWebService, WebsitePanel.Providers.VirtualizationForPC.IVirtualizationServerForPC
    {
		private WebsitePanel.Providers.VirtualizationForPC.IVirtualizationServerForPC VirtualizationForPC
        {
			get { return (WebsitePanel.Providers.VirtualizationForPC.IVirtualizationServerForPC)Provider; }
        }

        #region Virtual Machines

        [WebMethod, SoapHeader("settings")]
        public VMInfo GetVirtualMachine(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachine", ProviderSettings.ProviderName);
                VMInfo result = VirtualizationForPC.GetVirtualMachine(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineEx", ProviderSettings.ProviderName);
                VirtualMachine result = VirtualizationForPC.GetVirtualMachineEx(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachineEx", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineEx", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<VirtualMachine> GetVirtualMachines()
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachines", ProviderSettings.ProviderName);
                List<VirtualMachine> result = VirtualizationForPC.GetVirtualMachines();
                Log.WriteEnd("'{0}' GetVirtualMachines", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachines", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineThumbnailImage", ProviderSettings.ProviderName);
                byte[] result = VirtualizationForPC.GetVirtualMachineThumbnailImage(vmId, size);
                Log.WriteEnd("'{0}' GetVirtualMachineThumbnailImage", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineThumbnailImage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VMInfo CreateVirtualMachine(VMInfo vm)
        {
            try
            {
                Log.WriteStart("'{0}' CreateVirtualMachine", ProviderSettings.ProviderName);
                VMInfo result = VirtualizationForPC.CreateVirtualMachine(vm);
                Log.WriteEnd("'{0}' CreateVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid)
        {
            VMInfo result = vmTemplate;
            try
            {
                Log.WriteStart("'{0}' CreateVMFromVM", ProviderSettings.ProviderName);
                result = VirtualizationForPC.CreateVMFromVM(sourceName, vmTemplate, taskGuid);
                Log.WriteEnd("'{0}' CreateVMFromVM", ProviderSettings.ProviderName);
                return result;
            }
            catch (System.TimeoutException)
            {
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateVMFromVM", ProviderSettings.ProviderName), ex);
                throw;
            }

        }

        [WebMethod, SoapHeader("settings")]
        public VMInfo UpdateVirtualMachine(VMInfo vm)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateVirtualMachine", ProviderSettings.ProviderName);
                VMInfo result = VirtualizationForPC.UpdateVirtualMachine(vm);
                Log.WriteEnd("'{0}' UpdateVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeVirtualMachineState", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.ChangeVirtualMachineState(vmId, newState);
                Log.WriteEnd("'{0}' ChangeVirtualMachineState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeVirtualMachineState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            try
            {
                Log.WriteStart("'{0}' ShutDownVirtualMachine", ProviderSettings.ProviderName);
                ReturnCode result = VirtualizationForPC.ShutDownVirtualMachine(vmId, force, reason);
                Log.WriteEnd("'{0}' ShutDownVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShutDownVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineJobs", ProviderSettings.ProviderName);
                List<ConcreteJob> result = VirtualizationForPC.GetVirtualMachineJobs(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachineJobs", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineJobs", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            try
            {
                Log.WriteStart("'{0}' RenameVirtualMachine", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.RenameVirtualMachine(vmId, name);
                Log.WriteEnd("'{0}' RenameVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult DeleteVirtualMachine(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteVirtualMachine", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.DeleteVirtualMachine(vmId);
                Log.WriteEnd("'{0}' DeleteVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        //[WebMethod, SoapHeader("settings")]
        //public JobResult ExportVirtualMachine(string vmId, string exportPath)
        //{
        //    try
        //    {
        //        Log.WriteStart("'{0}' ExportVirtualMachine", ProviderSettings.ProviderName);
        //        JobResult result = VirtualizationForPC.ExportVirtualMachine(vmId, exportPath);
        //        Log.WriteEnd("'{0}' ExportVirtualMachine", ProviderSettings.ProviderName);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteError(String.Format("'{0}' ExportVirtualMachine", ProviderSettings.ProviderName), ex);
        //        throw;
        //    }
        //}
        #endregion

        #region Snapshots
        [WebMethod, SoapHeader("settings")]
        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineSnapshots", ProviderSettings.ProviderName);
                List<VirtualMachineSnapshot> result = VirtualizationForPC.GetVirtualMachineSnapshots(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachineSnapshots", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineSnapshots", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSnapshot", ProviderSettings.ProviderName);
                VirtualMachineSnapshot result = VirtualizationForPC.GetSnapshot(snapshotId);
                Log.WriteEnd("'{0}' GetSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult CreateSnapshot(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.CreateSnapshot(vmId);
                Log.WriteEnd("'{0}' CreateSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            try
            {
                Log.WriteStart("'{0}' RenameSnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.RenameSnapshot(vmId, snapshotId, name);
                Log.WriteEnd("'{0}' RenameSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' ApplySnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.ApplySnapshot(vmId, snapshotId);
                Log.WriteEnd("'{0}' ApplySnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ApplySnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.DeleteSnapshot(vmId, snapshotId);
                Log.WriteEnd("'{0}' DeleteSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSnapshotSubtree", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.DeleteSnapshotSubtree(snapshotId);
                Log.WriteEnd("'{0}' DeleteSnapshotSubtree", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSnapshotSubtree", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            try
            {
                Log.WriteStart("'{0}' GetSnapshotThumbnailImage", ProviderSettings.ProviderName);
                byte[] result = VirtualizationForPC.GetSnapshotThumbnailImage(snapshotId, size);
                Log.WriteEnd("'{0}' GetSnapshotThumbnailImage", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSnapshotThumbnailImage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Virtual Switches
        [WebMethod, SoapHeader("settings")]
        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            try
            {
                Log.WriteStart("'{0}' GetExternalSwitches", ProviderSettings.ProviderName);
                List<VirtualSwitch> result = VirtualizationForPC.GetExternalSwitches(computerName);
                Log.WriteEnd("'{0}' GetExternalSwitches", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetExternalSwitches", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<VirtualSwitch> GetSwitches()
        {
            try
            {
                Log.WriteStart("'{0}' GetSwitches", ProviderSettings.ProviderName);
                List<VirtualSwitch> result = VirtualizationForPC.GetSwitches();
                Log.WriteEnd("'{0}' GetSwitches", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSwitches", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SwitchExists(string switchId)
        {
            try
            {
                Log.WriteStart("'{0}' SwitchExists", ProviderSettings.ProviderName);
                bool result = VirtualizationForPC.SwitchExists(switchId);
                Log.WriteEnd("'{0}' SwitchExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SwitchExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VirtualSwitch CreateSwitch(string name)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSwitch", ProviderSettings.ProviderName);
                VirtualSwitch result = VirtualizationForPC.CreateSwitch(name);
                Log.WriteEnd("'{0}' CreateSwitch", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSwitch", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReturnCode DeleteSwitch(string switchId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSwitch", ProviderSettings.ProviderName);
                ReturnCode result = VirtualizationForPC.DeleteSwitch(switchId);
                Log.WriteEnd("'{0}' DeleteSwitch", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSwitch", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region DVD operations
        [WebMethod, SoapHeader("settings")]
        public string GetInsertedDVD(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetInsertedDVD", ProviderSettings.ProviderName);
                string result = VirtualizationForPC.GetInsertedDVD(vmId);
                Log.WriteEnd("'{0}' GetInsertedDVD", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetInsertedDVD", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult InsertDVD(string vmId, string isoPath)
        {
            try
            {
                Log.WriteStart("'{0}' InsertDVD", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.InsertDVD(vmId, isoPath);
                Log.WriteEnd("'{0}' InsertDVD", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InsertDVD", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult EjectDVD(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' EjectDVD", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.EjectDVD(vmId);
                Log.WriteEnd("'{0}' EjectDVD", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' EjectDVD", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Library
        [WebMethod, SoapHeader("settings")]
        public LibraryItem[] GetLibraryItems(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetLibraryItems", ProviderSettings.ProviderName);
                LibraryItem[] result = VirtualizationForPC.GetLibraryItems(path);
                Log.WriteEnd("'{0}' GetLibraryItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetLibraryItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public LibraryItem[] GetOSLibraryItems()
        {
            try
            {
                Log.WriteStart("'{0}' GetLibraryItems", ProviderSettings.ProviderName);
                LibraryItem[] result = VirtualizationForPC.GetOSLibraryItems();
                Log.WriteEnd("'{0}' GetLibraryItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetLibraryItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public LibraryItem[] GetHosts()
        {
            try
            {
                Log.WriteStart("'{0}' GetHosts", ProviderSettings.ProviderName);
                LibraryItem[] result = VirtualizationForPC.GetHosts();
                Log.WriteEnd("'{0}' GetHosts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHosts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public LibraryItem[] GetClusters()
        {
            try
            {
                Log.WriteStart("'{0}' GetHosts", ProviderSettings.ProviderName);
                LibraryItem[] result = VirtualizationForPC.GetClusters();
                Log.WriteEnd("'{0}' GetHosts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHosts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region KVP items
        [WebMethod, SoapHeader("settings")]
        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetKVPItems", ProviderSettings.ProviderName);
                List<KvpExchangeDataItem> result = VirtualizationForPC.GetKVPItems(vmId);
                Log.WriteEnd("'{0}' GetKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetStandardKVPItems", ProviderSettings.ProviderName);
                List<KvpExchangeDataItem> result = VirtualizationForPC.GetStandardKVPItems(vmId);
                Log.WriteEnd("'{0}' GetStandardKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetStandardKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            try
            {
                Log.WriteStart("'{0}' AddKVPItems", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.AddKVPItems(vmId, items);
                Log.WriteEnd("'{0}' AddKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveKVPItems", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.RemoveKVPItems(vmId, itemNames);
                Log.WriteEnd("'{0}' RemoveKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            try
            {
                Log.WriteStart("'{0}' ModifyKVPItems", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.ModifyKVPItems(vmId, items);
                Log.WriteEnd("'{0}' ModifyKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ModifyKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Storage
        [WebMethod, SoapHeader("settings")]
        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualHardDiskInfo", ProviderSettings.ProviderName);
                VirtualHardDiskInfo result = VirtualizationForPC.GetVirtualHardDiskInfo(vhdPath);
                Log.WriteEnd("'{0}' GetVirtualHardDiskInfo", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualHardDiskInfo", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            try
            {
                Log.WriteStart("'{0}' MountVirtualHardDisk", ProviderSettings.ProviderName);
                MountedDiskInfo result = VirtualizationForPC.MountVirtualHardDisk(vhdPath);
                Log.WriteEnd("'{0}' MountVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MountVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            try
            {
                Log.WriteStart("'{0}' UnmountVirtualHardDisk", ProviderSettings.ProviderName);
                ReturnCode result = VirtualizationForPC.UnmountVirtualHardDisk(vhdPath);
                Log.WriteEnd("'{0}' UnmountVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UnmountVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            try
            {
                Log.WriteStart("'{0}' ExpandVirtualHardDisk", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.ExpandVirtualHardDisk(vhdPath, sizeGB);
                Log.WriteEnd("'{0}' ExpandVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExpandVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            try
            {
                Log.WriteStart("'{0}' ConvertVirtualHardDisk", ProviderSettings.ProviderName);
                JobResult result = VirtualizationForPC.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType);
                Log.WriteEnd("'{0}' ConvertVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ConvertVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteRemoteFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteRemoteFile", ProviderSettings.ProviderName);
                VirtualizationForPC.DeleteRemoteFile(path);
                Log.WriteEnd("'{0}' DeleteRemoteFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteRemoteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            try
            {
                Log.WriteStart("'{0}' ExpandDiskVolume", ProviderSettings.ProviderName);
                VirtualizationForPC.ExpandDiskVolume(diskAddress, volumeName);
                Log.WriteEnd("'{0}' ExpandDiskVolume", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExpandDiskVolume", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string ReadRemoteFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' ReadRemoteFile", ProviderSettings.ProviderName);
                string result = VirtualizationForPC.ReadRemoteFile(path);
                Log.WriteEnd("'{0}' ReadRemoteFile", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ReadRemoteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void WriteRemoteFile(string path, string content)
        {
            try
            {
                Log.WriteStart("'{0}' WriteRemoteFile", ProviderSettings.ProviderName);
                VirtualizationForPC.WriteRemoteFile(path, content);
                Log.WriteEnd("'{0}' WriteRemoteFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' WriteRemoteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Jobs
        [WebMethod, SoapHeader("settings")]
        public ConcreteJob GetJob(string jobId)
        {
            try
            {
                Log.WriteStart("'{0}' GetJob", ProviderSettings.ProviderName);
                ConcreteJob result = VirtualizationForPC.GetJob(jobId);
                Log.WriteEnd("'{0}' GetJob", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetJob", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<ConcreteJob> GetAllJobs()
        {
            try
            {
                Log.WriteStart("'{0}' GetAllJobs", ProviderSettings.ProviderName);
                List<ConcreteJob> result = VirtualizationForPC.GetAllJobs();
                Log.WriteEnd("'{0}' GetAllJobs", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAllJobs", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeJobState", ProviderSettings.ProviderName);
                ChangeJobStateReturnCode result = VirtualizationForPC.ChangeJobState(jobId, newState);
                Log.WriteEnd("'{0}' ChangeJobState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeJobState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Configuration
        [WebMethod, SoapHeader("settings")]
        public int GetProcessorCoresNumber(string templateId)
        {
            try
            {
                Log.WriteStart("'{0}' GetProcessorCoresNumber", ProviderSettings.ProviderName);
                int result = VirtualizationForPC.GetProcessorCoresNumber(templateId);
                Log.WriteEnd("'{0}' GetProcessorCoresNumber", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetProcessorCoresNumber", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Hyper-V Cloud
        [WebMethod, SoapHeader("settings")]
        public bool CheckServerState(VMForPCSettingsName control, string connString, string connName)
        {
            try
            {
                Log.WriteStart("'{0}' CheckServerState", ProviderSettings.ProviderName);
                bool result = VirtualizationForPC.CheckServerState(control, connString, connName);
                Log.WriteEnd("'{0}' CheckServerState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckServerState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion Hyper-V Cloud

        #region Monitoring
        [WebMethod, SoapHeader("settings")]
        public List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName)
        {
            try
            {
                Log.WriteStart("'{0}' GetDeviceEvents", ProviderSettings.ProviderName);
                List<MonitoredObjectEvent> result = VirtualizationForPC.GetDeviceEvents(serviceName, displayName);
                Log.WriteEnd("'{0}' GetDeviceEvents", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDeviceEvents", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName)
        {
            try
            {
                Log.WriteStart("'{0}' GetMonitoringAlerts", ProviderSettings.ProviderName);
                List<MonitoredObjectAlert> result = VirtualizationForPC.GetMonitoringAlerts(serviceName, virtualMachineName);
                Log.WriteEnd("'{0}' GetMonitoringAlerts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetMonitoringAlerts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
        {
            try
            {
                Log.WriteStart("'{0}' GetPerfomanceValue", ProviderSettings.ProviderName);
                List<PerformanceDataValue> result = VirtualizationForPC.GetPerfomanceValue(VmName, perf, startPeriod, endPeriod);
                Log.WriteEnd("'{0}' GetPerfomanceValue", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetPerfomanceValue", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Networing
        [WebMethod, SoapHeader("settings")]
        public void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
        {
            try
            {
                Log.WriteStart("'{0}' ConfigureCreatedVMNetworkAdapters", ProviderSettings.ProviderName);
                VirtualizationForPC.ConfigureCreatedVMNetworkAdapters(vmInfo);
                Log.WriteEnd("'{0}' ConfigureCreatedVMNetworkAdapters", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ConfigureCreatedVMNetworkAdapters", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VMInfo MoveVM(VMInfo vmForMove)
        {
            try
            {
                Log.WriteStart("'{0}' MoveVM", ProviderSettings.ProviderName);
                VMInfo ret = VirtualizationForPC.MoveVM(vmForMove);
                Log.WriteEnd("'{0}' MoveVM", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveVM", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualNetworkByHostName", ProviderSettings.ProviderName);
                VirtualNetworkInfo[] result = VirtualizationForPC.GetVirtualNetworkByHostName(hostName);
                Log.WriteEnd("'{0}' GetVirtualNetworkByHostName", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualNetworkByHostName", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion
    }
}
