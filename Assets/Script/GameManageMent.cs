using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManageMent : MonoBehaviour
{
    [SerializeField] private OXZoneTrigger ZoneTrigger;
    [SerializeField] private GameObject Wall;
    [SerializeField] private Text pointText;
    [SerializeField] private Text countDownText;

    private bool SetContDown = false;

    public int GetPoint = 0;

    void Update()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    { 
        if(SetContDown == true)
        {
            countDownText.text = "5";
            yield return new WaitForSeconds(1);
            countDownText.text = "4";
            yield return new WaitForSeconds(1);
            countDownText.text = "3";
            yield return new WaitForSeconds(1);
            countDownText.text = "2";
            yield return new WaitForSeconds(1);
            countDownText.text = "1";
            yield return new WaitForSeconds(1);
            Wall.gameObject.SetActive(true);    
        }
    }
}
