using FishNet.Object;
using FishNet.Object.Synchronizing;
public class PlayerSettings : NetworkBehaviour
{
    [SyncVar] private string _username;

    public void SetUsername(string value)
    {
        _username = value;
    }

    public string GetUserName()
    {
        return _username;
    }
}

