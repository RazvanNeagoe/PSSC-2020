using Access.Primitives.IO;
using StackUnderflow.Domain.Core.Contexts.Question.CreateQuest;
using StackUnderflow.Domain.Core.Contexts.Question.CreateReply;
using StackUnderflow.Domain.Core.Contexts.Question.SendConfirm;
using StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply;
using System;
using System.Collections.Generic;
using System.Text;
using static PortExt;
using static StackUnderflow.Domain.Core.Contexts.Question.CreateQuest.CreateQuestResult;
using static StackUnderflow.Domain.Core.Contexts.Question.CreateReply.ResultReply;
using static StackUnderflow.Domain.Core.Contexts.Question.SendConfirm.ConfirmQuestResult;
using static StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply.NotifyResultReply;

namespace StackUnderflow.Domain.Core.Contexts.Question
{
   public static class QuestDomain
    {
        public static Port<ICreateQuestResult> CreateQuestion(CreateQuestCmd command) => NewPort<CreateQuestCmd, ICreateQuestResult>(command);
        public static Port<IConfirmQuestResult> ConfirmQuestion(ConfirmQuestCmd command) => NewPort<ConfirmQuestCmd, IConfirmQuestResult>(command);
        public static Port<IResultReply> CreateReply(CmdReply command) => NewPort<CmdReply, IResultReply>(command);

        public static Port<INotifyResultReply> NotifyReply(NotifyCmdReply command) => NewPort<NotifyCmdReply, INotifyResultReply>(command);

    }
}
