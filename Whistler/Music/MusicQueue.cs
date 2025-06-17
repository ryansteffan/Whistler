namespace Whistler.Music;

public class MusicQueue<T> : LinkedList<T>
{
    public void Shuffle()
    {
        List<T> musicList = new List<T>();
        Random.Shared.Shuffle(musicList.ToArray());
        Clear();
        foreach (T song in musicList)
        {
            AddFirst(song);
        }
    }
}