using Profile.Domain.Question;
using System;
using System.Collections.Generic;
using System.Net;
using static Profile.Domain.Question.CreateQuestionResult;

namespace Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
           
    var cmd1 = new CreateQuestionCmd("How can I get the internal error message to log?", "c++");
    var result = CreateQuestion(cmd1);

 result.Match(
         ProcessQuestionCreated,
         ProcessQuestionNotCreated,
         ProcessInvalidQuestion
     );

 Console.ReadLine();
}

private static ICreateQuestionResult ProcessInvalidQuestion(QuestionValidationFailed validationErrors)
{
 Console.WriteLine("Question validation failed: ");
 foreach (var error in validationErrors.ValidationErrors)
 {
     Console.WriteLine(error);
 }
 return validationErrors;
}

private static ICreateQuestionResult ProcessQuestionNotCreated(QuestionNotCreated questionNotCreatedResult)
{
 Console.WriteLine($"QUestion not created: {questionNotCreatedResult.Reason}");
 return questionNotCreatedResult;
}

private static ICreateQuestionResult ProcessQuestionCreated(QuestionCreated question)
{
 Console.WriteLine($"Question {question.QuestionId}");
 return question;
}

public static ICreateQuestionResult CreateQuestion(CreateQuestionCmd createQuestionCommand)
{
 if (string.IsNullOrWhiteSpace(createQuestionCommand.Body))
 {
     var errors = new List<string>() { "Invalid body content" };
     return new QuestionValidationFailed(errors);
 }

 if(new Random().Next(10) > 1)
 {
     return new QuestionNotCreated("Body content could not be verified");
 }

 var questionId = Guid.NewGuid();
 var result = new QuestionCreated(questionId, createQuestionCommand.Body);

 return result;

}
}
}
