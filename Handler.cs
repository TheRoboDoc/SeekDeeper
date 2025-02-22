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

            DiscordUser? mentioned = args.MentionedUsers.Where(x => x == client.CurrentUser).First();

            if (mentioned == null)
            {
                return;
            }

            await args.Message.RespondAsync("I was mentioned!");
        }
    }
}
