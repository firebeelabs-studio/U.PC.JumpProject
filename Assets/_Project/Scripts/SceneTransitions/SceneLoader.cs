
using System.Collections.Generic;
using FishNet;
using UnityEngine;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool _moveAllObjects;
    [SerializeField] private bool _automaticallyUnload;
    [SerializeField] private string[] _scenes = new string[0];
    [SerializeField] private bool _replaceScenes;

    [Client]
    public void LoadScenePlease()
    {
        LoadScene();
    }
    
    [Server(Logging = LoggingType.Off)]
    public void LoadScene(NetworkConnection sender = null)
    {
        if (!InstanceFinder.NetworkManager.IsServer)
            return;
        
        //Which objects to move.
        List<NetworkObject> movedObjects = new List<NetworkObject>();
        if (_moveAllObjects)
        {
            foreach (NetworkConnection item in InstanceFinder.ServerManager.Clients.Values)
            {
                foreach (NetworkObject nob in item.Objects)
                {
                    movedObjects.Add(nob);
                }
            }
        }

        LoadOptions loadOptions = new LoadOptions
        {
            AutomaticallyUnload = _automaticallyUnload
        };

        SceneLoadData sld = new SceneLoadData(_scenes);
        sld.ReplaceScenes = (_replaceScenes) ? ReplaceOption.All : ReplaceOption.None;
        sld.Options = loadOptions;
        sld.MovedNetworkObjects = movedObjects.ToArray();


        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        // InstanceFinder.SceneManager.LoadConnectionScenes(sender, sld);
        print("works");
    }
    
}
