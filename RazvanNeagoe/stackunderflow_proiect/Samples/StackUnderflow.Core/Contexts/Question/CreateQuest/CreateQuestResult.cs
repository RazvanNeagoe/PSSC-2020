using Access.Primitives.Extensions.Cloning;
using CSharp.Choices;
using StackUnderflow.DatabaseModel.Models;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.CreateQuest
{
    [AsChoice]

    public static partial class CreateQuestResult
    {
        public interface ICreateQuestResult : IDynClonable { }

        public class QuestCreated : ICreateQuestResult
            {
            public User Author { get; }
            public Post Question { get; }
            public QuestCreated(Post post, User author)
            {
                Question = post;
                Author = author;
            }

            public object Clone() => this.ShallowClone();
        }
        public class QuestNotCreated : ICreateQuestResult
        {
            public string Reason { get; private set; }

            ///TODO
            public object Clone() => this.ShallowClone();
        }

        public class InvalidQuest : ICreateQuestResult
        {
            public string Message { get; }

            public InvalidQuest(string message)
            {
                Message = message;
            }
            
            public object Clone() => this.ShallowClone();

        }
    }
}
