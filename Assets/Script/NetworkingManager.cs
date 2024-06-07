using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NetworkingManager : NetworkManager
{
    [SerializeField] private InputField InputField_UserName;
    //[SerializeField] private GameObject gameManageMent;
    //private GameManageMent GameManageMent;


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        //GameObject gameManagement = Instantiate(gameManageMentPrefab);
        //NetworkServer.Spawn(gameManageMent.gameObject);
        
        //if (GameManageMent.StartGames)
        //{
        //    GameManageMent.StartCountDown();
        //}
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
