using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    public GameObject Socket;

    [SerializeField]
    private MissionRewards missionRewards;

    [SerializeField]
    public TextMeshProUGUI GoldText;

    [SerializeField]
    public TextMeshProUGUI ModifierText;

    private void Start()
    {
        Debug.Log("Victory Screen exists");
        MissionController.Instance.FinishedMission += OnFinishedMission;
        Socket.SetActive(false);
    }

    public void OnFinishedMission()
    {
        if(missionRewards)
        {
            DisplayGoldText();
            DisplayModifierText();
        }

        Socket.SetActive(true);
        MissionController.Instance.CurrentlyRunning = false;
    }

    public void ReceiveRewards()
    {
        missionRewards?.ReceiveRewards();

        CloseMissionScene();
    }

    public void CloseMissionScene()
    {
        if(ShopTab.Instance)
        {
            Scene[] scenes = SceneManager.GetAllScenes();
            foreach (var scene in scenes)
            {
                if (scene.name != "Shop")
                {
                    SceneManager.UnloadSceneAsync(scene.name);
                    ShopTab.Instance.ActivateScene();
                }
            }
        }
    }

    public void DisplayGoldText()
    {
        string message = "+ ";
        message += missionRewards.Gold.Amount.ToString();
        message += " G";

        GoldText.text = message;
    }

    public void DisplayModifierText()
    {
        string message = "";
        foreach (var modifier in missionRewards.ChosenModifiers)
        {
            message += "+ ";
            message += modifier.Name;
            message += " Modifier\n";
        }

        ModifierText.text = message;
    }

}
