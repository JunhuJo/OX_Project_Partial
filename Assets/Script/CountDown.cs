using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private GameManageMent GameManageMent;
    private Text countDownText;

    private void Awake()
    {
        countDownText = GetComponent<Text>();
    }

    void OnEnable()
    {
        Count();
    }

    IEnumerator Count()
    {
        if(GameManageMent.SetCountDown == true)
        {
            for (int i = 4; i < 5; i--)
            {
                countDownText.text = $"{i + 1}";
                Debug.Log($"Ä«¿îÆ® : {i + 1}");
                yield return new WaitForSeconds(1);
                if (i <= 0) break;
            }
            countDownText.text = "Time Over";
            countDownText.enabled = false;
        }
    }
}
