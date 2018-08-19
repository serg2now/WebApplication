using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;

namespace WebApplicationExercise.Loging
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        private DateTime startTime;
        private ITraceWriter _nLogLogger;

        public LoggingFilterAttribute(ITraceWriter logger)
        {
            _nLogLogger = logger;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionName = actionContext.ActionDescriptor.ActionName;

            startTime = DateTime.Now;

            _nLogLogger.Info(actionContext.Request,null, $"{actionName} started");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            var finishTime = DateTime.Now;
            var duration = new TimeSpan(finishTime.Ticks - startTime.Ticks).TotalMilliseconds;

            _nLogLogger.Info(actionExecutedContext.Request, null, $"{actionName} finished, duration : {duration} ms.");
        }
    }
}