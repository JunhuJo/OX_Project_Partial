using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : MonoBehaviour
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
    [SerializeField] Canvas UI_Window;
    [SerializeField] GameObject[] QuestionBox;


    public bool setCountDown = false;
    public bool setStartWave = false;
    public bool StartGames = false;
    public bool Wave = false;
    public int GetPoint = 0;

    private int Rand;
    private float time = 0;
    private bool TrueFalse = false;

    //문제 정답 검증을 위한 변수 -> 1. 정답, 2.오답
    public int QustionValue = 0;

    private void Start()
    {
        CreateQuestion();
        RandQuestion();
    }

    private void Update()
    {
        StartCountDown();
        ViewPoint();
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
        StartGames = true;
    }

    private void StartCountDown()
    {
        //게임 시작
        if (setCountDown == true)
        {
            CountDown.enabled = true;
        }
        else if (StartGames == true)
        {
            StartCount.enabled = true;
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
            CountDown.enabled = false;
            Wave = true;
        }
    }

    private void RandQuestion()
    {
        Rand = Random.Range(0, QuestionBox.Length);
    }

    IEnumerator GameWave()
    {
        QuestionBox[Rand].SetActive(true);
        Wave = false;
        yield return new WaitForSeconds(10);
        
        Debug.Log("10초 시작");
        setCountDown = true;
    }
    //private void GameWave()
    //{
    //    
    //}

   //private void SetStartWave()
   //{
   //    
   //}

    private void Verification()
    {
        QustionValue = 0;
    }

    private void ViewPoint()
    {
        pointText.text = $"점수 : {GetPoint}";
    }
}
