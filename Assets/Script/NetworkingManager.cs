using UnityEngine;
using Mirror;
using UnityEngine.UI;
using kcp2k;
using System.Drawing;

public class NetworkingManager : NetworkManager
{
    [SerializeField] private InputField InputField_UserName;

    //[SerializeField] private Text Win_Text;
    //[SerializeField] private Text Point;
    //[SerializeField] private GameManageMent gameManageMent;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab);

        //player.GetComponent<NetSpawnedObject>().point = Point;
        //player.GetComponent<NetSpawnedObject>().Win_Text = Win_Text;
        //player.GetComponent<NetSpawnedObject>().gameManageMent = gameManageMent;
       
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
