using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Question.SendConfirm.ConfirmQuestResult;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendConfirm
{
    public partial class ConfirmQuestAdapter : Adapter<ConfirmQuestCmd, IConfirmQuestResult, QuestWrite, QuestDependencies>
    {
        private readonly IExecutionContext _ex;

        public ConfirmQuestAdapter(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override async Task<IConfirmQuestResult> Work(ConfirmQuestCmd command, QuestWrite state, QuestDependencies dependencies)
        {
            var wf = from isValid in command.TryValidate()
                     from user in command.QuestionUser.ToTryAsync()
                     let letter = GenerateConfirmationLetter(user)
                     from confirmationAck in dependencies.SendConfirmationEmail(letter)
                     select (user, confirmationAck);

            return await wf.Match(
                Succ: r => new QuestConfirmed(r.user, r.confirmationAck.Received),
                Fail: ex => (IConfirmQuestResult)new InvalidRequest(ex.ToString()));
        }
        private ConfirmLetter GenerateConfirmationLetter(User user)
        {
            var link = $"https://stackunderflow/Question986";
            var letter = $@"Dear {user.DisplayName} your question is posted. For details please click on {link}";
            return new ConfirmLetter(user.Email, letter, new Uri(link));
        }
        public override Task PostConditions(ConfirmQuestCmd cmd, IConfirmQuestResult result, QuestWrite state)
        {
            return Task.CompletedTask;
        }
    }
}
