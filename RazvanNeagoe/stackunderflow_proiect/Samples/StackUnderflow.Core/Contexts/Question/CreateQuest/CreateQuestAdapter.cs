using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.IO.Mocking;
using StackUnderflow.Domain.Core.Contexts.Question.CreateQuest;
using StackUnderflow.DatabaseModel.Models;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Question.CreateQuest.CreateQuestResult;

namespace StackUnderflow.Domain.Core.Contexts.Question
{
    public partial class CreateQuestAdapter : Adapter<CreateQuestCmd, ICreateQuestResult, QuestWrite, QuestDependencies>
    {
        private readonly IExecutionContext _ex;

        public CreateQuestAdapter(IExecutionContext ex)
        {
            _ex = ex;
        }

        public override async Task<ICreateQuestResult> Work(CreateQuestCmd command, QuestWrite state, QuestDependencies dependencies)
        {
            var workflow = from valid in command.TryValidate()
                           let t = AddQuestionIfMissing(state, CreateQuestFromCommand(command))
                           select t;
            var result = await workflow.Match(
                Succ: r => r,
                Fail: ex => new InvalidQuest(ex.ToString()));
            return result;
        }

        public ICreateQuestResult AddQuestionIfMissing(QuestWrite state, Post post)
        {
            if (state.Questions.Any(p => p.Title.Equals(post.Title)))
                return new QuestNotCreated();
            if (state.Questions.All(p => p.PostId != post.PostId))
                state.Questions.Add(post);
            return new QuestCreated(post,post.TenantUser.User);
        }

        private Post CreateQuestFromCommand(CreateQuestCmd cmd)
        {
            var question = new Post()
            {
                Title = cmd.Title,
                PostText = cmd.Body,
                PostTag = cmd.Tags
            };
            return question;
        }

        public override Task PostConditions(CreateQuestCmd op, CreateQuestResult.ICreateQuestResult result, QuestWrite state)
        {
            return Task.CompletedTask;
        }

    }
}
