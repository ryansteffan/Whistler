namespace Whistler.Music;

/// <summary>
/// Represents a queue that can be shuffled
/// and has a current item.
/// </summary>
/// <typeparam name="T">
/// The type of object that will be stored in the Music Queue.
/// </typeparam>
public class MusicQueue<T> : LinkedList<T>
{
    /// <summary>
    /// The currently selected node.
    /// </summary>
    private LinkedListNode<T>? Current { get; set; }
    
    /// <summary>
    /// Creates a new instance of the MusicQueue class.
    /// </summary>
    public MusicQueue()
    {
        Current = First;
    }
    
    public T GetCurrentSong()
    {
        if (Current == null)
        {
            throw new NullReferenceException("The current song is null at this point.");
        } 
        return Current.Value;
    }
    
    public T GetPreviousSong()
    {
        if (Current?.Previous == null)
        {
            throw new NullReferenceException("The previous song does not exist.");
        }
        T previousSong = Current.Previous.Value;
        Current = Current.Previous;
        return previousSong;
    }

    public T GetNextSong()
    {
        if (Current?.Next == null)
        {
            throw new NullReferenceException("The next song does not exist.");
        }
        T nextSong = Current.Next.Value;
        Current = Current.Next;
        return nextSong;
    }
    
    /// <summary>
    /// Shuffles the Queue to be in a random order.
    /// </summary>
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