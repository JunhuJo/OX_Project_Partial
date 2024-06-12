using System.Collections.Generic;
using UnityEngine;

public class RandomCreate : MonoBehaviour
{
    [SerializeField] private GameObject[] QuestionBox;
    [SerializeField] private GameManageMent EndGame;
    public int QuestionCount;

    public int rand;
    public int CreateRand;
   
    public List<int> RandList = new List<int>();

    private void Start()
    {
        QuestionCount = QuestionBox.Length;
    }

    private void OnEnable()
    {
        do
        {
            rand = Random.Range(0, QuestionBox.Length);
        } while (RandList.Contains(rand));

        RandList.Add(rand);
        CreateRand = rand;
    }
}