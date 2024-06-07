using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NetworkingManager : NetworkManager
{
    [SerializeField] private InputField InputField_UserName;
    

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        // Ŭ���̾�Ʈ ���� �� �߰� �۾� ó��
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        // Ŭ���̾�Ʈ ���� ���� �� �߰� �۾� ó��
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }
}
