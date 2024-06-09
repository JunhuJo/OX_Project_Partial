using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class SetComponentTrigger : MonoBehaviour
{
    [SerializeField] private Text point;
    [SerializeField] private Text Win_Text;
    [SerializeField] private GameManageMent gameManageMent;
    [SerializeField] private GameObject SetResult_Win;
    [SerializeField] private GameObject SetResult_Lose;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<NetSpawnedObject>().Win_Text = Win_Text;
            other.gameObject.GetComponent<NetSpawnedObject>().gameManageMent= gameManageMent;
            other.gameObject.GetComponent<NetSpawnedObject>().SetResult_Win = SetResult_Win;
            other.gameObject.GetComponent<NetSpawnedObject>().SetResult_Lose = SetResult_Lose;
        }
    }
}
