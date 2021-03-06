﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using List_Bot.Services;

namespace List_Bot.Commands
{
    public class CommandInfo : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            var prefix = Environment.GetEnvironmentVariable("prefix");

            var embed = new EmbedBuilder()
                .WithTitle("Usage")
                .AddField("Create List", $"{prefix}create <list name>\nCreate a new list with the specified name.")
                .AddField("Add to List", $"{prefix}add <list name> <item>\nAdd a new item to the specified list.")
                .AddField("Pick Random from List", $"{prefix}random <list name>")
                .AddField("Remove from List", $"{prefix}remove <list name> <item>\nRemove the specified item from the specified list.")
                .AddField("View List", $"{prefix}view <list name>\nView the entire list.")
                .AddField("Delete List", $"{prefix}delete <list name>\nDelete the specified list.")
                .AddField("View all Lists", $"{prefix}viewall\nView all created lists.")
                .WithColor(Colors.GetRandomDiscordColor())
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
