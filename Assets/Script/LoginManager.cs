using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LoginManager : MonoBehaviour
{
    [Header("Loging_UI")]
    public GameObject loginWindow;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField serverAddressInput;
    [SerializeField] private Button serverLoginButton;
    [SerializeField] private Button clientLoginButton;
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private SceneTransition SceneChange;

    [Header("InGame_UI")]
    [SerializeField] private GameObject ServerName_Obj;
    [SerializeField] private Text ServerName_Text;
    [SerializeField] private GameObject InGameHostExitBtn;
    [SerializeField] private GameObject InGameClientExitBtn;
    [SerializeField] private GameObject GameStart_Btn;


    private void Start()
    {
        // ������ Ŭ���̾�Ʈ �α��� ��ư�� �̺�Ʈ ������ �߰�
        serverLoginButton.onClick.AddListener(OnClick_StartServer);
        clientLoginButton.onClick.AddListener(OnClick_StartClient);

    }


    public void OnClick_StartServer()
    {
        string username = usernameInput.text;
        string serverAddress = serverAddressInput.text;
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(serverAddress))
        {
            //���� ���� ȭ��
            PlayerPrefs.SetString("PlayerName", username); // �г��� ����
            networkManager.networkAddress = serverAddress;
            ServerName_Text.text = $"Server : {serverAddress}";
            networkManager.StartHost();
            SceneChange.isGame = true;
            Invoke("Wait", 1.5f);

            //���� ���� �� �ΰ��� ó��
            ServerName_Obj.gameObject.SetActive(true);
            InGameHostExitBtn.gameObject.SetActive(true);
            GameStart_Btn.gameObject.SetActive(true);
        }
    }

    public void OnClick_StartClient() 
    {
        string username = usernameInput.text;
        string serverAddress = serverAddressInput.text;
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(serverAddress))
        {
            PlayerPrefs.SetString("PlayerName", username); // �г��� ����
            networkManager.networkAddress = serverAddress;
            ServerName_Text.text = $"Server_Name : {serverAddress}";
            networkManager.StartClient();
            SceneChange.isGame = true;
            Invoke("Wait", 1.5f);

            ServerName_Obj.gameObject.SetActive(true);
            InGameClientExitBtn.gameObject.SetActive(true);
        }
    }

    public void Wait()
    {
        loginWindow.gameObject.SetActive(false);
        SceneChange.isGame = false;
    }

    
    public void StopHost()
    {
        networkManager.StopHost();
        loginWindow.gameObject.SetActive(true);
        ServerName_Obj.SetActive(false);
        InGameHostExitBtn.gameObject.SetActive(false);
    }

    public void StopClient()
    {
        networkManager.StopClient();
        loginWindow.gameObject.SetActive(true);
        ServerName_Obj.SetActive(false);
        InGameHostExitBtn.gameObject.SetActive(false);
    }

    public void ApplicationExit()
    {
        Application.Quit();
    }
}
