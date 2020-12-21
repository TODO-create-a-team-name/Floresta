﻿using System.ComponentModel.DataAnnotations;

namespace Floresta.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string QuestionText { get; set; }
        // TODO: add bool variable isAnswered
        public string UserId { get; set; }
        public User User { get; set; }
    }
}