using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IChatBotService
    {
        ChatBotResponse Ask(string question);
    }
}
