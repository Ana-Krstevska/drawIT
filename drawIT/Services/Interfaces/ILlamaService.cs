namespace drawIT.Services.Interfaces
{
    public interface ILlamaService
    {
        Task SendPromptToLlamaApiAsync(string prompt);
    }
}
