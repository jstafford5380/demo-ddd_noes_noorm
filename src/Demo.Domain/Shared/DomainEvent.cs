using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Shared
{
    public class DomainEvent
    {

    }

    public class ErrorEvent : DomainEvent
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }

        public int DomainCode { get; set; }

        public ErrorEvent() { }

        public ErrorEvent(int domainCode, Exception exception)
        {
            Message = exception.Message;
            DomainCode = domainCode;
        }
    }
}
