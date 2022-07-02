using FishNet;
using FishNet.Connection;
using FishNet.Object;

public class ClientInstance : NetworkBehaviour
{
    //atm this class will update for every user overall connected to server have to change this to RPC
    // Start is called before the first frame update
    public static ClientInstance Instance;
    public bool Initialized { get; private set; }

    //version of code on client
    private const int VERSION_CODE = 0;
    public PlayerSettings PlayerSettings { get; private set; }

    private void Awake()
    {
        PlayerSettings = GetComponent<PlayerSettings>();
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        if (IsOwner)
        {
            Instance = this;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            CmdVerifyVersion(VERSION_CODE);
        }
    }

    //Compares server and client version
    [ServerRpc]
    private void CmdVerifyVersion(int versionCode)
    {
        bool pass = (versionCode == VERSION_CODE);
        TargetVerifyVersion(Owner, pass);

        if (!pass)
        {
            NetworkManager.TransportManager.Transport.StopConnection(Owner.ClientId, false);
        }

    }

    [TargetRpc]
    private void TargetVerifyVersion(NetworkConnection owner, bool pass)
    {
        Initialized = pass;
        if (!pass)
        {
            NetworkManager.ClientManager.StopConnection();
        }
    }

    public static ClientInstance ReturnClientInstance(NetworkConnection conn)
    {
        //if called from server
        if (InstanceFinder.IsServer && conn is not null)
        {
            NetworkObject nob = conn.FirstObject;
            return (nob is null) ? null : nob.GetComponent<ClientInstance>();
        }
        else //if called from client
        {
            return Instance;
        }
    }
}
