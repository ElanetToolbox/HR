﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HR.Web
{
    public static class Functions
    {
        public static string GetFileNameFromPath(string fName)
        {
            return Path.GetFileName(fName);
        }
    }
}