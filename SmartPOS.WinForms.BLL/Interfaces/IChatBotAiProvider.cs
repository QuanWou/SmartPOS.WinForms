namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IChatBotAiProvider
    {
        bool IsConfigured { get; }

        string Analyze(string question, string businessContext);
    }
}
