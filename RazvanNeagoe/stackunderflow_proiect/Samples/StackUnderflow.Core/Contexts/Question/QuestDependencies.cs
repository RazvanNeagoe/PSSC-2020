using LanguageExt;
using StackUnderflow.Domain.Core.Contexts.Question.SendConfirm;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question
{
    public class QuestDependencies
    {
        public Func<string> GenerateConfirmationToken { get; set; }
        public Func<ConfirmLetter, TryAsync<ConfirmAcknowledgement>> SendConfirmationEmail { get; set; }
    }
}
