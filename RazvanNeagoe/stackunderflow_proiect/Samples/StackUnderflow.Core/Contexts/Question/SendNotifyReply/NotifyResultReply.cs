using CSharp.Choices;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply
{
    [AsChoice]
    public static partial class NotifyResultReply
    {
        public interface INotifyResultReply { }

        public class ReplyConfirmed : INotifyResultReply
        {
            public User QuestionUser { get; }

            public string InvitationAcknowlwedgement { get; set; }

            public ReplyConfirmed(User adminUser, string invitationAcknowledgement)
            {
                QuestionUser = adminUser;
                InvitationAcknowlwedgement = invitationAcknowledgement;
            }
        }

        public class ReplyNotConfirmed : INotifyResultReply
        {
            ///TODO
        }

        public class InvalidRequest : INotifyResultReply
        {
            public string Message { get; }

            public InvalidRequest(string message)
            {
                Message = message;
            }

        }
    }
}
