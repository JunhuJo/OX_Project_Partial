using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartCount : MonoBehaviour
{
    [SerializeField] private GameManageMent GameManageMent;
    
    [SerializeField] private Text startCount;

    void OnEnable()
    {
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        if (GameManageMent.StartGames)
        {
            for (int i = 4; i < 5; i--)
            {
                startCount.text = $"{i + 1}";
                Debug.Log($"시작 카운트 : {i + 1}");
                yield return new WaitForSeconds(1);
                if (i <= 0) break;
            }
            startCount.text = "게임 시작!@!";
            GameManageMent.StartGames = false;
            yield return new WaitForSeconds(1);
            startCount.text = " ";
        }
    }
}
