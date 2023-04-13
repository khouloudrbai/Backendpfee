using System;
using System.Collections.Generic;
using System.Text;

namespace PFE.SMSNotification.Utility
{
    public static class Config
    {
        static public string CONNECTION_STRING { get; set; }
        static public string SCHEMA { get; set; }
        static public string DIRECTORY_TRACE { get; set; }
        static public string TRACE_LEVEL { get; set; }
    }
}
