namespace ThsCrmSample.Core
{
    using System;

    using Microsoft.Xrm.Sdk;

    internal class Logger : ILogger
    {
        private readonly ITracingService tracer;

        public Logger(ITracingService tracer)
        {
            this.tracer = tracer;
        }

        public void Information(string message)
        {
            this.tracer.Trace($"[INFO]  {DateTime.UtcNow.ToString("O")}: {message}");
        }

        public void Warning(string message)
        {
            this.tracer.Trace($"[WARN]  {DateTime.UtcNow.ToString("O")}: {message}");
        }

        public void Error(string message)
        {
            this.tracer.Trace($"[ERROR] {DateTime.UtcNow.ToString("O")}: {message}");
        }

        public void Error(Exception exception)
        {
            var message = $"{exception.Message}\r\n{exception.StackTrace}";
            this.AddInnerExceptionToMessage(exception, ref message);
            this.Error(message);
        }

        private void AddInnerExceptionToMessage(Exception exception, ref string message)
        {
            if (exception.InnerException == null)
            {
                return;
            }

            message += $"\r\nInner Exception: {exception.InnerException.Message}\r\n{exception.InnerException.StackTrace}";
            this.AddInnerExceptionToMessage(exception.InnerException, ref message);
        }
    }
}
