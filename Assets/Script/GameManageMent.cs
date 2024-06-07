using Mirror;
using System.Collections;
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

    [SyncVar(hook = nameof(OnPointChanged))]
    public int GetPoint = 0; // Ŭ���̾�Ʈ���� ������ �����ϴ� �����Դϴ�.


    public bool setCountDown = false;
    public bool setStartWave = false;
    public bool StartGames = false;
    public bool Wave = false;

    private int Rand;
    
    //private int Rand;
    //private float time = 0;
    //private bool TrueFalse = false;

    //���� ���� ������ ���� ���� -> 1. ����, 2.����
    public int QustionValue = 0;

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
    }
    
    private void CreateQuestion()
    {
        for (int i = 0; i < QuestionBox.Length; i++)
        {
            GameObject questionInstance = Instantiate(QuestionBox[i], UI_Window.transform);
            QuestionBox[i] = questionInstance;
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
            StartGame(StartCount.enabled);
        }
    
        //���� ���� ����
        if (setCountDown == false && CountDown_Text.text == "Time Over")
        {
            CountDown.enabled = false;
            X_Zone.gameObject.SetActive(true);
            O_Zone.gameObject.SetActive(true);
            Wall.gameObject.SetActive(true);
            QuestionCheck();
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
        isCountDown(setCountDown);
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


    private void QuestionCheck()
    {
        
        for (int i = 0; i < QuestionBox.Length; i++)
        {
            GameObject targetObject = GameObject.Find($"Question0{i}(Clone)");
            if (targetObject.activeInHierarchy)
            {
                Question question = targetObject.GetComponent<Question>();
                if (question.QuestionCurrent == QustionValue)
                {
                    Win_Text.text = "���� �Դϴ� ^ �� ^";
                    GetPoint += 1;

                    StartCoroutine(CloseQuestionBox());
                }
                else if (question.QuestionCurrent != QustionValue)
                {
                    Win_Text.text = "Ʋ�Ƚ��ϴ� �� �� ��";
                    StartCoroutine(CloseQuestionBox());
                }
            }
        }
        
    }

    IEnumerator CloseQuestionBox()
    {
        yield return new WaitForSeconds(3);
        QustionValue = 0;
        Win_Text.text = " ";
    }

    private void OnPointChanged(int oldPoint, int newPoint)
    {
        if (isLocalPlayer)
        {
            pointText.text = $"���� : {newPoint}"; // Ŭ���̾�Ʈ���� ������ ������Ʈ�մϴ�.
        }
    }
}
