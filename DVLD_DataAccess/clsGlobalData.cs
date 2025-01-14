using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsGlobalData
    {


        public static void WriteEventLog(Exception ex, string FunctionName)
        {
            string Source = "DVLDApp";

            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, "Application");
            }

            EventLog.WriteEntry(Source, "Error accured in function : " + FunctionName + "Message:" + ex.Message,EventLogEntryType.Error);
        }

        public static void LogError(Exception ex)
            {
                // Specify the source name for the event log
                string sourceName = "DVLD";

                // Create the event source if it does not exist
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName,"Application");
                }

                string errorMessage = $"Error : {ex.Source}\n\nException Message:" +
                        $" {ex.Message}\n\nException Type: {ex.GetType().Name}\n\nStack Trace:" +
                        $" {ex.StackTrace}\n\nException Location: {ex.TargetSite}";

                // Log an error event
                EventLog.WriteEntry(sourceName, errorMessage, EventLogEntryType.Error);
            }
     }

}

