using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContentVisualizer : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI Header;

    [SerializeField]
    public TextMeshProUGUI Body;

    public void Visualize(Content content)
    {
        Header.text = content.Header;



        string[] strings = content.Body.Split(';');

        content.Body = strings[0];
        for (int i = 1; i < strings.Length; i++)
        {
            content.Body += "\n" + strings[i];

        }
        Body.text = content.Body;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
