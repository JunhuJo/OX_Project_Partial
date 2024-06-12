
using Org.BouncyCastle.Asn1.Cms;
using UnityEngine;
using UnityEngine.UI;

public class SetComponentTrigger : MonoBehaviour
{
    [SerializeField] private Text point;
    [SerializeField] private Text Win_Text;
    [SerializeField] private Text Exit_Room;
    [SerializeField] private GameManageMent gameManageMent;
    [SerializeField] private GameObject SetResult_Win;
    [SerializeField] private GameObject SetResult_Lose;
    [SerializeField] private GameObject Client_Stop_Btn;
    [SerializeField] private GameObject Server_Stop_Btn;
    [SerializeField] private NetworkingManager NetworkManager;
    [SerializeField] private GameObject Login_Window;
    [SerializeField] private Text Point_Text;
    [SerializeField] private RandomCreate RandomCreate;
    [SerializeField] private Text Questdown;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<NetSpawnedObject>().Win_Text = Win_Text;
            other.gameObject.GetComponent<NetSpawnedObject>().gameManageMent= gameManageMent;
            other.gameObject.GetComponent<NetSpawnedObject>().SetResult_Win = SetResult_Win; 
            other.gameObject.GetComponent<NetSpawnedObject>().SetResult_Lose = SetResult_Lose;
            other.gameObject.GetComponent<NetSpawnedObject>().NetworkManager = NetworkManager;
            other.gameObject.GetComponent<NetSpawnedObject>().Exit_Room = Exit_Room;
            other.gameObject.GetComponent<NetSpawnedObject>().Login_Window = Login_Window;
            other.gameObject.GetComponent<NetSpawnedObject>().Point_Text = Point_Text;
            other.gameObject.GetComponent<NetSpawnedObject>().randomCreate = RandomCreate;
            other.gameObject.GetComponent<NetSpawnedObject>().Questdown = Questdown;
        }
    }
}
