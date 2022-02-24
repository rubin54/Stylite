using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceObserver : MonoBehaviour
{
    [SerializeField]
    private Resource resource;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private string suffix = "G";

    public AK.Wwise.Event CollectSFX;

    void Start()
    {
        resource.ChangedAmount += OnChangedAmount;
        OnChangedAmount(resource.Amount, 0);
    }

    public void OnChangedAmount(float amount, float changedAmount)
    {
        CollectSFX.Post(gameObject);

        if(text)
        {
            text.text = amount.ToString() + suffix;
        }
        else
        {
            Debug.LogError("Text in " + gameObject.name + "/ResourceObserver isnt set");
        }
    }
}
