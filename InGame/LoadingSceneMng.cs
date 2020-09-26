using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneMng : MonoBehaviour
{
    public static string nextScene;
    private Slider m_slider;
    private void Start()
    {
        m_slider = GameObject.Find("Canvas/Slider").GetComponent<Slider>();
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                m_slider.value = Mathf.Lerp(m_slider.value, op.progress, timer);
                if (m_slider.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                m_slider.value = Mathf.Lerp(m_slider.value, 1f, timer);
                if (m_slider.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
