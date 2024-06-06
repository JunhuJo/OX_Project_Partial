using UnityEngine;

public class OXZoneTrigger : MonoBehaviour
{
    [SerializeField] GameManageMent gameManageMent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && this.gameObject.name == "O_Zone")
        {
            gameManageMent.QustionValue = 1;
        }
        else if (other.gameObject.CompareTag("Player") && this.gameObject.name == "X_Zone")
        {
            gameManageMent.QustionValue = 2;
        }
    }
}
