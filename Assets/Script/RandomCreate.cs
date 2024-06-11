using System.Collections.Generic;
using UnityEngine;

public class RandomCreate : MonoBehaviour
{
    [SerializeField] private GameObject[] QuestionBox;
    [SerializeField] private GameManageMent EndGame;
    public int CreateRand;

    public List<int> RandList = new List<int>();


    private void OnEnable()
    {
        CreateRand = Random.Range(0, QuestionBox.Length);
        RandList.Add(CreateRand);
        for (int i = 0; i < RandList.Count; i++)
        {
            if (RandList[i] == CreateRand)
            {
                CreateRand = Random.Range(0, QuestionBox.Length);
            }
            
        }
    }
}
