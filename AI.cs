using OllamaSharp;

namespace SeekDeeper
{
    internal class AI
    {
        private Uri ApiAddress;

        private OllamaApiClient OllamaClient;

        public AI(string url = "http://localhost:11434")
        {
            ApiAddress = new Uri(url);
            OllamaClient = new OllamaApiClient(ApiAddress);

            OllamaClient.SelectedModel = "deepseek-r1:8b";
        }

        public async Task<string> GenerateResponse(string input/*, DiscordChannel channel*/)
        {
            return OllamaClient.GenerateAsync(input).StreamToEndAsync().Result.Response;
        }
    }
}
