using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionShader : MonoBehaviour
{
    private Material material;
    private Color previousColor;

    

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;

    }


}
