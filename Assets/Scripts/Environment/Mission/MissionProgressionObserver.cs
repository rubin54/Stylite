using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class MissionProgressionObserver : MonoBehaviour
{
    [SerializeField]
    private GameObject missionControllerContainer;

    private MissionController missionController;

    public float totalAIHealth = 0; 
    public float totalPlayerHealth = 0;

    private Unit lastUnit;

    private void Awake()
    {
        missionController = missionControllerContainer.GetComponent<MissionController>();
        missionController.FinishedSetup += OnFinishedSetup;
        missionController.StartedNextTurn += OnNextTurn;
    }

    public void OnFinishedSetup()
    {
        foreach (var controller in missionController.PlayerControllers)
        {
            totalPlayerHealth += controller.Unit.TotalHitPoints;
        }
    }

    private void Update()
    {
        float currentHealth = 0;
        foreach (var controller in missionController.PlayerControllers)
        {
            currentHealth += controller.Unit.HitPoints;
        }

        float remainingPercentage = 0;
        if(currentHealth != 0)
        {
            remainingPercentage = currentHealth / totalPlayerHealth;
        }

        AkSoundEngine.SetRTPCValue("UnitHealth", remainingPercentage);

    }

    public void OnNextTurn()
    {
        Unit currentUnit = missionController.SortedInitiatives[0].Unit.Unit;

        //remember currentUnit as lastUnit for NextTurn
        if (missionController.SortedInitiatives.Count > 0)
        {
            lastUnit = currentUnit;
            currentUnit.Ring.GetComponent<Renderer>().material.SetFloat("_Rotation", 1.0f);

            if (CamController.instance.cinematicCam == true)
            {
                CamController.instance.focusTransform = null;
            }
            if(CamController.instance.cinematicCam == false)
            {
                CamController.instance.freeCam = false;

                CamController.instance.focusTransform = currentUnit.transform;
            }


            //missionController.SortedInitiatives[0].Unit.FinishedTurn += StopRINGVFX;
                
            
        }
    }

    public void StopRINGVFX()
    {
        Unit currentUnit = missionController.SortedInitiatives[0].Unit.Unit;
        currentUnit.Ring.GetComponent<Renderer>().material.SetFloat("_Rotation", 0.0f);
    }

}
