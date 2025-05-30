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

namespace WebsitePanel.Providers.Virtualization
{
    public enum VMComputerSystemStateInfo : int
    {
        CheckpointFailed = 213,
        CreatingCheckpoint = 210,
        CreationFailed = 101,
        CustomizationFailed = 105,
        Deleting = 13,
        DeletingCheckpoint = 211,
        DiscardingDrives = 80,
        DiscardSavedState = 10,
        FinishingCheckpointOperation = 215,
        HostNotResponding = 221,
        IncompleteVMConfig = 223,
        InitializingCheckpointOperation = 214,
        MergingDrives = 12,
        MigrationFailed = 201,
        Missing = 220,
        P2VCreationFailed = 240,
        Paused = 6,
        Pausing = 81,
        PoweringOff = 2,
        PowerOff = 1,
        RecoveringCheckpoint = 212,
        Restoring = 5,
        Running = 0,
        Saved = 3,
        Saving = 4,
        Starting = 11,
        Stored = 102,
        TemplateCreationFailed = 104,
        UnderCreation = 100,
        UnderMigration = 200,
        UnderTemplateCreation = 103,
        UnderUpdate = 106,
        Unsupported = 222,
        UnsupportedCluster = 225,
        UnsupportedSharedFiles = 224,
        UpdateFailed = 107,
        V2VCreationFailed = 250
    }
}
