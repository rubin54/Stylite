using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMissionController : MissionController
{
    [SerializeField]
    public int SceneIndex = 1;

    public override void NextTurn()
    {
        CurrentlyRunning = true;
        if (SortedInitiatives.Count > 0)
        {
            SortedInitiatives[0].Unit.FinishedTurn += OnFinishedTurn;

            SortedInitiatives[0].Unit.Unit.Cell.UnitStartsTurn(SortedInitiatives[0].Unit.Unit);
            SortedInitiatives[0].Unit.StartAction();
            ConcludeActions += SortedInitiatives[0].Unit.Act;

            StartedNextTurn?.Invoke();
        }
        else
        {
            if (aiControllers.Count == 0)
            {
                SceneManager.LoadScene(SceneIndex);
            }

            if (playerControllers.Count == 0)
            {
                SceneManager.LoadScene(menuSceneIndex);
                return;
            }

            NewRound();
        }
    }

}
