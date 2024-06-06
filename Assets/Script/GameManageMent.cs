using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject O_Zone;
    [SerializeField] private GameObject X_Zone;
    [SerializeField] private GameObject Wall;

    [SerializeField] private CountDown CountDown;
    [SerializeField] private Text CountDown_Text;
    [SerializeField] private Text pointText;

    [Header("Question")]
    [SerializeField] Canvas UI_Window;
    [SerializeField] GameObject[] QuestionBox;


    public bool SetCountDown = false;

    private bool TrueFalse = false;
    public int GetPoint = 0;
    
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

    private void StartCountDown()
    {
        if (SetCountDown == true)
        {
            CountDown.enabled = true;   
        }
        else if(SetCountDown == false && CountDown_Text.text == "Time Over")
        {
            CountDown.enabled = false;
            X_Zone.gameObject.SetActive(true);
            O_Zone.gameObject.SetActive(true);
            Wall.gameObject.SetActive(true);
        }
    }

    private void Verification()
    {


    }

    private void ViewPoint()
    {
        pointText.text = $"{GetPoint}";
    }
}
