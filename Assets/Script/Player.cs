using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    private void Start()
    {
        if (isLocalPlayer)
        {
            string username = PlayerPrefs.GetString("username");
            CmdSetPlayerName(username);
        }
    }

    [Command]
    private void CmdSetPlayerName(string username)
    {
        playerName = username;
    }
}
