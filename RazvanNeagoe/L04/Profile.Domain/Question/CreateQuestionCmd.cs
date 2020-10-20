using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Profile.Domain.Question
{
    public struct CreateQuestionCmd
    {
        [Required]
        public string Title { get; private set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Tags { get; private set; }
        
        

        public CreateQuestionCmd(string title, string body, string tags)
        {
            Title = title;
            Body = body;
            Tags = tags;

        }
    }
}
