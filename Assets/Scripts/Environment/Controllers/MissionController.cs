using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Grid.GridStates;

public class UnitEntry
{
    public Controller Unit;
    public int init = 0;

    public UnitEntry(Controller unit)
    {
        Unit = unit;
    }

    public void Roll()
    {
        int firebonus = Unit.Unit.Cell.OnFire ? 3 : 0;
        init = Unit.Unit.Initiative + Unit.Unit.Cell.InitiativeModifier + Mathf.RoundToInt(UnityEngine.Random.Range(2, 12)) + firebonus;
    }
}

public class MissionController : MonoBehaviour
{
    public Action StartedNextRound;
    public Action StartedNextTurn;
    public Action FinishedMission;
    public Action FinishedSetup;
    protected Action ConcludeActions;

    public static MissionController Instance;

    [SerializeField]
    protected List<AiController> aiControllers;

    [SerializeField]
    protected List<AiSpawnerWithRandomizedContent> aiSpawnerWithRandomizedContents;

    [SerializeField]
    protected List<PlayerController> playerControllers;

    [SerializeField]
    protected UnitCreator unitCreator;

    [SerializeField]
    protected CellGrid cellGrid;

    [SerializeField]
    protected EffectSocket effectSocket;

    [SerializeField]
    protected UnitInfoTab unitInfoTab;

    [SerializeField]
    protected RestingInput restingInput;

    [SerializeField]
    protected SpellCastTab spellTab;

    [SerializeField]
    protected float waitingTimeBetweenTurns = 0.1f;

    [SerializeField]
    protected float currentTime = 0.0f;

    [SerializeField]
    protected int menuSceneIndex = 0;

    public List<AiController> AiControllers { get => aiControllers; }
    public List<PlayerController> PlayerControllers { get => playerControllers; }

    public List<UnitEntry> SortedInitiatives = new List<UnitEntry>();

    public bool CurrentlyRunning = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        foreach (var aiSpawnerWithRandomizedContent in aiSpawnerWithRandomizedContents)
        {
            foreach (var ais in aiSpawnerWithRandomizedContent.Spawn())
            {
                aiControllers.Add(ais);
            }
        }

        cellGrid.CellGridState = new CellGridStateBlockInput(cellGrid);

        if (effectSocket)
        {
            effectSocket.StopTimeForEffect += StopTime;
            effectSocket.ContinueTimeForEffect += RewindTime;
        }

        if (unitCreator)
        {
            unitCreator.CreatedUnit += OnCreatedPlayerUnit;
        }

        if (aiControllers == null)
        {
            aiControllers = new List<AiController>();
        }

        if (playerControllers == null)
        {
            playerControllers = new List<PlayerController>();
        }
        else
        {
            List<Unit> playerTeammates = new List<Unit>();
            foreach (var player in playerControllers)
            {
                player.Unit.Setup(player.Unit.UnitTemplate);
                playerTeammates.Add(player.Unit);
            }

            foreach (var playerController in playerControllers)
            {
                playerController.Died += OnPlayerControllerDeath;
                playerController.Setup(cellGrid, spellTab, unitInfoTab, restingInput);
                playerController.Unit.SetTeammates(playerTeammates);
            }
        }

        unitCreator?.Setup();

        foreach (var aiController in aiControllers)
        {
            aiController.Died += OnAiControllerDeath;
            aiController.Setup(cellGrid, spellTab, unitInfoTab, restingInput);
            List<Unit> teammates = new List<Unit>();

            foreach (var ai in aiControllers)
            {
                teammates.Add(ai.Unit);
            }

            aiController.Unit.SetTeammates(teammates);



            List<Unit> enemies = new List<Unit>();
            foreach (var player in playerControllers)
            {
                enemies.Add(player.Unit);
            }
            aiController.AISetup(enemies);
        }

        NewRound();
        FinishedSetup?.Invoke();
    }

    private void Update()
    {
        if(currentTime >= 0.0f)
        {
            currentTime += Time.deltaTime;
        }

        if(currentTime >= waitingTimeBetweenTurns)
        {
            currentTime = -1f;
            NextTurn();
        }

        if(CurrentlyRunning)
        {

            if (SortedInitiatives.Count > 0 && !SortedInitiatives[0].Unit.Unit.IsMoving)
            {
                ConcludeActions?.Invoke();
            }
        }
    }

    public void NewRound()
    {
        CalculateInitiative();
        StartedNextRound?.Invoke();
        currentTime = 0.0f;
    }

    public virtual void NextTurn()
    {
        CurrentlyRunning = true;
        if(SortedInitiatives.Count > 0)
        {
            SortedInitiatives[0].Unit.FinishedTurn += OnFinishedTurn;

            SortedInitiatives[0].Unit.Unit.Cell.UnitStartsTurn(SortedInitiatives[0].Unit.Unit);
            SortedInitiatives[0].Unit.StartAction();
            ConcludeActions += SortedInitiatives[0].Unit.Act;

            StartedNextTurn?.Invoke();
        }
        else
        {
            if(CheckForFinishedMissionCondition())
            {
                return;
            }

            NewRound();
        }
    }

    public void OnFinishedTurn(Controller controller)
    {
        StartCoroutine(WaitForEndOfMovement(controller));
    }



    protected IEnumerator WaitForEndOfMovement(Controller controller)
    {
        while(controller.Unit.IsMoving)
        {
            yield return 0;
        }

        SortedInitiatives.Remove(SortedInitiatives[0]);
        ConcludeActions -= controller.Act;
        controller.FinishedTurn -= OnFinishedTurn;
        currentTime = 0.0f;
    }


    public void OnCreatedPlayerUnit(Unit unit)
    {
        cellGrid.AddUnit(unit);

        PlayerController controller = unit.GetComponent<PlayerController>();
        controller.Setup(cellGrid, spellTab, unitInfoTab, restingInput);
        controller.Died += OnPlayerControllerDeath;
        playerControllers.Add(controller);
    }

    private void CalculateInitiative()
    {
        List<UnitEntry> allUnits = new List<UnitEntry>();

        foreach(AiController controller in aiControllers)
        {
            allUnits.Add(new UnitEntry(controller));
        }

        foreach (PlayerController controller in playerControllers)
        {
            allUnits.Add(new UnitEntry(controller));
        }

        foreach (var unit in allUnits)
        {
            unit.Roll();
        }

        while (AnyDouble(allUnits).Item1)
        {
            AnyDouble(allUnits).Item2.Roll();
        }

        SortAllEntries(allUnits);
    }

    private (bool, UnitEntry) AnyDouble(List<UnitEntry> units)
    {
        foreach (var unit in units)
        {
            foreach (var unitx in units)
            {
                if (unit != unitx)
                {
                    if (unit.init == unitx.init) return (true, unit);
                }
            }
        }

        return (false, null);
    }

    private void SortAllEntries(List<UnitEntry> units)
    {
        SortedInitiatives.Clear();

        for (int i = 0; units.Count != 0; i++)
        {
            UnitEntry highestUnit = GetHighestEntry(units);

            SortedInitiatives.Add(highestUnit);

            units.Remove(highestUnit);
        }
    }

    private UnitEntry GetHighestEntry(List<UnitEntry> units)
    {
        int highestInit = int.MinValue;
        UnitEntry highestUnit = null;
        foreach (UnitEntry unit in units)
        {
            if (unit.init > highestInit)
            {
                highestInit = unit.init;
                highestUnit = unit;
            }
        }
        return highestUnit;
    }

    public void StopTime()
    {
        CurrentlyRunning = false;
    }

    public void RewindTime()
    {
        CurrentlyRunning = true;
    }

    public void OnAiControllerDeath(Controller controller)
    {
        if(SortedInitiatives.Count > 0)
        {
            if (SortedInitiatives[0].Unit == controller)
            {
                OnFinishedTurn(controller);
            }
        }


        if (aiControllers.Contains((AiController)controller))
        {
            aiControllers.Remove((AiController)controller);
        }


        controller.Unit.QueueDeath();
        OnControllerDeath(controller);

        CheckForFinishedMissionCondition();
    }

    public void OnPlayerControllerDeath(Controller controller)
    {
        if (SortedInitiatives[0].Unit == controller)
        {
            OnFinishedTurn(controller);
        }

        if (playerControllers.Contains((PlayerController)controller))
        {
            playerControllers.Remove((PlayerController)controller);
        }


        controller.Unit.QueueDeath();
        OnControllerDeath(controller);

        CheckForFinishedMissionCondition();
    }

    public void OnControllerDeath(Controller controller)
    {
        foreach (var entry in SortedInitiatives.ToArray())
        {
            if(entry.Unit == controller)
            {
                SortedInitiatives.Remove(entry);
            }
        }

        foreach (var aiController in aiControllers)
        {
            aiController.OnUnitDeath(controller.Unit);
        }

        foreach (var playerController in playerControllers)
        {
            playerController.OnUnitDeath(controller.Unit);
        }
    }

    public bool CheckForFinishedMissionCondition()
    {
        if (aiControllers.Count == 0)
        {
            FinishedMission?.Invoke();
            return true;
        }

        if (playerControllers.Count == 0)
        {
            SceneManager.LoadScene(menuSceneIndex);
            return true;
        }

        return false;
    }
}
