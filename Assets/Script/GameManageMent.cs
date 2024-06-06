using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : MonoBehaviour
{
    [SerializeField] private GameObject O_Zone;
    [SerializeField] private GameObject X_Zone;
    [SerializeField] private GameObject Wall;

    [SerializeField] private CountDown CountDown;
    [SerializeField] private Text CountDown_Text;
    [SerializeField] private Text pointText;
    
    
    public bool SetCountDown = false;

    private bool TrueFalse = false;
    public int GetPoint = 0;
    
    //문제 정답 검증을 위한 변수 -> 1. 정답, 2.오답
    public int QustionValue = 0;
    


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

    private void ViewPoint()
    {
        pointText.text = $"{GetPoint}";
    }
}
