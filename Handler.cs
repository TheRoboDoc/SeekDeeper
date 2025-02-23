using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace SeekDeeper
{
    internal static class Handler
    {
        public static async Task Run(DiscordClient client, MessageCreatedEventArgs args)
        {
            if (args.Author.IsBot || args.Author == client.CurrentUser)
            {
                return;
            }

            DiscordUser? mentioned = args.MentionedUsers.Where(x => x == client.CurrentUser).FirstOrDefault();

            if (mentioned == null)
            {
                return;
            }

            AI ai = new("http://192.168.0.12:11434");

            bool typing = true;

            _ = Task.Run(async () =>
            {
                while (typing)
                {
                    await args.Channel.TriggerTypingAsync();

                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            });

            string? responseRaw = await ai.GenerateResponse(args.Message, args.Channel);

            typing = false;

            if (responseRaw == null)
            {
                await args.Message.RespondAsync("System: Failed to generate a response");
            }

            string respnse = responseRaw.Split("</think>")[1];

            await args.Message.RespondAsync(respnse);
        }
    }
}
