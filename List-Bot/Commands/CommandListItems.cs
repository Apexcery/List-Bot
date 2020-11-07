using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using List_Bot.Data;
using List_Bot.Models;
using List_Bot.Services;

namespace List_Bot.Commands
{
    public class CommandListItems : ModuleBase<SocketCommandContext>
    {
        private readonly IDatabaseOperations _dbOperations;

        public CommandListItems(IDatabaseOperations dbOperations)
        {
            _dbOperations = dbOperations;
        }

        [Command("add")]
        public async Task AddToList(string listName, [Remainder]string item)
        {
            if (string.IsNullOrEmpty(listName) || string.IsNullOrEmpty(item))
            {
                await ReplyAsync("You need to specify the name of the list and an item to add.");
                return;
            }

            var list = await _dbOperations.FindByName(listName);
            if (list == null)
            {
                await ReplyAsync("List could not be found with that name.");
                return;
            }

            var newListItem = new ListItem
            {
                Value = item,
                CreatedDate = DateTime.UtcNow
            };

            var success = await _dbOperations.AddToList(list, newListItem);
            if (!success)
            {
                await ReplyAsync($"'{item}' could not be added to '{listName}'.");
                return;
            }

            await ReplyAsync($"'{item}' successfully added to list: '{listName}'");
        }

        [Command("remove")]
        public async Task RemoveFromList(string listName, string itemName)
        {
            if (string.IsNullOrEmpty(listName) || string.IsNullOrEmpty(itemName))
            {
                await ReplyAsync("You need to specify the name of the list and an item to remove.");
                return;
            }

            var list = await _dbOperations.FindByName(listName);
            if (list == null)
            {
                await ReplyAsync("List could not be found with that name.");
                return;
            }

            if (!list.Items.Any(x => x.Value.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)))
            {
                await ReplyAsync("Specified item could not be found in the specified list.");
                return;
            }

            var item = list.Items.First(x => x.Value.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));

            var success = await _dbOperations.RemoveFromList(list, item);
            if (!success)
            {
                await ReplyAsync($"'{itemName}' could not be removed from '{listName}'.");
                return;
            }

            await ReplyAsync($"'{itemName}' successfully removed from list: '{listName}'");
        }

        [Command("view")]
        public async Task ViewListItems(string listName)
        {
            if (string.IsNullOrEmpty(listName))
            {
                await ReplyAsync("You need to specify a list to view.");
                return;
            }

            var list = await _dbOperations.FindByName(listName);
            if (list == null)
            {
                await ReplyAsync("A list with that name could not be found.");
                return;
            }

            var items = list.Items;
            
            var embed = new EmbedBuilder()
                .WithTitle(list.Name)
                .WithDescription($"{list.Description}\n({items.Count} items)")
                .AddField("Items:", string.Join(",\n", items.Select(item => item.Value)))
                .WithColor(Colors.GetRandomDiscordColor());

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
