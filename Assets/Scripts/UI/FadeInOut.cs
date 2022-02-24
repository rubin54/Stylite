using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public void LoadNextLevel(string name)
    {
        StartCoroutine(LevelLoad(name));
    }

    IEnumerator LevelLoad(string name)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    public void OnClick()
    {
       AkSoundEngine.SetRTPCValue("Shop_Looper", 0f);
    }


}
