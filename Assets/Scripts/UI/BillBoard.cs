using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera cam;

    public bool useStaticBillboard;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (!useStaticBillboard)
        {
            transform.LookAt(cam.transform);

        }
        else
        {
            transform.rotation = cam.transform.rotation;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
