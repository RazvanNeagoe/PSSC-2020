using Access.Primitives.IO;
using EarlyPay.Primitives.ValidationAttributes;
using LanguageExt;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply
{
    public struct NotifyCmdReply
    {
        [OptionValidator(typeof(RequiredAttribute))]
        public Option<User> QuestionUser { get; }
        public NotifyCmdReply(Option<User> questionUser)
        {
            QuestionUser = questionUser;
        }
    }

    public enum NotifyCmdReplyInput
    {
        Valid,
        Invalid
    }
    public class NotifyCmdReplyInputGen : InputGenerator<NotifyCmdReply, NotifyCmdReplyInput>
    {
        public NotifyCmdReplyInputGen()
        {
            mappings.Add(NotifyCmdReplyInput.Valid, () =>
             new NotifyCmdReply(
                 Option<User>.Some(new User()
                 {
                     DisplayName = Guid.NewGuid().ToString(),
                     Email = $"{Guid.NewGuid()}@mailinator.com"
                 }))
            );

            mappings.Add(NotifyCmdReplyInput.Invalid, () =>
                new NotifyCmdReply(
                    Option<User>.None
                    )
            );
        }
    }
}
