namespace ThsCrmSample.Core
{
    using System;

    public interface ILogger
    {
        void Information(string message);

        void Warning(string message);

        void Error(string message);

        void Error(Exception exception);
    }
}
