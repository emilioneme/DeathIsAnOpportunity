using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerFade : MonoBehaviour
{
    public static SceneManagerFade Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure fadeCanvasGroup is set up
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeSceneManager: Missing CanvasGroup for fading!");
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScenes(sceneName));
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(Fade(1f));

        // Load scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade back in
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    } 
}
