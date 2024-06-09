using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject O_Zone;
    [SerializeField] private GameObject X_Zone;
    [SerializeField] private GameObject Wall;

    [SerializeField] private CountDown CountDown;
    [SerializeField] private StartCount StartCount;
    [SerializeField] private Text CountDown_Text;
    [SerializeField] private Text GameStart_Text;
    //[SerializeField] private Text pointText;

    [SerializeField] private Button GameStartBtn;

    [Header("Question")]
    [SerializeField] private Canvas UI_Window;
    //[SerializeField] private Text Win_Text;
    public GameObject[] QuestionBox;
    [SerializeField] private NetSpawnedObject player;
    [SerializeField] private OXZoneTrigger O_ZoneTrigger;
    [SerializeField] private OXZoneTrigger X_ZoneTrigger;

    [Header("Volume")]
    [SerializeField] private AudioSource Login_BGM;
    [SerializeField] private AudioSource InGame_BGM;
    [SerializeField] private AudioSource Effect_Sound;
    [SerializeField] private Slider Volume_Slider;

    [Header("PlayerPrefab")]
    [SerializeField] private GameObject playerprefab;


    public bool setCountDown = false;
    public bool setStartWave = false;
    public bool StartGames = false;
    public bool Wave = false;
    private int Rand; //랜덤 문제 값 담을 변수
  
    
   
    //// 클론된 QuestionBox 오브젝트들을 추적하기 위한 리스트
    //private List<GameObject> questionBoxClones = new List<GameObject>();

    private void Start()
    {
        CreateQuestion();
        RandQuestion(QuestionBox.Length);
    }

    private void Update()
    {
        StartCountDown();
        
        
        if (Wave) 
        {
            StartCoroutine(GameWave());
        }

        VolumeControl();

    }



    private void CreateQuestion()
    {
        for (int i = 0; i < QuestionBox.Length; i++)
        {
            GameObject questionInstance = Instantiate(QuestionBox[i], UI_Window.transform);
            QuestionBox[i] = questionInstance;
            //questionBoxClones.Add(questionInstance);
            
            questionInstance.SetActive(false);
        }
    }

    public void OnClick_GameStart()
    {
      
        GameStartBtn.gameObject.SetActive(false);
        StartGames = true; // 게임 시작을 서버에서 처리합니다.
        ReceiveGameStart(StartGames);
        //GameStartBtn.gameObject.SetActive(false);
        //StartGames = true;
    }

    [Command]
    private void GameStart(bool GameStart)
    {
        ReceiveGameStart(GameStart);
    }

    [ClientRpc]
    private void ReceiveGameStart(bool GameStart)
    {
        StartGames = GameStart;
    }

    public void StartCountDown()
    {
        //게임 시작
        if (setCountDown == true)
        {
            CountDown.enabled = true;
        }
        else if (StartGames == true)
        {
            StartCount.enabled = true;

            if (isServer)
            {
                StartGame(StartCount.enabled);
            }
        }
    
        //게임 라운드 종료
        if (setCountDown == false && CountDown_Text.text == "Time Over")
        {
            CountDown.enabled = false;
            X_Zone.gameObject.SetActive(true);
            O_Zone.gameObject.SetActive(true);
            Wall.gameObject.SetActive(true);
            
        }
        else if (StartGames == false && GameStart_Text.text == "게임 시작!@!")
        {
            StartCount.enabled = false;
            Wave = true;
        }
    }

    [Command]
    private void StartGame(bool StartCount)
    {
        RpcReceiveStartGame(StartCount);
    }

    [ClientRpc]
    private void RpcReceiveStartGame(bool StartCount)
    {
        CountDown.enabled = StartCount;
    }


    [Command]
    private void RandQuestion(int QuestionBoxLenght)
    {
        RpcReceiveRandQuestion(QuestionBoxLenght);
    }

    [ClientRpc]
    private void RpcReceiveRandQuestion(int QuestionBoxLenght)
    {
        Rand = Random.Range(0, QuestionBoxLenght);
    }



    IEnumerator GameWave()
    {
        yield return new WaitForSeconds(1);
        QuestionBox[Rand].SetActive(true);
        Wave = false;
        yield return new WaitForSeconds(10);
        
        Debug.Log("10초 시작");
        setCountDown = true;
        if (isServer)
        {
            isCountDown(setCountDown);
        }
        
    }

    [Command]
    private void isCountDown(bool setCountDouwn)
    {
        RpcReceiveisCountDown(setCountDouwn);
    }

    [ClientRpc]
    private void RpcReceiveisCountDown(bool setCountDouwn)
    {
        setCountDown = setCountDouwn;
    }


    private void VolumeControl()
    {
        if (Login_BGM != null)
        {
            Login_BGM.volume = Volume_Slider.value;
        }
        if (InGame_BGM != null)
        {
            InGame_BGM.volume = Volume_Slider.value;
        }
        if (Effect_Sound != null)
        {
            Effect_Sound.volume = Volume_Slider.value;
        }
    }
}
