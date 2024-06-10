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
        // 원하는 해상도를 설정합니다. 예: 1920x1080, 창 모드
        Screen.SetResolution(Width, Heigth, FullScreenMode.Windowed);
    }
}
