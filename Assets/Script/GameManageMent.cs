using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : MonoBehaviour
{
    [SerializeField] private GameObject O_Zone;
    [SerializeField] private GameObject X_Zone;
    [SerializeField] private GameObject Wall;
    [SerializeField] private Text pointText;
    //[SerializeField] private GameObject counDownText_obj;
    [SerializeField] private Text countDownText;

    [SerializeField] private bool SetContDown = false;

    private bool TrueFalse = false;
    public int GetPoint = 0;


    private void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    private void Start()
    {
        StartCountDown();
        ViewPoint();
    }

    //void Update()
    //{
    //    
    //}

    private void ViewPoint()
    {
        pointText.text = $"{GetPoint}";
    }

    private void StartCountDown()
    {
        if (SetContDown == true)
        {
            countDownText.enabled = true;
        }
        else if(SetContDown == false)
        {
            Wall.gameObject.SetActive(true);

            X_Zone.gameObject.SetActive(true);
            O_Zone.gameObject.SetActive(true);

            countDownText.enabled = false;
        }
    }
    IEnumerator CountDown()
    { 
        for (int i = 4; i < 5; i--)
        {
            countDownText.text = $"{i+1}";
            Debug.Log($"Ä«¿îÆ® : {i+1}");
            yield return new WaitForSeconds(1);
            if (i <= 0) break;
        }
        countDownText.text = "Time Over";
        SetContDown = false;
    }
}
