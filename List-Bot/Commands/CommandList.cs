using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using List_Bot.Data;
using List_Bot.Models;
using List_Bot.Services;
using Microsoft.EntityFrameworkCore;

namespace List_Bot.Commands
{
    public class CommandList : ModuleBase<SocketCommandContext>
    {
        private readonly IDatabaseOperations _dbOperations;

        public CommandList(IDatabaseOperations dbOperations)
        {
            _dbOperations = dbOperations;
        }

        [Command("create"), Alias("new")]
        public async Task CreateList(string name, [Remainder]string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                await ReplyAsync("You need to specify a name and a description for the new list.");
                return;
            }

            var currentDate = DateTime.UtcNow;

            var newList = new List
            {
                Name = name,
                Description = description,
                CreationDate = currentDate,
                UpdatedDate = currentDate
            };

            var success = await _dbOperations.AddNewList(newList);
            if (!success)
            {
                await ReplyAsync("List could not be created.");
                return;
            }

            await ReplyAsync($"List: '{name}' successfully created.");
        }

        [Command("delete"), Alias("del")]
        public async Task DeleteList(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                await ReplyAsync("You need to specify a name for the list to remove.");
                return;
            }

            var success = await _dbOperations.DeleteListByName(name);
            if (!success)
            {
                await ReplyAsync("List could not be deleted.");
                return;
            }

            await ReplyAsync($"List: '{name}' successfully deleted.");
        }

        [Command("viewall")]
        public async Task ViewAllLists()
        {
            var lists = await _dbOperations.GetAllLists();

            if (!lists.Any())
            {
                await ReplyAsync("No lists have been created yet.");
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle("Lists")
                .WithColor(Colors.GetRandomDiscordColor());

            lists.ForEach(list => embed.AddField(list.Name, list.Description));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
