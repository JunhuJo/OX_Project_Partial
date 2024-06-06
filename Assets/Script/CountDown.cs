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
        
        if(GameManageMent.SetCountDown)
        {
            for (int i = 4; i < 5; i--)
            {
                countDownText.text = $"{i + 1}";
                Debug.Log($"Ä«¿îÆ® : {i + 1}");
                yield return new WaitForSeconds(1);
                if (i <= 0) break;
            }
            countDownText.text = "Time Over";
            GameManageMent.SetCountDown = false;
            yield return new WaitForSeconds(0.5f);
            countDownText.text = " ";
            
        }
    }
}
