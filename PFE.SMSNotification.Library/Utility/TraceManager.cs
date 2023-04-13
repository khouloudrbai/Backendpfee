using PFE.SMSNotification.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;

namespace PFE.SMSNotification.Library.Utility
{
    public class TraceManager
    {
        private string strLogFileName = "";
        private string DATA_SEPARATOR_LOG_COLUMN = "¤";
        private string DATA_SEPARATOR_LOG_ROW = "$";
        private string DIRECTORY_TRACE = Config.DIRECTORY_TRACE;
        private string TRACE_LEVEL = Config.TRACE_LEVEL;
        public enum TRACELEVEL { Desactivated = 0, Followed = 1, Warning = 2, Info = 3, Advanced = 4, Verbose = 5 };
        public HttpContext _HttpContext = null;

        public TraceManager(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            DIRECTORY_TRACE = Config.DIRECTORY_TRACE;
            TRACE_LEVEL = Config.TRACE_LEVEL;
        }

        public TraceManager()
        {
            DIRECTORY_TRACE = Config.DIRECTORY_TRACE;
            TRACE_LEVEL = Config.TRACE_LEVEL;
        }

        private string GetLogFileName(System.DateTime objDateTime)
        {
            return objDateTime.ToString("yyMMdd") + ".log";
        }

        public void InitializeTraceListener(string strCurrentFileName)
        {
            // Creates the text file that the trace listener will write to.
            FileStream objFileStream = new FileStream(DIRECTORY_TRACE + strCurrentFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            // Creates the new trace listener.
            TextWriterTraceListener objTextWriterTraceListener = new TextWriterTraceListener(objFileStream);

            //add TextWriterTraceListener to listeners collection
            Trace.Listeners.Clear();
            Trace.Listeners.Add(objTextWriterTraceListener);

            strLogFileName = strCurrentFileName;
        }


        public void WriteLog(string traceMethod, string message, string detailMessage, TRACELEVEL enumTraceLevel)
        {
            if (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Desactivated, "d").ToString()) != -1)
                return;

            string currentFileName = GetLogFileName(DateTime.Now);

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            string userId = "NaN";

            /*
            §TraceDateTime•IP•URL•User•Method•TraceLevel•Message 
            Détail
            */
            DateTime traceDateTime = DateTime.Now;
            string traceIP = "127.0.0.1";
            string traceURL = "";

            try
            {
                traceIP = _HttpContext.Connection.RemoteIpAddress.ToString(); ; //Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                traceURL = _HttpContext.Request.PathBase.ToString();
            }
            catch
            {
            }
            string traceUser = userId;
            string traceLevel = enumTraceLevel.ToString();
            string traceMessage = message;
            string traceDetail = detailMessage;

            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            errorMsg.Append(DATA_SEPARATOR_LOG_ROW);
            errorMsg.Append(traceDateTime);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceIP);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceURL);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceUser);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMethod);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceLevel);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMessage);

            //Ecrire dans le fichier log
            Trace.WriteLine(errorMsg.ToString());

            if ((TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Advanced, "d").ToString()) != -1)
                ||
                (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Verbose, "d").ToString()) != -1))
            {
                //Ecrire dans le fichier log
                Trace.WriteLine(traceDetail);
            }

            //vider la mémoire 
            Trace.Flush();
        }

        public void WriteLog(string traceMethod, string message, TRACELEVEL enumTraceLevel)
        {
            if (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Desactivated, "d").ToString()) != -1)
                return;

            string currentFileName = GetLogFileName(DateTime.Now);

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            string userId = "NaN";

            /*
            §TraceDateTime•IP•URL•User•Method•TraceLevel•Message 
            */
            DateTime traceDateTime = DateTime.Now;
            string traceIP = "127.0.0.1";
            string traceURL = "";

            try
            {
                traceIP = _HttpContext.Connection.RemoteIpAddress.ToString(); ; //Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                traceURL = _HttpContext.Request.PathBase.ToString();
            }
            catch
            {

            }
            string traceUser = userId;
            string traceLevel = enumTraceLevel.ToString();
            string traceMessage = message;

            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            errorMsg.Append(DATA_SEPARATOR_LOG_ROW);
            errorMsg.Append(traceDateTime);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceIP);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceURL);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceUser);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMethod);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceLevel);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMessage);

            //Ecrire dans le fichier log
            Trace.WriteLine(errorMsg.ToString());
            //vider la mémoire 
            Trace.Flush();
        }

        public void WriteLog(string traceMethod, string message, string detailMessage)
        {
            if (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Desactivated, "d").ToString()) != -1)
                return;

            string currentFileName = GetLogFileName(DateTime.Now);

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            string userId = "NaN";

            /*
            §TraceDateTime•IP•URL•User•Method•TraceLevel•Message 
            Détail
            */
            DateTime traceDateTime = DateTime.Now;
            string traceIP = "127.0.0.1";
            string traceURL = "";

            try
            {
                traceIP = _HttpContext.Connection.RemoteIpAddress.ToString(); ; //Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                traceURL = _HttpContext.Request.PathBase.ToString();
            }
            catch
            {

            }
            string traceUser = userId;
            string traceLevel = TRACELEVEL.Info.ToString();
            string traceMessage = message;
            string traceDetail = detailMessage;

            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            errorMsg.Append(DATA_SEPARATOR_LOG_ROW);
            errorMsg.Append(traceDateTime);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceIP);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceURL);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceUser);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMethod);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceLevel);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMessage);

            //Ecrire dans le fichier log
            Trace.WriteLine(errorMsg.ToString());

            if ((TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Advanced, "d").ToString()) != -1)
                ||
                (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Verbose, "d").ToString()) != -1))
            {
                //Ecrire dans le fichier log
                Trace.WriteLine(traceDetail);
            }

            //vider la mémoire 
            Trace.Flush();
        }

        public void WriteLog(string traceMethod, string message)
        {
            if (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Desactivated, "d").ToString()) != -1)
                return;

            string currentFileName = GetLogFileName(DateTime.Now);

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            string userId = "NaN";

            /*
            §TraceDateTime•IP•URL•User•Method•TraceLevel•Message 
            Détail
            */




            DateTime traceDateTime = DateTime.Now;

            string traceIP = "127.0.0.1";
            string traceURL = "";

            try
            {
                traceIP = _HttpContext.Connection.RemoteIpAddress.ToString(); ; //Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                traceURL = _HttpContext.Request.PathBase.ToString();
            }
            catch
            {

            }
            string traceUser = userId;
            string traceLevel = TRACELEVEL.Info.ToString();
            string traceMessage = message;

            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            errorMsg.Append(DATA_SEPARATOR_LOG_ROW);
            errorMsg.Append(traceDateTime);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceIP);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceURL);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceUser);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMethod);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceLevel);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMessage);

            //Ecrire dans le fichier log
            Trace.WriteLine(errorMsg.ToString());
            //vider la mémoire 
            Trace.Flush();
        }

        public void WriteLog(Exception exception, string traceMethod)
        {
            if (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Desactivated, "d").ToString()) != -1)
                return;

            string currentFileName = GetLogFileName(DateTime.Now);

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            string userId = "NaN";
            Exception currentException = exception;

            /*
            §TraceDateTime•IP•URL•User•Method•TraceLevel•Message 
            Détail
            */
            DateTime traceDateTime = DateTime.Now;
            string traceIP = "127.0.0.1";
            string traceURL = "";

            try
            {
                //RETOURNER TODO SOULAYMA
                //traceIP = _HttpContext.Connection.RemoteIpAddress.ToString(); ; //Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                //traceURL = _HttpContext.Request.PathBase.ToString();
            }
            catch
            {

            }
            string traceUser = userId;
            string traceLevel = TraceLevel.Error.ToString();
            string traceMessage = currentException.Message;
            string traceDetail = currentException.StackTrace;

            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            errorMsg.Append(DATA_SEPARATOR_LOG_ROW);
            errorMsg.Append(traceDateTime);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceIP);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceURL);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceUser);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMethod);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceLevel);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMessage);

            //Ecrire dans le fichier log
            Trace.WriteLine(errorMsg.ToString());

            if ((TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Advanced, "d").ToString()) != -1)
                ||
                (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Verbose, "d").ToString()) != -1))
            {
                //Ecrire dans le fichier log
                Trace.WriteLine(traceDetail);
            }

            //vider la mémoire 
            Trace.Flush();
        }

        public void WriteLog(Exception exception)
        {
            if (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Desactivated, "d").ToString()) != -1)
                return;

            string currentFileName = GetLogFileName(DateTime.Now);

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            string userId = "NaN";
            Exception currentException = exception;

            /*
            §TraceDateTime•IP•URL•User•Method•TraceLevel•Message 
            Détail
            */
            DateTime traceDateTime = DateTime.Now;
            string traceIP = "127.0.0.1";
            string traceURL = "";

            try
            {
                traceIP = _HttpContext.Connection.RemoteIpAddress.ToString(); ; //Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                traceURL = _HttpContext.Request.PathBase.ToString();
            }
            catch
            {

            }
            string traceUser = userId;
            string traceLevel = TraceLevel.Error.ToString();
            string traceMessage = currentException.Message;
            string traceDetail = currentException.StackTrace;
            string traceMethod = currentException.TargetSite.ToString();
            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            errorMsg.Append(DATA_SEPARATOR_LOG_ROW);
            errorMsg.Append(traceDateTime);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceIP);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceURL);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceUser);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMethod);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceLevel);
            errorMsg.Append(DATA_SEPARATOR_LOG_COLUMN);
            errorMsg.Append(traceMessage);

            //Ecrire dans le fichier log
            Trace.WriteLine(errorMsg.ToString());

            if ((TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Advanced, "d").ToString()) != -1)
                ||
                (TRACE_LEVEL.IndexOf(Enum.Format(typeof(TRACELEVEL), TRACELEVEL.Verbose, "d").ToString()) != -1))
            {
                //Ecrire dans le fichier log
                Trace.WriteLine(traceDetail);
            }

            //vider la mémoire 
            Trace.Flush();
        }
    }
}
