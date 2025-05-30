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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebsitePanel.WebDav.Core.Config.WebConfigSections;

namespace WebsitePanel.WebDav.Core.Config.Entities
{
    public class FileIconsDictionary : AbstractConfigCollection, IReadOnlyDictionary<string, string>
    {
        private readonly IDictionary<string, string> _fileIcons;

        public FileIconsDictionary()
        {
            DefaultPath = ConfigSection.FileIcons.DefaultPath;
            FolderPath = ConfigSection.FileIcons.FolderPath;
            _fileIcons = ConfigSection.FileIcons.Cast<FileIconsElement>().ToDictionary(x => x.Extension, y => y.Path);
        }

        public string DefaultPath { get; private set; }
        public string FolderPath { get; private set; }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _fileIcons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _fileIcons.Count; }
        }

        public bool ContainsKey(string extension)
        {
            return _fileIcons.ContainsKey(extension);
        }

        public bool TryGetValue(string extension, out string path)
        {
            return _fileIcons.TryGetValue(extension, out path);
        }

        public string this[string extension]
        {
            get { return ContainsKey(extension) ? _fileIcons[extension] : DefaultPath; }
        }

        public IEnumerable<string> Keys
        {
            get { return _fileIcons.Keys; }
        }

        public IEnumerable<string> Values
        {
            get { return _fileIcons.Values; }
        }
    }
}
