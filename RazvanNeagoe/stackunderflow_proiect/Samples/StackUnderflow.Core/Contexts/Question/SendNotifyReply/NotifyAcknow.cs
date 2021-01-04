using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply
{
    public class NotifyAcknow
    {
        public NotifyAcknow(string receipt)
        {
            Receipt = receipt;
        }
        public string Receipt { get; private set; }
    }
}
