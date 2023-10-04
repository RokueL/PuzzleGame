using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject canvas;
    public GameObject FadePannel;
    public GameObject tmp;

    private void Start()
    {
        StartCoroutine(FadeOutStartText());
        StartCoroutine(FadeInStartText());
        StartCoroutine(FadeOutStart());
    }

    //���̵� ��

    public IEnumerator FadeInStartText() //�ؽ�Ʈ
    {
        tmp.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.002f)
        {
            Color c = tmp.GetComponent<TextMeshProUGUI>().color;
            c.a = f;
            tmp.GetComponent<TextMeshProUGUI>().color = c;
            yield return null;
        }
        tmp.SetActive(false);
    }

    //���̵� �ƿ�
    public IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(2f);
        FadePannel.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.002f)
        {
            Color c = FadePannel.GetComponent<Image>().color;
            c.a = f;
            FadePannel.GetComponent<Image>().color = c;
            yield return null;
        }
        tmp.SetActive(false);
        canvas.SetActive(false);
    }

    public IEnumerator FadeOutStartText() //�ؽ�Ʈ
    {
        Color t = tmp.GetComponent<TextMeshProUGUI>().color;
        t.a = 0;
        tmp.GetComponent<TextMeshProUGUI>().color = t;
        for (float f = 0f; f < 1; f += 0.002f)
        {
            Color c = tmp.GetComponent<TextMeshProUGUI>().color;
            c.a = f;
            tmp.GetComponent<TextMeshProUGUI>().color = c;
            yield return null;
        }
    }
}

