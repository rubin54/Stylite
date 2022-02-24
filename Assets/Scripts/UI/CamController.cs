using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class CamController : MonoBehaviour
{
    public static CamController instance;

    public Transform cameraTransform;
    public Transform focusTransform;
    public List<Unit> units;

    //public float zoomSpeed;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    [Header("Movement Limits")]
    [Space]
    public bool enableMovementLimits = true;
    //public Vector2 heightLimit;
    public Vector2 lenghtLimit;
    public Vector2 widthLimit;
    private Vector2 zoomLimit;
    public float minZoom;
    public float maxZoom;

    public Quaternion newRotation;
    public Vector3 newPosition;
    public Vector3 newZoom;
    private Vector3 pos;

    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;
    public bool freeCam = false;

    public bool cinematicCam = false;

    public float rotationMax = 60;
    public float rotationMin = -60;
    

    private void Start()
    {
        instance = this;

        newPosition = transform.position;
        newRotation = transform.rotation;
        //newZoom = cameraTransform.localPosition;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cinematicCam = true;

            if (cinematicCam == true)
            {
                focusTransform = null;
                HandleMovementInput();
                HandleMouseInput();
                //HandleRotation();
            }
        }

        if(focusTransform != null && cinematicCam == false)
        {
            if(freeCam == false)
            {
                transform.position = focusTransform.position;
                //HandleRotation();

                if(Input.GetKeyDown(KeyCode.W) ||
                    Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) ||
                    Input.GetKeyDown(KeyCode.D))
                {
                    freeCam = true;
                    focusTransform = null;
                }
            }

        }
        else
        {
            HandleMovementInput();
            HandleMouseInput();
           // HandleRotation();
        }

       //if (Input.GetKey(KeyCode.Escape))
       //{
       //    focusTransform = null;
       //}

        if (enableMovementLimits == true)
        {
            pos = transform.position;
            //pos.y = Mathf.Clamp(pos.y, heightLimit.x, heightLimit.y);
            pos.z = Mathf.Clamp(pos.z, lenghtLimit.x, lenghtLimit.y);
            pos.x = Mathf.Clamp(pos.x, widthLimit.x, widthLimit.y);
            transform.position = pos;
        }

        //newRotation.y = Mathf.Clamp(newRotation.y, rotationMax, rotationMax);

        //Camera.main.fieldOfView -= Input.mouseScrollDelta.y * zoomSpeed;
        //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomLimit.x, zoomLimit.y);
    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mousePosition.y * zoomAmount;
        }

        if (Input.GetMouseButton(1))
        {
            rotateStartPosition = Input.mousePosition;
        }
        //if (Input.GetMouseButtonDown(1))
        //{
        //    rotateCurrentPosition = Input.mousePosition;
        //    Vector3 difference = rotateStartPosition - rotateCurrentPosition;
        //    rotateStartPosition = rotateCurrentPosition;
        //
        //    newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        //}

        newZoom.y = Mathf.Clamp(newZoom.y, -minZoom, maxZoom);
        newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, minZoom);
    }
    
    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            freeCam = true;
            newPosition += transform.forward * movementSpeed;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {

            newPosition += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            newPosition += transform.right * -movementSpeed;
        }



        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        newZoom.y = Mathf.Clamp(newZoom.y, -minZoom, maxZoom);
        newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, minZoom);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    public void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }


        //newRotation.x = Mathf.Clamp(newRotation.x, rotationMax, rotationMax);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);


    }




    public void HandleInputs()
    {
        HandleMovementInput();
        freeCam = true;
        focusTransform = null;
    }
}
