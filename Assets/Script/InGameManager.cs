using System.Collections;
using System.Collections.Generic;
using Telepathy;
using UnityEngine;
using Mirror;

public class InGameManager : MonoBehaviour
{
    private NetworkManager networkingManager;


    public void OnClickStopServer()
    {
        
        networkingManager.StopHost();
    }
}
