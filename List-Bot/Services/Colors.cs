using System;
using System.Collections.Generic;
using Discord;

namespace List_Bot.Services
{
    public class Colors
    {
        public static Color GetRandomDiscordColor()
        {
            var allColors = new List<Color>
            {
                Color.Blue,
                Color.DarkBlue,
                Color.DarkGreen,
                Color.DarkGrey,
                Color.DarkMagenta,
                Color.DarkOrange,
                Color.DarkPurple,
                Color.DarkRed,
                Color.DarkTeal,
                Color.DarkerGrey,
                Color.Default,
                Color.Gold,
                Color.Green,
                Color.LightGrey,
                Color.LightOrange,
                Color.Magenta,
                Color.Orange,
                Color.Purple,
                Color.Red,
                Color.Teal
            };

            var randomColor = allColors[new Random().Next(0, allColors.Count - 1)];

            return randomColor;
        }
    }
}
