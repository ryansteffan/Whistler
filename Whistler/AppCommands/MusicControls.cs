using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using Whistler.Logging;
using Whistler.Music;

namespace Whistler.AppCommands;

public class MusicControls(
    DiscordSocketClient client, 
    ref Dictionary<string, MusicQueue<Song>> musicQueues, 
    string commandName = "music-controls"
    ) : IAppCommand
{
    public string CommandName { get; } = commandName;
    private Logger CommandLogger { get; set; } = Logger.GetLogger("MakeFilterChannel Logger", LogLevel.Info);
    private bool IsPaused { get; set; } = false;
    
    public async Task AddCommandAsync(ulong guildId)
    {
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName(CommandName)
            .WithDescription("Gets the controls for the music that is playing currently.");
        
        client.ButtonExecuted += ButtonHandler;

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
        Embed embed = CreateEmbed();
        MessageComponent component = CreateComponent();
        
        await command.RespondAsync(embeds: [embed], components: component);
    }

    private async Task ButtonHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "whistler-play-pause-button":
                await component.RespondAsync(
                    IsPaused ? "Paused music..." : "Playing music...", 
                    ephemeral: true
                    );
                IsPaused = !IsPaused;
                await component.Message.ModifyAsync(properties =>
                {
                    properties.Components = CreateComponent();
                });
                break;
            case "whistler-previous-button":
                await component.RespondAsync("Playing previous track...", ephemeral: true);
                break;
            case "whistler-skip-button":
                await component.RespondAsync("Playing next track...", ephemeral: true);
                break;
            default: 
                CommandLogger.Warn($"Unknown command from {component.Data.CustomId}");
                break;
        }
    }

    private Embed CreateEmbed()
    {
        // TODO: Make the embed dynamic.
        EmbedBuilder embedBuilder = new EmbedBuilder()
            .WithTitle("Music Management:")
            .WithColor(Color.DarkBlue)
            .AddField("Song: ", "Runnin' with the Devil")
            .AddField("Artist: ", "Van Halen")
            .AddField("Current Time: ", "2:00")
            .AddField("Length: ", "3:35")
            .WithImageUrl("https://lh3.googleusercontent.com/fo7TvhOuUeNhvkJXS2bXtuPTktt1IG0iL6P0aHh6Jumc1kMXeImwWsphwaYj68wfD6jCAYkkXdDTwCX2=w544-h544-l90-rj");

        return embedBuilder.Build();
    }

    private MessageComponent CreateComponent()
    {
        ComponentBuilder componentBuilder = new ComponentBuilder()
            .WithButton("Previous", "whistler-previous-button")
            .WithButton(IsPaused ? "Pause" : "Play", "whistler-play-pause-button")
            .WithButton("Skip", "whistler-skip-button");
        
        return componentBuilder.Build();
    }
}