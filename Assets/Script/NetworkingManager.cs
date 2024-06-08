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
