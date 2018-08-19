using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;

namespace WebApplicationExercise.Loging
{
    public class NLogLogger : ITraceWriter
    {
        private static readonly Logger classLogger = LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> loggingMap =
            new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>>
                {
                    { TraceLevel.Info, classLogger.Info },
                    { TraceLevel.Error, classLogger.Error },
                    { TraceLevel.Debug, classLogger.Debug },
                    { TraceLevel.Warn,  classLogger.Warn }
                });

        private Dictionary<TraceLevel, Action<string>> Logger
        {
            get { return loggingMap.Value; }
        }

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level != TraceLevel.Off)
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                Log(record);
            }
        }

        private void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if (record.Request != null)
            {
                if (record.Request.Method != null)
                {
                    message.Append(record.Request.Method);
                }                   
            }

            if (!string.IsNullOrWhiteSpace(record.Category))
            {
                message.Append(" ").Append(record.Category);
            }
                
            if (record.RequestId != Guid.Empty)
            {
                message.Append(" ").Append(record.RequestId);
            }
                
            if (!string.IsNullOrWhiteSpace(record.Message))
            {
                message.Append(" ").Append(record.Message);
            }

            if (record.Exception != null && !string.IsNullOrWhiteSpace(record.Exception.GetBaseException().Message))
            {
                message.Append(" ").Append(record.Exception.GetBaseException().Message);
            }

            Logger[record.Level](message.ToString());
        }
    }
}