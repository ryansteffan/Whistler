using Discord.WebSocket;

namespace Whistler.AppCommands;

public interface IAppCommand
{
    public string CommandName { get; }
    
    public Task AddCommandAsync(ulong guildId);
    
    public Task HandleCommandAsync(SocketSlashCommand command);
}