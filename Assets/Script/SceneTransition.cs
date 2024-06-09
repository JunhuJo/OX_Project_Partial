using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image transitionImage;
    public float transitionDuration = 1.0f;
    public bool isGame = false;

    private void Start()
    {
        // �ʱ� ����
        transitionImage.rectTransform.localScale = Vector3.zero;
    }

    public void TransitionToScene()
    {
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        // ȭ���� �˰� ä��� �ִϸ��̼�
        yield return StartCoroutine(AnimateTransition(Vector3.one, 0, 0));

        // ȭ���� �ٽ� ���׶��� �����ִ� �ִϸ��̼�
       yield return StartCoroutine(AnimateTransition(Vector3.zero, 12000, 12000));
    }

    private IEnumerator AnimateTransition(Vector3 targetScale, float targetWidth, float targetHeight)
    {
        float timeElapsed = 0;
        Vector3 initialScale = transitionImage.rectTransform.localScale;

        Vector2 initialSize = transitionImage.rectTransform.sizeDelta;
        Vector2 targetSize = new Vector2(targetWidth, targetHeight);

        while (timeElapsed < transitionDuration)
        {
            transitionImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / transitionDuration);
            transitionImage.rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, timeElapsed / transitionDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transitionImage.rectTransform.localScale = targetScale;
        transitionImage.rectTransform.sizeDelta = targetSize;
    }

    public void OnClick_SceneChage()
    {
        if (isGame)
        {
            FindObjectOfType<SceneTransition>().TransitionToScene();
        }
    }
}
