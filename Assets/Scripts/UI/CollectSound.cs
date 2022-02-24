using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class CollectSound : MonoBehaviour
{
    public AK.Wwise.Event CollectSFX;
    

    public void PlayCollectSound()
    {
        CollectSFX.Post(gameObject);
    }
}
