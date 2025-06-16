using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using Whistler.Logging;

namespace Whistler.AppCommands;

public class MusicControls(DiscordSocketClient client, string commandName = "music-controls") : IAppCommand
{
    public string CommandName { get; } = commandName;
    private Logger CommandLogger { get; set; } = Logger.GetLogger("MakeFilterChannel Logger", LogLevel.Info);
    
    public async Task AddCommandAsync(ulong guildId)
    {
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName(CommandName)
            .WithDescription("Gets the controls for the music that is playing currently.");

        try
        {
            await client.GetGuild(guildId).CreateApplicationCommandAsync(commandBuilder.Build());
        }
        catch (HttpException exception)
        {
            string response = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            CommandLogger.Error(response);
        }

    }

    public async Task HandleCommandAsync(SocketSlashCommand command)
    {
        ComponentBuilder componentBuilder = new ComponentBuilder()
            .WithButton("Previous", "whistler-previous-button")
            .WithButton("Play/Pause", "whistler-play-pause-button")
            .WithButton("Skip", "whistler-skip-button");
        
        // TODO: Make the embed dynamic.
        EmbedBuilder embedBuilder = new EmbedBuilder()
            .WithTitle("Music Management:")
            .AddField("Song: ", "Runnin' with the Devil")
            .AddField("Artist: ", "Van Halen")
            .AddField("Current Time: ", "2:00")
            .AddField("Length: ", "3:35")
            .WithImageUrl("https://lh3.googleusercontent.com/fo7TvhOuUeNhvkJXS2bXtuPTktt1IG0iL6P0aHh6Jumc1kMXeImwWsphwaYj68wfD6jCAYkkXdDTwCX2=w544-h544-l90-rj");
        
        
        await command.RespondAsync(embeds: [embedBuilder.Build()], components: componentBuilder.Build());
    }
}