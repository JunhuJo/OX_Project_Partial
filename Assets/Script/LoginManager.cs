using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LoginManager : MonoBehaviour
{
    [Header("Loging_UI")]
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField serverAddressInput;
    [SerializeField] private Button serverLoginButton;
    [SerializeField] private Button clientLoginButton;
    [SerializeField] private NetworkManager networkManager;

    [Header("InGame_UI")]
    [SerializeField] private GameObject ServerName_Obj;
    [SerializeField] private Text ServerName_Text;
    [SerializeField] private GameObject InGameHostExitBtn;
    [SerializeField] private GameObject InGameClientExitBtn;


    private void Start()
    {
        serverLoginButton.onClick.AddListener(OnLoginButtonClicked);
        clientLoginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string serverAddress = serverAddressInput.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(serverAddress))
        {
            //서버 입장 화면
            PlayerPrefs.SetString("ServerStart_Btn", username);
            networkManager.networkAddress = serverAddress;
            ServerName_Text.text = $"Server : {serverAddress}";
            networkManager.StartHost();
            loginWindow.gameObject.SetActive(false);
            
            //서버 입장 후 인게임 처리
            ServerName_Obj.gameObject.SetActive(true);
            InGameHostExitBtn.gameObject.SetActive(true);
        }
        else if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(serverAddress))
        {
            //
            PlayerPrefs.SetString("ClientStart_Btn", username);
            networkManager.networkAddress = serverAddress;
            ServerName_Text.text = $"Server_Name : {serverAddress}";
            networkManager.StartClient();
            loginWindow.gameObject.SetActive(false);
            
            //
            ServerName_Obj.gameObject.SetActive(true);
            InGameClientExitBtn.gameObject.SetActive(true);
        }
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
