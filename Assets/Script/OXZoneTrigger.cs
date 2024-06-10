using UnityEngine;


public class OXZoneTrigger : MonoBehaviour
{
    public bool check = false;
    [SerializeField] GameObject Wall;


    private void Update()
    {
        CloseOX();
    }

    void CloseOX()
    {
        if (!check)
        {
            Wall.gameObject.SetActive(false);
            gameObject.SetActive(false);
            
        }
    }

}

