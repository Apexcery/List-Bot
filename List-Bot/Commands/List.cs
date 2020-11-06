using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace List_Bot.Commands
{
    public class List : ModuleBase<SocketCommandContext>
    {
        [Command("create")]
        public async Task CreateList(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                await ReplyAsync("You need to specify a name for the new list.");
                return;
            }

            if (!Directory.Exists("./appdata"))
            {
                await ReplyAsync("'appdata' directory does not exist.");
                return;
            }
            if (!File.Exists("./appdata/lists.json"))
            {
                await File.WriteAllTextAsync("./appdata/lists.json", "[]");
            }
        }
    }
}
