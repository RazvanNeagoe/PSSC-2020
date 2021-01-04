using Access.Primitives.Extensions.Cloning;
using CSharp.Choices;
using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.CreateReply
{
    [AsChoice]
    public static partial class ResultReply
    {
        public interface IResultReply : IDynClonable { }

        public class ReplyCreated : IResultReply
        {
            public User Author { get; }
            public Post Answer { get; }
            public ReplyCreated(Post answer, User author)
            {
                Answer = answer;
                Author = author;
            }

            public object Clone() => this.ShallowClone();
        }

        public class ReplyNotCreated : IResultReply
        {
            public string Reason { get; private set; }

            ///TODO
            public object Clone() => this.ShallowClone();
        }

        public class InvalidReply : IResultReply
        {
            public string Message { get; }

            public InvalidReply(string message)
            {
                Message = message;
            }

            ///TODO
            public object Clone() => this.ShallowClone();

        }
    }
}
