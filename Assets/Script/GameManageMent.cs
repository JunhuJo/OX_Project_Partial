using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class GameManageMent : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject O_Zone;
    [SerializeField] private GameObject X_Zone;
    [SerializeField] private GameObject Wall;

    [SerializeField] private CountDown CountDown;
    [SerializeField] private Text CountDown_Text;
    [SerializeField] private Text GameStart_Text;
    [SerializeField] private Text pointText;

    
    [SerializeField] private Button GameStartBtn;

    [Header("Question")]
    [SerializeField] Canvas UI_Window;
    [SerializeField] GameObject[] QuestionBox;


    public bool SetCountDown = false;
    public bool StartGames = false;
    public int GetPoint = 0;

    private float time = 0;
    private bool TrueFalse = false;

    //문제 정답 검증을 위한 변수 -> 1. 정답, 2.오답
    public int QustionValue = 0;

    
    private void Start()
    {
        
        for (int i = 0; i < QuestionBox.Length; i++)
        {
            GameObject questionInstance = Instantiate(QuestionBox[i], UI_Window.transform);
            QuestionBox[i] = questionInstance;
            questionInstance.SetActive(false);
        }
    }

    private void Update()
    {
        StartCountDown();
        ViewPoint();
    }

    public void OnClick_GameStart()
    {
        GameStartBtn.gameObject.SetActive(false);
        StartGames = true;
    }

    private void StartCountDown()
    {
        if (SetCountDown == true || StartGames == true)
        {
            CountDown.enabled = true;
        }
        else if (SetCountDown == false && CountDown_Text.text == "Time Over")
        {
            CountDown.enabled = false;
            X_Zone.gameObject.SetActive(true);
            O_Zone.gameObject.SetActive(true);
            Wall.gameObject.SetActive(true);
        }
        else if (StartGames == false && GameStart_Text.text == "게임 시작!@!")
        {
            CountDown.enabled = false;
        }
    }

   

    private void Verification()
    {
        QustionValue = 0;
    }

    private void ViewPoint()
    {
        pointText.text = $"맞춘 문제 : {GetPoint}";
    }
}
