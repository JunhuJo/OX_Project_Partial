using System.Collections.Generic;
using UnityEngine;

public class RandomCreate : MonoBehaviour
{
    [SerializeField] private GameObject[] QuestionBox;
    [SerializeField] private GameManageMent EndGame;
    

    public int rand;
    public int CreateRand;

    public List<int> RandList = new List<int>();

    private void OnEnable()
    {
        // 중복되지 않는 랜덤 값을 찾을 때까지 반복
        do
        {
            rand = Random.Range(0, QuestionBox.Length);
        } while (RandList.Contains(rand));

        // 중복되지 않는 값을 리스트에 추가
        if (RandList.Count == QuestionBox.Length)
        {
            EndGame.EndGame = true;
        }
        else 
        {
            RandList.Add(rand);
            CreateRand = rand;
        }
    }
}