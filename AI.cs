using DSharpPlus.Entities;
using OllamaSharp;
using OllamaSharp.Models.Chat;

namespace SeekDeeper
{
    internal class AI
    {
        private Uri ApiAddress;

        private OllamaApiClient OllamaClient;

        public AI(string url = "http://localhost:11434")
        {
            ApiAddress = new Uri(url);

            OllamaClient = new OllamaApiClient(ApiAddress)
            {
                SelectedModel = "deepseek-r1:8b"
            };
        }

        public async Task<string?> GenerateResponse(DiscordMessage triggerMessage, DiscordChannel channel)
        {
            return await Task.Run(() =>
            {
                List<DiscordMessage> rawMessages = channel.GetMessagesBeforeAsync(triggerMessage.Id, limit: 20).ToBlockingEnumerable().ToList();

                List<Message> messages = [];

                string setupMessage =
                    """
                    You are SeekDeep. A Discord chat bot.
                    You are direct and reply shortly and promptly.
                    Your username: SeekDeeper
                    Your ID: 1342955845817733241
                    Messages will be provided to you in the following standard:
                    {username} | {userID}: {messageContent}

                    You reply without this formatting.
                    To mention a user use this format: <@{userID}>
                    """;

                messages.Add
                (
                    new Message(ChatRole.System, setupMessage)
                );

                foreach (DiscordMessage rawMessage in rawMessages)
                {
                    if (rawMessage.Author == null)
                    {
                        continue;
                    }

                    if (rawMessage.Author.IsCurrent)
                    {
                        messages.Add(new Message(ChatRole.Assistant, $"{rawMessage.Author.Username} | {rawMessage.Author.Id}: {rawMessage.Content}"));
                    }
                    else
                    {
                        messages.Add(new Message(ChatRole.User, $"{rawMessage.Author.Username} | {rawMessage.Author.Id}: {rawMessage.Content}"));
                    }
                }

                messages.Add(new Message(ChatRole.User, $"{triggerMessage.Author?.Username} | {triggerMessage.Author?.Id}: {triggerMessage.Content}"));
                messages.Add(new Message(ChatRole.System, "As SeekDeeper, generate a response to the user"));

                ChatRequest chatRequest = new()
                {
                    Messages = messages,
                    Options = new OllamaSharp.Models.RequestOptions()
                    {
                        Temperature = 1,
                        FrequencyPenalty = 1.1F,
                        PresencePenalty = 1,
                    }
                };

                return OllamaClient.ChatAsync(chatRequest).StreamToEndAsync().Result?.Message.Content;
            });
        }
    }
}
