namespace Whistler.Music;

public class Song(
    string name, 
    string artist, 
    string albumArt, 
    string album, 
    float duration, 
    float currentTime
    )
{

    public string Name { get; set; } = name;
    public string Artist { get; set; } = artist;
    public string AlbumArt {get; set; } = albumArt;
    public string Album { get; set; } = album;
    public float Duration { get; set; } = duration;
    public float CurrentTime { get; set; } = currentTime;
}