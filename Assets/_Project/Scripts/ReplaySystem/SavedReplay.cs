public class SavedReplay
{
    public string SerializedReplay { get; private set; }

    public SavedReplay(string serializedReplay)
    {
        SerializedReplay = serializedReplay;
    }
}
