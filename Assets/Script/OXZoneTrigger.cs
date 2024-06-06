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
            gameManageMent.GetPoint += 1;
            Invoke("CloseSuccess", 2.0f);
        }
    }

    private void CloseSuccess()
    {
        Success = false;
    }
}
