using Access.Primitives.EFCore;
using Access.Primitives.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Access.Primitives.Extensions.ObjectExtensions;
using StackUnderflow.Domain.Core.Contexts.Question;
using StackUnderflow.Domain.Core.Contexts.Question.CreateQuest;
using StackUnderflow.Domain.Core.Contexts.Question.SendConfirm;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Orleans;
using Microsoft.AspNetCore.Http;
using GrainInterfaces;
using StackUnderflow.Domain.Core.Contexts.Question.CreateReply;
using StackUnderflow.Domain.Core.Contexts.Question.SendNotifyReply;

namespace StackUnderflow.API.AspNetCore.Controllers
{
    [ApiController]
    [Route("question")]
    public class QuestController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private readonly IClusterClient _client;

        public QuestController(IInterpreterAsync interpreter, StackUnderflowContext dbContext, IClusterClient client)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
            _client = client;

        }


        [HttpPost("post")]
        public async Task<IActionResult> CreateAndConfirmationQuestion([FromBody] CreateQuestCmd createQuestCmd)
        {
            QuestWrite ctx = new QuestWrite(
               new EFList<Post>(_dbContext.Post),
               new EFList<User>(_dbContext.User));

            var dependencies = new QuestDependencies();
            dependencies.GenerateConfirmationToken = () => Guid.NewGuid().ToString();
            dependencies.SendConfirmationEmail = SendEmail;

            var expr = from createQuestResult in QuestDomain.CreateQuestion(createQuestCmd)
                       let user = createQuestResult.SafeCast<CreateQuestResult.QuestCreated>().Select(p => p.Author)
                       let confirmQuestCmd = new ConfirmQuestCmd(user)
                       from ConfirmQuestResult in QuestDomain.ConfirmQuestion(confirmQuestCmd)
                       select new { createQuestResult, ConfirmQuestResult };
            var r = await _interpreter.Interpret(expr, ctx, dependencies);

            // _dbContext.Post.Add(new Post { PostTypeId=1,Title=createQuestCmd.Title, PostText=createQuestCmd.Body});
            await _dbContext.SaveChangesAsync();

            return r.createQuestResult.Match(
                created => (IActionResult)Ok(created.Question.PostId),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Question could not be created."),//todo return 500 (),
            invalidRequest => BadRequest("Invalid request."));

        }
        private TryAsync<ConfirmAcknowledgement> SendEmail(ConfirmLetter letter)
       => async () =>
       {
           var emialSender = _client.GetGrain<IEmailSender>(0);
           await emialSender.SendEmailAsync(letter.Letter);
           return new ConfirmAcknowledgement(Guid.NewGuid().ToString());
       };


        [HttpPost("question")]
        public async Task<IActionResult> CreateAndNotifyReply([FromBody] CmdReply createNotifyCmd)
        {
            QuestWrite ctx = new QuestWrite(
               new EFList<Post>(_dbContext.Post),
               new EFList<User>(_dbContext.User));

            var dependencies = new QuestDependencies();
            dependencies.GenerateConfirmationToken = () => Guid.NewGuid().ToString();
            dependencies.SendConfirmationEmail = SendEmail;

            var expr = from createReplyResult in QuestDomain.CreateReply(createNotifyCmd)
                       let user = createReplyResult.SafeCast<ResultReply.ReplyCreated>().Select(p => p.Author)
                       let notifyReplyCmd = new NotifyCmdReply(user)
                       from NotifyReplyResult in QuestDomain.NotifyReply(notifyReplyCmd)
                       select new { createReplyResult };
            var r = await _interpreter.Interpret(expr, ctx, dependencies);

            _dbContext.Post.Add(new Post { PostTypeId = 2, PostText = createNotifyCmd.Body, PostedBy = new Guid("f505c32f-3573-4459-8112-af8276d3e919"), PostId = createNotifyCmd.QuestionId });
            await _dbContext.SaveChangesAsync();
            
            return r.createReplyResult.Match(
                created => (IActionResult)Ok(created.Answer.PostId),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Reply could not be created."),//todo return 500 (),
            invalidRequest => BadRequest("Invalid request."));

        }

        private TryAsync<NotifyAcknow> SendNotifyEmail(NotifyLetter letter)
      => async () =>
      {
          var emialSender = _client.GetGrain<IEmailSender>(0);
          await emialSender.SendEmailAsync(letter.Letter);
          return new NotifyAcknow(Guid.NewGuid().ToString());
      };

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuestions()
        {
            //var questions= GetQuestionsFromDb();
            var questionsGrain = this._client.GetGrain<IQuestGain>("Id");
            var questions = await questionsGrain.GetQuestionsAsync();
            List<Object> all = (from x in questions select (Object)x).ToList();
            all.AddRange(questions);

            return Ok(all);
        }

    }
}
