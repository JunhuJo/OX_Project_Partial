
using UnityEngine;

public class OXZoneTrigger : MonoBehaviour
{
    [SerializeField] GameManageMent gameManageMent;
    [SerializeField] NetSpawnedObject player;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player") && this.gameObject.name == "O_Zone")
        {
            player.QustionValue = 1;
            gameManageMent.QuestionCheck();
            Debug.Log($"QustionValue set to: {player.QustionValue} by O_Zone");
        }
        else if (other.gameObject.CompareTag("Player") && this.gameObject.name == "X_Zone")
        {
            player.QustionValue = 2;
            gameManageMent.QuestionCheck();
            Debug.Log($"QustionValue set to: {player.QustionValue} by X_Zone");
        }
        
    }
}
