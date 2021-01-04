using StackUnderflow.Domain.Schema.Models;
using StackUnderflow.EF.Models;
using StackUnderflow.DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question
{
    public class QuestWrite
    {
        public QuestWrite(ICollection<Post> questions, ICollection<User> users)
        {
            Questions = questions ?? new List<Post>(0);
            Users = users ?? new List<User>(0);
        }

        public ICollection<Post> Questions { get; }
        public ICollection<User> Users { get; }

    }
}
