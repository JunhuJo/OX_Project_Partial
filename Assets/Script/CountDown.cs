using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private GameManageMent GameManageMent;
    [SerializeField] private Text countDownText;
    [SerializeField] private Text startCount;

    void OnEnable()
    {
        StartCoroutine(Count());
    }
    IEnumerator Count()
    {
        if (GameManageMent.SetCountDown)
        {
            for (int i = 4; i < 5; i--)
            {
                countDownText.text = $"{i + 1}";
                Debug.Log($"카운트 : {i + 1}");
                yield return new WaitForSeconds(1);
                if (i <= 0) break;
            }
            countDownText.text = "Time Over";
            GameManageMent.SetCountDown = false;
            yield return new WaitForSeconds(0.5f);
            countDownText.text = " ";

        }
        
        else if (GameManageMent.StartGames )
        {
            for (int i = 4; i < 5; i--)
            {
                startCount.text = $"{i + 1}";
                Debug.Log($"카운트 : {i + 1}");
                yield return new WaitForSeconds(1);
                if (i <= 0) break;
            }
            startCount.text = "게임 시작!@!";
            GameManageMent.StartGames = false;
            yield return new WaitForSeconds(0.5f);
            startCount.text = " ";
        }
    }
}
