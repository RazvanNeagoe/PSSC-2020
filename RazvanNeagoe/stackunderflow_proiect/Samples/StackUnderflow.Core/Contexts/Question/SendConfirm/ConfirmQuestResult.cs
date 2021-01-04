using CSharp.Choices;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.SendConfirm
{
    [AsChoice]
    public static partial class ConfirmQuestResult
    {
        public interface IConfirmQuestResult { }

        public class QuestConfirmed : IConfirmQuestResult
        {
            public User QuestionUser { get; }

            public string InvitationAcknowlwedgement { get; set; }

            public QuestConfirmed(User adminUser, string invitationAcknowledgement)
            {
                QuestionUser = adminUser;
                InvitationAcknowlwedgement = invitationAcknowledgement;
            }
        }
        public class QuestNotConfirmed : IConfirmQuestResult
        {
           
        }

        public class InvalidRequest : IConfirmQuestResult
        {
            public string Message { get; }

            public InvalidRequest(string message)
            {
                Message = message;
            }

        }
    }
}
