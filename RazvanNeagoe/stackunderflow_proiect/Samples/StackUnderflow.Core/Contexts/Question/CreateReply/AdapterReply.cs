using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Question.CreateReply.ResultReply;

namespace StackUnderflow.Domain.Core.Contexts.Question.CreateReply
{
    public partial class AdapterReply : Adapter<CmdReply, IResultReply, QuestWrite, QuestDependencies>
    {
        private readonly IExecutionContext _ex;

        public AdapterReply(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override async Task<IResultReply> Work(CmdReply command, QuestWrite state, QuestDependencies dependencies)
        {
            var workflow = from valid in command.TryValidate()
                           let t = AddReplyIfMissing(state, CreateReplyFromCommand(command))
                           select t;
            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new InvalidReply(ex.ToString()));
            return result;
        }

        public IResultReply AddReplyIfMissing(QuestWrite state, Post post)
        {
            if (state.Questions.Any(p => p.PostId.Equals(post.PostId)))
                return new ReplyNotCreated();
            if (state.Questions.All(p => p.PostId != post.PostId))
                state.Questions.Add(post);
            return new ReplyCreated(post, post.TenantUser.User);
        }

        private Post CreateReplyFromCommand(CmdReply cmd)
        {
            var reply = new Post()
            {
                PostId = cmd.QuestionId,
                PostedBy = cmd.UserId,
                PostText = cmd.Body
            };
            return reply;
        }

        public override Task PostConditions(CmdReply cmd, IResultReply result, QuestWrite state)
        {
            return Task.CompletedTask;
        }
    }
}
