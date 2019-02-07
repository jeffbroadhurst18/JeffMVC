using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace JeffMVC.Filters
{
	public class LoggingAttribute : ActionFilterAttribute
	{
		private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

		public LoggingAttribute()
		{

		}

		public override void OnResultExecuted(ResultExecutedContext context)
		{
		
			_logger.Log(NLog.LogLevel.Debug, context.ActionDescriptor.DisplayName);
			var returnData = ((Microsoft.AspNetCore.Mvc.ViewResult)context.Result).ViewData;

			var conc = string.Empty;
			foreach(var item in returnData)
			{
				conc += $"({item.Key},{item.Value})";
			}

			_logger.Log(NLog.LogLevel.Debug, $"Parameters = {conc}");
			base.OnResultExecuted(context);
		}
	}
}
