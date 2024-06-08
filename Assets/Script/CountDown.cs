using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private GameManageMent GameManageMent;
    [SerializeField] private Text countDownText;

    void OnEnable()
    {
        StartCoroutine(Count());
    }
    IEnumerator Count()
    {
        if (GameManageMent.setCountDown)
        {
            for (int i = 4; i >=0; i--)
            {
                countDownText.text = $"{i + 1}";
                yield return new WaitForSeconds(1);
                if (i <= 0) break;
                //countDownText.text = $"{i + 1}";
                //Debug.Log($"종료 카운트 : {i + 1}");
                //yield return new WaitForSeconds(1);
                //if (i <= 0) break;
            }
            countDownText.text = "Time Over";
            GameManageMent.setCountDown = false;
            yield return new WaitForSeconds(1);
            countDownText.text = " ";
        }
    }
}
