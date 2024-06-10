using UnityEngine;

public class WindowSize : MonoBehaviour
{
    [SerializeField] private int Width = 1280;
    [SerializeField] private int Heigth = 720;

    private void Start()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        // ���ϴ� �ػ󵵸� �����մϴ�. ��: 1920x1080, â ���
        Screen.SetResolution(Width, Heigth, FullScreenMode.Windowed);
    }
}
