using Mirror;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Text pointText;

    [SerializeField] private Button GameStartBtn;

    [Header("Question")]
    [SerializeField] private Canvas UI_Window;
    [SerializeField] private Text Win_Text;
    [SerializeField] private GameObject[] QuestionBox;
    [SerializeField] private NetSpawnedObject Player;

    [Header("Volume")]
    [SerializeField] private AudioSource Login_BGM;
    [SerializeField] private AudioSource InGame_BGM;
    [SerializeField] private AudioSource Effect_Sound;
    [SerializeField] private Slider Volume_Slider;

    ///[Header("Network")]
    ///[SerializeField] private LoginManager LoginManager;

    

    public bool setCountDown = false;
    public bool setStartWave = false;
    public bool StartGames = false;
    public bool Wave = false;
    private int Rand; //���� ���� �� ���� ����
  
    
   
    //// Ŭ�е� QuestionBox ������Ʈ���� �����ϱ� ���� ����Ʈ
    //private List<GameObject> questionBoxClones = new List<GameObject>();

    private void Start()
    {
        CreateQuestion();
        RandQuestion(QuestionBox.Length);
    }

    private void Update()
    {
        StartCountDown();
        OnPointChanged();
        
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
        StartGames = true; // ���� ������ �������� ó���մϴ�.
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
        //���� ����
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
    
        //���� ���� ����
        if (setCountDown == false && CountDown_Text.text == "Time Over")
        {
            CountDown.enabled = false;
            X_Zone.gameObject.SetActive(true);
            O_Zone.gameObject.SetActive(true);
            Wall.gameObject.SetActive(true);
            
        }
        else if (StartGames == false && GameStart_Text.text == "���� ����!@!")
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
        
        Debug.Log("10�� ����");
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


    public void QuestionCheck()
    {
        Debug.Log("���� ���� üũ ����~~~");

        // Ŭ�е� QuestionBox ����Ʈ�� ��ȸ�ϸ� ������Ʈ�� Ȱ�� ���¸� Ȯ���մϴ�.
        foreach (GameObject questionBox in QuestionBox)
                 {
            if (questionBox.activeInHierarchy)
            {
                Question question = questionBox.GetComponent<Question>();
                if (question != null)
                {
                    Debug.Log($"QuestionCurrent: {question.QuestionCurrent}, QustionValue: {Player.QustionValue}");
                    if (question.QuestionCurrent == Player.QustionValue)
                    {
                        question.gameObject.SetActive(false);
                        Win_Text.text = "���� �Դϴ� ^ �� ^";
                        Player.GetPoint += 1;

                        StartCoroutine(CloseQuestionBox());
                    }
                    else if (question.QuestionCurrent != Player.QustionValue)
                    {
                        question.gameObject.SetActive(false);
                        Win_Text.text = "Ʋ�Ƚ��ϴ� �� �� ��";
                        StartCoroutine(CloseQuestionBox());
                    }
                }
            }
        }

        //for (int i = 0; i < QuestionBox.Length; i++)
        //{
        //    Debug.Log("���� ���� üũ ����~~~");
        //    GameObject targetObject = GameObject.Find($"Question0{i}(Clone)");
        //    if (targetObject.activeInHierarchy)
        //    {
        //        Question question = targetObject.GetComponent<Question>();
        //        if (question.QuestionCurrent == QustionValue)
        //        {
        //            Win_Text.text = "���� �Դϴ� ^ �� ^";
        //            GetPoint += 1;
        //
        //            StartCoroutine(CloseQuestionBox());
        //        }
        //        else if (question.QuestionCurrent != QustionValue)
        //        {
        //            Win_Text.text = "Ʋ�Ƚ��ϴ� �� �� ��";
        //            StartCoroutine(CloseQuestionBox());
        //        }
        //    }
        //}

    }

    IEnumerator CloseQuestionBox()
    {
        yield return new WaitForSeconds(3);
        Player.QustionValue = 0;
        Win_Text.text = " ";
    }

    private void OnPointChanged()
    {
          pointText.text = $"���� : {Player.GetPoint}"; 
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
