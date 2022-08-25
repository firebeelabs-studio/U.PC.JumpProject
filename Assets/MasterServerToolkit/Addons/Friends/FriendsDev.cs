using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.Dev
{
    public class FriendsDev : MonoBehaviour
    {
        private void Start()
        {
            Mst.Client.Connection.AddConnectionOpenListener(OnConnectionOpen);
        }

        private void OnConnectionOpen(IClientSocket client)
        {
            Mst.Client.Friends.Test();
        }
    }
}