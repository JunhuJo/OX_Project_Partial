using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : MonoBehaviour
{
    [SerializeField] private GameObject O_Zone;
    [SerializeField] private GameObject X_Zone;
    [SerializeField] private GameObject Wall;

    [SerializeField] private CountDown CountDownText;
    [SerializeField] private Text pointText;
    
    
    public bool SetCountDown = false;

    private bool TrueFalse = false;
    public int GetPoint = 0;


    private void Update()
    {
        StartCountDown();
        ViewPoint();
    }

    private void StartCountDown()
    {
        if (SetCountDown == true)
        {
            CountDownText.enabled = true;   
        }
        else if(CountDownText.gameObject.activeSelf == false)
        {
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
