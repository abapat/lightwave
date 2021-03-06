﻿/*
 * Copyright © 2012-2015 VMware, Inc.  All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the “License”); you may not
 * use this file except in compliance with the License.  You may obtain a copy
 * of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an “AS IS” BASIS, without
 * warranties or conditions of any kind, EITHER EXPRESS OR IMPLIED.  See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using Vmware.Tools.RestSsoAdminSnapIn.Core.Serialization;
using Vmware.Tools.RestSsoAdminSnapIn.Helpers.Interop;

namespace Vmware.Tools.RestSsoAdminSnapIn.Helpers
{
    public class KerberosHelper
    {
        public static string GetKerberosContext(string user, string pass, string domain, string sts)
        {
            IntPtr iSPN = Marshal.StringToBSTR(sts);
            IntPtr iUser = Marshal.StringToBSTR(user);
            IntPtr iPass = Marshal.StringToBSTR(pass);
            IntPtr iDomain = Marshal.StringToBSTR(domain);

            var sb = new StringBuilder(4096);
            var result = SsoSsspiInterop.InitSecurityContext(iSPN, iUser, iPass, iDomain, sb);
            if (sb.Length > 0)
                throw new Exception(sb.ToString());

            string op = Marshal.PtrToStringBSTR(result);

            var dto = XmlConvert.Deserialize<SecurityContext>(op);

            bool deleteResult = SsoSsspiInterop.CloseSecurityContext(dto.ID, sb);
            if (sb.Length > 0)
                throw new Exception(sb.ToString());

            return dto.Context;
        }
    }

    public class SecurityContext    {        public long ID { get; set; }        public string Context { get; set; }    }
}
