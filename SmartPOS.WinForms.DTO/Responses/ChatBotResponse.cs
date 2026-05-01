using System;
using System.Collections.Generic;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotResponse
    {
        public string Intent { get; set; }

        public string Answer { get; set; }

        public List<string> SuggestedQuestions { get; set; }

        public DateTime GeneratedAt { get; set; }
    }
}
