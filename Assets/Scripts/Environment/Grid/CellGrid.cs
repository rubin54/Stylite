using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Cells;
using Grid.GridStates;
using Grid.UnitGenerators;
using Players;
using Units;
using UnityEngine.EventSystems;

namespace Grid
{

    public class CellGrid : MonoBehaviour
    {
        public Action<Unit> ClickedUnit;

        public Action<Cell> ClickedCell;

 
        public event EventHandler GameStarted;

        public event EventHandler GameEnded;
  
        public event EventHandler TurnEnded;

        public event EventHandler<UnitCreatedEventArgs> UnitAdded;

        public static CellGrid Instance;

        private CellGridState _cellGridState; 
        public CellGridState CellGridState
        {
            get
            {
                return _cellGridState;
            }
            set
            {
                if (_cellGridState != null)
                    _cellGridState.OnStateExit();
                _cellGridState = value;
                _cellGridState.OnStateEnter();
            }
        }

        public int NumberOfPlayers { get; private set; }

        public Player CurrentPlayer
        {
            get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
        }
        public int CurrentPlayerNumber { get; private set; }

        public Transform PlayersParent;

        public bool GameFinished { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Cell> Cells { get; private set; }
        public List<Unit> Units { get; private set; } = new List<Unit>();

        private void OnEnable()
        {
            Instance = this;

            Initialize();
        }

        private void Update()
        {
            if (_cellGridState != null) _cellGridState.OnUpdate();
        }

        private void Initialize()
        {

            Cells = new List<Cell>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
                if (cell != null)
                    Cells.Add(cell);
                else
                {
                    Destroy(transform.GetChild(i).gameObject);
                    Debug.Log("Invalid object in cells parent game object, deleting them");
                }
            }


            foreach (var cell in Cells)
            {
                cell.Setup();
                cell.CellClicked += OnCellClicked;
                cell.CellHighlighted += OnCellHighlighted;
                cell.CellDehighlighted += OnCellDehighlighted;
            }

            foreach (var cell in Cells)
            {
                cell.GetNeighbours(Cells);
            }

            Units = new List<Unit>();
            var unitGenerator = GetComponent<IUnitGenerator>();
            if (unitGenerator != null)
            {
                var units = unitGenerator.SpawnUnits(Cells);
                foreach (var unit in units)
                {
                    AddUnit(unit);
                }
            }
            else
                Debug.LogError("No IUnitGenerator script attached to cell grid");
        }

        private void OnCellDehighlighted(object sender, EventArgs e)
        {
            CellGridState.OnCellDeselected(sender as Cell);
        }
        private void OnCellHighlighted(object sender, EventArgs e)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CellGridState.OnCellSelected(sender as Cell);
            }
        }
        public void OnCellClicked(object sender, EventArgs e)
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                CellGridState.OnCellClicked(sender as Cell);
                ClickedCell?.Invoke(sender as Cell);
            }
        }

        public void OnUnitClicked(object sender, EventArgs e)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CellGridState.OnUnitClicked(sender as Unit);
                ClickedUnit?.Invoke(sender as Unit);
            }
        }

        public void OnUnitHighlighted(object sender, EventArgs e)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CellGridState.OnUnitSelected(sender as Unit);
            }
        }

        public void OnUnitDehighlighted(object sender, EventArgs e)
        {
            CellGridState.OnUnitDeselected(sender as Unit);
        }

        private void OnUnitDestroyed(object sender, EventArgs e)
        {
            Units.Remove(sender as Unit);
            var totalPlayersAlive = Units.Select(u => u.PlayerNumber).Distinct().ToList(); 
            if (totalPlayersAlive.Count == 1)
            {
                CellGridState = new CellGridStateBlockInput(this);
                GameFinished = true;
                if (GameEnded != null)
                    GameEnded.Invoke(this, new EventArgs());
            }
        }


        public void AddUnit(Unit unit)
        {
            unit.UnitClicked += OnUnitClicked;
            unit.UnitHighlighted += OnUnitHighlighted;
            unit.UnitDehighlighted += OnUnitDehighlighted;
            //unit.GetComponent<Unit>().UnitDestroyed += OnUnitDestroyed;

            if (UnitAdded != null)
                UnitAdded.Invoke(this, new UnitCreatedEventArgs(unit.transform));
        }

        public Hexagon GetHexagon(Vector3 position)
        {
            Hexagon retVal = null;

            foreach (Hexagon cell in Cells)
            {
                if(cell.CubeCoord == position)
                {
                    retVal = cell;
                    break;
                }
            }

            return retVal;
        }
    }
}

