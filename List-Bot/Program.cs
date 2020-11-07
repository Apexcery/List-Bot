using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using List_Bot.Data;

namespace List_Bot
{
    internal class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddScoped<IDatabaseOperations, DatabaseOperations>()
                .AddEntityFrameworkSqlite()
                .AddDbContext<DatabaseContext>()
                .BuildServiceProvider();

            await CreateDatabase();

            await RegisterCommandsAsync();

            var token = Environment.GetEnvironmentVariable("token");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task CreateDatabase()
        {
            await using var client = new DatabaseContext();
            await client.Database.EnsureCreatedAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message?.Author == null || message.Author.IsBot)
                return;

            var argPos = 0;
            var prefix = Environment.GetEnvironmentVariable("prefix");
            
            string prefix2 = null;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("prefix2")))
            {
                prefix2 = Environment.GetEnvironmentVariable("prefix2");
            }

            if (message.HasStringPrefix(prefix, ref argPos) || (prefix2 != null && message.HasStringPrefix(prefix2, ref argPos)))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
