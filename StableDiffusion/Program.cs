using DSharpPlus;
using DSharpPlus.CommandsNext;
using StableDiffusion.Commands;
using System.Diagnostics;
using dotenv.net;
using DSharpPlus.Entities;

namespace StableDiffusion
{
    class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load();
            DiscordClientAsync().GetAwaiter().GetResult();
        }

        private static async Task DiscordClientAsync()
        {
            DiscordConfiguration discordConfiguration = new DiscordConfiguration()
            {
                AutoReconnect = true,
                Intents = DiscordIntents.All,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot
            };

            CommandsNextConfiguration commandsNextConfiguration = new CommandsNextConfiguration()
            {
                EnableDms = false,
                EnableDefaultHelp = false,
                StringPrefixes= new string[] { "~" },
                EnableMentionPrefix = true,
            };

            DiscordClient discordClient = new DiscordClient(discordConfiguration);
            CommandsNextExtension commands = discordClient.UseCommandsNext(commandsNextConfiguration);
            commands.RegisterCommands<DefaultCommands>();
            await discordClient.ConnectAsync(_discordActivity);
            await Task.Delay(-1);

            discordClient.Ready += DiscordClient_Ready;
        }

        private static DiscordActivity _discordActivity = new DiscordActivity(name: "Nazrin", type: ActivityType.Watching);
        private async static Task DiscordClient_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            Console.WriteLine(await sender.GetUserAsync(sender.CurrentUser.Id));
            Console.WriteLine(sender.CurrentUser.Username);
        }
    }
}