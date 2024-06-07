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
        // 클라이언트 연결 후 추가 작업 처리
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        // 클라이언트 연결 해제 후 추가 작업 처리
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }
}
