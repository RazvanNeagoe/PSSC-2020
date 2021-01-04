using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendConfirm
{
    public class ConfirmAcknowledgement
    {
        public ConfirmAcknowledgement(string received)
        {
            Received = received;
        }
        public string Received { get; private set; }
    }
}
