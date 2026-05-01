using System;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class ChatSessionMessageDTO
    {
        public bool IsUser { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
