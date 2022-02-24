using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void SwitchSceneNotSync(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        ShopTab.Instance.DeactivateScene();
    }
}
