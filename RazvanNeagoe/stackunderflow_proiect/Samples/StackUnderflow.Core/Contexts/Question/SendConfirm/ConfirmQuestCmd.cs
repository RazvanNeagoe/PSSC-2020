using Access.Primitives.IO;
using EarlyPay.Primitives.ValidationAttributes;
using LanguageExt;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendConfirm
{
    public struct ConfirmQuestCmd
    {
        [OptionValidator(typeof(RequiredAttribute))]
        public Option<User> QuestionUser { get; }
        public ConfirmQuestCmd(Option<User> questionUser)
        {
            QuestionUser = questionUser;
        }
    }
    public enum ConfirmQuestCmdInput
    {
        Valid,
        UserIsNone
    }

    public class ConfirmQuestCmdInputGen : InputGenerator<ConfirmQuestCmd, ConfirmQuestCmdInput>
    {
        public ConfirmQuestCmdInputGen()
        {
            mappings.Add(ConfirmQuestCmdInput.Valid, () =>
             new ConfirmQuestCmd(
                 Option<User>.Some(new User()
                 {
                     DisplayName = Guid.NewGuid().ToString(),
                     Email = $"{Guid.NewGuid()}@mailinator.com"
                 }))
            );

            mappings.Add(ConfirmQuestCmdInput.UserIsNone, () =>
                new ConfirmQuestCmd(
                    Option<User>.None
                    )
            );
        }
    }

}
