using UnityEngine;

public class OXZoneTrigger : MonoBehaviour
{
    [SerializeField] GameManageMent gameManageMent;
    private bool Success = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Success = true;
            if (Success)
            {
                gameManageMent.GetPoint += 1;
                gameObject.SetActive(false);
                Success = false;
            }
        }
    }
}
