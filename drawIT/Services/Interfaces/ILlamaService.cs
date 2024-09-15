namespace drawIT.Services.Interfaces
{
    public interface ILlamaService
    {
        Task<string> SendPromptToLlamaApiAsync(string prompt);
    }
}
