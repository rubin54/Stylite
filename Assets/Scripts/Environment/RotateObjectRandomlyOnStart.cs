using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectRandomlyOnStart : MonoBehaviour
{
    [SerializeField]
    public float Directions = 6;

    private void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(this.gameObject.transform.rotation.eulerAngles.x, 30 + ((360/Directions) * Mathf.Round(Random.Range(0,Directions))), this.gameObject.transform.rotation.eulerAngles.z));
    }
}
