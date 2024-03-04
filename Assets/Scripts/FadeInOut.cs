using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private Image panel;
    private float fadeSpeed = 1f;

    private void Awake()
    {
        panel = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = panel.color.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, alpha);
            yield return null;
        }

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float alpha = panel.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, alpha);
            yield return null;
        }

        ReStart();
    }

    private void ReStart()
    {
        SceneManager.LoadScene("Player");
    }
}
