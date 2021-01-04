using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GrainImplementation
{
    class QuestGrain : Grain
    {
        private StackUnderflowContext _dbContext;
        private QuestGrain state;
        private IList<Post> _questions;

        public QuestGrain(StackUnderflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnActivateAsync()
        {
            var key = this.GetPrimaryKey();
            Post post = new Post();

            var expPostId = _questions.Where(p => p.PostId.Equals(key.ToString()));

            var expParentPostId = _questions.Where(p => p.ParentPostId.Equals(key.ToString()));

            if (expPostId == null && expParentPostId == null)
            {
                //nu exista inregistrari
            }
            else
            {
                // subscribe to replys stream
                var streamProvider = GetStreamProvider("SMSProvider");
                var stream = streamProvider.GetStream<Post>(Guid.Empty, "questions");
                await stream.SubscribeAsync((IAsyncObserver<Post>)this);
            }

            //return base.OnActivateAsync();
        }

        public async Task<IList<Post>> GetQuestionWithReplys()
        {
            return (IList<Post>)_dbContext.Post.Where(p => p.PostTypeId == 2).ToListAsync();
        }
    }
}
