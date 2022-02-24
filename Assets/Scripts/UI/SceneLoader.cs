using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    public int levelIndex = 4;

    public GameObject loadingScreen;
    public Slider loadingBar;

    public void LoadScene(int levelIndex)
    {
        StartCoroutine(LoadSceneAsynchronously(levelIndex));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadSceneAsynchronously(levelIndex));
    }

    IEnumerator LoadSceneAsynchronously(int levelIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        //AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        ShopTab.Instance.DeactivateScene();
    }
}
