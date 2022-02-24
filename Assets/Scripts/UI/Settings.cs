using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider masterVolumeSlider;


    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        masterVolumeSlider.value = masterVolume;
        AkSoundEngine.SetRTPCValue("Master_Volume", masterVolume);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.Save();
    }
    public void SetMusicValue(float value)
    {
        musicVolume = value;
        musicVolumeSlider.value = musicVolume;
        AkSoundEngine.SetRTPCValue("Music_Volume", musicVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXValue(float value)
    {
        sfxVolume = value;
        sfxVolumeSlider.value = sfxVolume;
        AkSoundEngine.SetRTPCValue("SFX_Volume", sfxVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
