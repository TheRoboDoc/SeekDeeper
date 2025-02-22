using DSharpPlus;

namespace SeekDeeper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string? token = Environment.GetEnvironmentVariable("SeekDeepToken");

            if (token == null)
            {
                return;
            }

            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault
            (
                token: token,
                intents: DiscordIntents.GuildMessages | DiscordIntents.MessageContents
            );

            builder.ConfigureEventHandlers
            (
                _event => _event.HandleMessageCreated(async (_client, _args) => await Handler.Run(_client, _args))
            );

            DiscordClient client = builder.Build();

            await client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
