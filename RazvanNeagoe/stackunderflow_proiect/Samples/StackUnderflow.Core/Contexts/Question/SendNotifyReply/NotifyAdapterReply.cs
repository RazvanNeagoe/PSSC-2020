using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using StackUnderflow.Domain.Core.Contexts.Question.CreateReply;
using StackUnderflow.Domain.Core.Contexts.Question.SendConfirm;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Question.CreateReply.ResultReply;
using static StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply.NotifyResultReply;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply
{
    public partial class NotifyAdapterReply : Adapter<NotifyCmdReply, INotifyResultReply, QuestWrite, QuestDependencies>
    {
        private readonly IExecutionContext _ex;

        public NotifyAdapterReply(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override async Task<INotifyResultReply> Work(NotifyCmdReply command, QuestWrite state, QuestDependencies dependencies)
        {
            var wf = from isValid in command.TryValidate()
                     from user in command.QuestionUser.ToTryAsync()
                     let letter = GenerateConfirmationLetter(user)
                     from confirmationAck in dependencies.SendConfirmationEmail(letter)
                     select (user, confirmationAck);

            return await wf.Match(
                Succ: r => new ReplyConfirmed(r.user, r.confirmationAck.Received),
                Fail: ex => (INotifyResultReply)new InvalidRequest(ex.ToString()));
        }

        private ConfirmLetter GenerateConfirmationLetter(User user)
        {
            var link = $"https://stackunderflow/Question986";
            var letter = $@"Dear {user.DisplayName} your reply is posted. For more details please click on {link}";
            return new ConfirmLetter(user.Email, letter, new Uri(link));
        }

        public override Task PostConditions(NotifyCmdReply cmd, INotifyResultReply result, QuestWrite state)
        {
            return Task.CompletedTask;
        }
    }
}
