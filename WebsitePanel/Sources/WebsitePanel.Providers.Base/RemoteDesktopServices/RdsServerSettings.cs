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
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebsitePanel.EnterpriseServer.Base.RDS
{
    public class RdsServerSettings
    {
        private List<RdsServerSetting> settings = null;

        public const string LOCK_SCREEN_TIMEOUT = "LockScreenTimeout";        
        public const string REMOVE_RUN_COMMAND = "RemoveRunCommand";        
        public const string REMOVE_POWERSHELL_COMMAND = "RemovePowershellCommand";        
        public const string HIDE_C_DRIVE = "HideCDrive";        
        public const string REMOVE_SHUTDOWN_RESTART = "RemoveShutdownRestart";        
        public const string DISABLE_TASK_MANAGER = "DisableTaskManager";
        public const string CHANGE_DESKTOP_DISABLED = "ChangingDesktopDisabled";        
        public const string SCREEN_SAVER_DISABLED = "ScreenSaverDisabled";        
        public const string DRIVE_SPACE_THRESHOLD = "DriveSpaceThreshold";

        public const string LOCK_SCREEN_TIMEOUT_VALUE = "LockScreenTimeoutValue";
        public const string LOCK_SCREEN_TIMEOUT_ADMINISTRATORS = "LockScreenTimeoutAdministrators";
        public const string LOCK_SCREEN_TIMEOUT_USERS = "LockScreenTimeoutUsers";
        public const string REMOVE_RUN_COMMAND_ADMINISTRATORS = "RemoveRunCommandAdministrators";
        public const string REMOVE_RUN_COMMAND_USERS = "RemoveRunCommandUsers";
        public const string REMOVE_POWERSHELL_COMMAND_ADMINISTRATORS = "RemovePowershellCommandAdministrators";
        public const string REMOVE_POWERSHELL_COMMAND_USERS = "RemovePowershellCommandUsers";
        public const string HIDE_C_DRIVE_ADMINISTRATORS = "HideCDriveAdministrators";
        public const string HIDE_C_DRIVE_USERS = "HideCDriveUsers";
        public const string REMOVE_SHUTDOWN_RESTART_ADMINISTRATORS = "RemoveShutdownRestartAdministrators";
        public const string REMOVE_SHUTDOWN_RESTART_USERS = "RemoveShutdownRestartUsers";
        public const string DISABLE_TASK_MANAGER_ADMINISTRATORS = "DisableTaskManagerAdministrators";
        public const string DISABLE_TASK_MANAGER_USERS = "DisableTaskManagerUsers";
        public const string CHANGE_DESKTOP_DISABLED_ADMINISTRATORS = "ChangingDesktopDisabledAdministrators";
        public const string CHANGE_DESKTOP_DISABLED_USERS = "ChangingDesktopDisabledUsers";
        public const string SCREEN_SAVER_DISABLED_ADMINISTRATORS = "ScreenSaverDisabledAdministrators";
        public const string SCREEN_SAVER_DISABLED_USERS = "ScreenSaverDisabledUsers";
        public const string DRIVE_SPACE_THRESHOLD_VALUE = "DriveSpaceThresholdValue";
        public const string RDS_VIEW_WITHOUT_PERMISSION = "RDSViewWithoutPermission";
        public const string RDS_VIEW_WITHOUT_PERMISSION_ADMINISTRATORS = "RDSViewWithoutPermissionAdministrators";
        public const string RDS_VIEW_WITHOUT_PERMISSION_Users = "RDSViewWithoutPermissionUsers";
        public const string RDS_CONTROL_WITHOUT_PERMISSION = "RDSControlWithoutPermission";
        public const string RDS_CONTROL_WITHOUT_PERMISSION_ADMINISTRATORS = "RDSControlWithoutPermissionAdministrators";
        public const string RDS_CONTROL_WITHOUT_PERMISSION_Users = "RDSControlWithoutPermissionUsers";
        public const string DISABLE_CMD = "DisableCMD";
        public const string DISABLE_CMD_ADMINISTRATORS = "DisableCMDAdministrators";
        public const string DISABLE_CMD_USERS = "DisableCMDUsers";
        public const string ALLOWCOLLECTIONSIMPORT = "AllowCollectionsImport";

        public string SettingsName { get; set; }
        public int ServerId { get; set; }

        public List<RdsServerSetting> Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new List<RdsServerSetting>();
                }
                return settings;
            }
            set
            {
                settings = value;
            }
        }

        public static List<KeyValuePair<string, string>> ScreenSaverTimeOuts
        {
            get
            {
                return new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("", "None"),
                    new KeyValuePair<string, string>("10", "10"),
                    new KeyValuePair<string, string>("20", "20"),
                    new KeyValuePair<string, string>("30", "30"),
                    new KeyValuePair<string, string>("40", "40"),
                    new KeyValuePair<string, string>("50", "50"),
                    new KeyValuePair<string, string>("60", "60")
                };
            }
        }
    }
}
