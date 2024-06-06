using UnityEngine;

public class OXZoneTrigger : MonoBehaviour
{
    [SerializeField] GameManageMent gameManageMent;
    [SerializeField] Question question;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && this.gameObject.name == "O_Zone")
        {
            gameManageMent.Value = 1;
        }
        else if (other.gameObject.CompareTag("Player") && this.gameObject.name == "X_Zone")
        {
            gameManageMent.Value = 2;
        }
    }
}
