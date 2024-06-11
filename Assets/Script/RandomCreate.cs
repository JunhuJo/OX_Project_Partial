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
        // �ߺ����� �ʴ� ���� ���� ã�� ������ �ݺ�
        do
        {
            rand = Random.Range(0, QuestionBox.Length);
        } while (RandList.Contains(rand));

        // �ߺ����� �ʴ� ���� ����Ʈ�� �߰�
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