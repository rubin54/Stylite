using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Cells;
using Pathfinding.Algorithms;
using Units.UnitStates;
using TMPro;

namespace Units
{

    [ExecuteInEditMode]
    public abstract class Unit : MonoBehaviour
    {
        #region Schnittstelle

        [SerializeField]
        public GameObject Model;

        [SerializeField]
        public GameObject ShatteredPrefab;

        [SerializeField]
        public Collider Collider;

        [SerializeField]
        public GameObject Ring;

        public Action<Unit> Died;

        public UnitType Type = UnitType.Melee;

        public int Initiative = 0;
        public SpellTemplate[] Spells = new SpellTemplate[UnitTemplate.MaxSpellCount];
        public ModifierTemplate[] Modifiers = new ModifierTemplate[UnitTemplate.MaxSpellCount];

        public UnitTemplate UnitTemplate;

        public bool isDead = false;

        private void Update()
        {
            if(isDead && !IsMoving)
            {
                cell.IsTaken = false;
                cell.CurrentUnit = null;
                cell = null;
                Destroy(this);
            }
        }

        public virtual void Setup(UnitTemplate template)
        {
            for (int i = 0; i < Spells.Length; i++)
            {
                if (Spells[i])
                {
                    template.SetSpell(Spells[i], i);
                }
            }

            for (int i = 0; i < Modifiers.Length; i++)
            {
                if (Modifiers[i])
                {
                    template.AddModifier(Modifiers[i], i);
                }
            }

            Spells = template.Spells;
            Modifiers = template.Modifiers;
            hitPoints = template.Health;
            UnitTemplate = template;
        }

        public void ReceiveDamage(int amount)
        {
            hitPoints -= amount;

            if(FloatingTextPrefab != null)
            {
                ShowFloatingText(amount);
            }

            if(hitPoints <= 0)
            {
                Die();
            }
        }

        public GameObject FloatingTextPrefab;

        public void ShowFloatingText(int amount)
        {
            var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = amount.ToString();
            //go.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        }

        public void Die()
        {
            cell.CurrentUnit = null;
            cell.IsTaken = false;
            UnitTemplate.Died?.Invoke(UnitTemplate);
            Died?.Invoke(this);
        }

        public void Heal(int amount)
        {
            RestSFX.Post(gameObject);
            hitPoints += amount;
            if (TotalHitPoints < hitPoints + amount) hitPoints = TotalHitPoints;
        }

        public void ResetActionPoints()
        {
            actionPoints = TotalActionPoints;
        }

        public bool ReduceActionPoints(int value)
        {
            bool retVal = value <= actionPoints;

            if (retVal) actionPoints -= value;

            return retVal;
        }

        public float GetPathCost(List<Cell> path)
        {
            return path.Sum(h => h.MovementCost);
        }


        public void QueueDeath()
        {
            isDead = true;
            Destroy(Collider);
            Destroy(Ring);
            Destroy(Model);
            GameObject createdObject = Instantiate(ShatteredPrefab, transform);
            createdObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        #endregion Schnittstelle

        public AK.Wwise.Event RestSFX;

        private List<Unit> teammates;
        public List<Unit> Teammates { get => teammates; }

        public void SetTeammates(List<Unit> units)
        {
            teammates = units;
        }

        Dictionary<Cell, List<Cell>> cachedPaths = null;

        public event EventHandler UnitClicked;

        public event EventHandler UnitSelected;

        public event EventHandler UnitDeselected;
 
        public event EventHandler UnitHighlighted;

        public event EventHandler UnitDehighlighted;

        public event EventHandler<MovementEventArgs> UnitMoved;

        public UnitState UnitState { get; set; }
        public void SetState(UnitState state)
        {
            //UnitState.MakeTransition(state);
        }

        public int TotalHitPoints { get; private set; }
        public float TotalActionPoints { get; private set; }


        [SerializeField]
        [HideInInspector]
        private Cell cell;
        public Cell Cell
        {
            get
            {
                return cell;
            }
            set
            {
                cell = value;
            }
        }

        private int hitPoints;
        public int HitPoints { get => hitPoints; }
        public int FieldOfView;

        public float MovementSpeed;

        [SerializeField]
        private float actionPoints = 1;
        public float ActionPoints
        {
            get
            {
                return actionPoints;
            }
            set
            {
                actionPoints = value;
            }
        }

        public int PlayerNumber;

        CameraController cam;

        public bool IsMoving { get; set; }

        private static DijkstraPathfinding _pathfinder = new DijkstraPathfinding();
        private static IPathfinding _fallbackPathfinder = new AStarPathfinding();


        public virtual void Initialize()
        {
            UnitState = new UnitStateNormal(this);

            hitPoints = UnitTemplate.Health;
            TotalHitPoints = HitPoints;
 
            TotalActionPoints = ActionPoints;
            actionPoints = -1;

        }
        #region Mouse Methodes
        protected virtual void OnMouseDown()
        {
            if (UnitClicked != null)
            {
                UnitClicked.Invoke(this, new EventArgs());
            }

            //CameraController.instance.focusTransform = transform;

            //CamController.instance.focusTransform = transform;
            //CamController.instance.zoomAmount = new Vector3(10,10,0);
        }
        protected virtual void OnMouseEnter()
        {
            if (UnitHighlighted != null)
            {
                UnitHighlighted.Invoke(this, new EventArgs());
            }
        }
        protected virtual void OnMouseExit()
        {
            if (UnitDehighlighted != null)
            {
                UnitDehighlighted.Invoke(this, new EventArgs());
            }
        }
        #endregion

        public virtual void OnTurnStart()
        {
            ActionPoints = TotalActionPoints;

            SetState(new UnitStateMarkedAsFriendly(this));
        }
 
        public virtual void OnTurnEnd()
        {
            cachedPaths = null;

            SetState(new UnitStateNormal(this));
        }

        protected virtual void OnDestroyed()
        {
            Cell.IsTaken = false;
            MarkAsDestroyed();
            Destroy(gameObject);
        }


        public virtual void OnUnitSelected()
        {
            SetState(new UnitStateMarkedAsSelected(this));
            if (UnitSelected != null)
            {
                UnitSelected.Invoke(this, new EventArgs());
            }
        }

        public virtual void OnUnitDeselected()
        {
            SetState(new UnitStateMarkedAsFriendly(this));
            if (UnitDeselected != null)
            {
                UnitDeselected.Invoke(this, new EventArgs());
            }
        }

        public virtual void Move(Cell destinationCell, List<Cell> path)
        {
            var totalMovementCost = path.Sum(h => h.MovementCost);
            ActionPoints -= totalMovementCost;

            ForceMovement(destinationCell, path, false);
        }

        public void ForceMovement(Cell destinationCell, List<Cell> path, bool forced = true)
        {


            foreach (Hexagon hexagon in path)
            {
                if(hexagon.MovementCost == 99)
                {
                    Die();
                }

                if(hexagon.OnFire)
                {
                    ReceiveDamage(1);
                }
            }

            if (MovementSpeed > 0)
            {
                StartCoroutine(Movement(path));
            }
            else
            {
                Cell = destinationCell;
                transform.position = Cell.transform.position;
            }

            Cell.IsTaken = false;
            Cell.CurrentUnit = null;
            Cell = destinationCell;
            destinationCell.IsTaken = true;
            destinationCell.CurrentUnit = this;

            if (UnitMoved != null)
            {
                UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));
            }
        }

        protected virtual IEnumerator Movement(List<Cell> path)
        {
            int pathCount = path.Count;
            IsMoving = true;
            path.Add(cell);
            path.Reverse();
            path.Add(path[path.Count - 1]);

            Cell[] pathCopy = path.ToArray();

            List<List<Cell>> chains = new List<List<Cell>>();
            Vector3 currentDirection = Vector3.zero;
            List<Cell> currentChain = new List<Cell>();
            chains.Add(currentChain);
            for (int i = 0; i < pathCopy.Length -2; i++)
            {
                Vector3 calculatedDirection = Hexagon.GetCellPosition(pathCopy[i]) - Hexagon.GetCellPosition(pathCopy[i+1]);

                if(currentDirection != calculatedDirection)
                {
                    currentChain = new List<Cell>();
                    chains.Add(currentChain);
                    currentChain.Add(pathCopy[i]);
                }

                currentChain.Add(pathCopy[i + 1]);

                currentDirection = calculatedDirection;
            }


            foreach (var chain in chains)
            {
                while (chain.Count > 2)
                {
                    path.Remove(chain[1]);
                    chain.Remove(chain[1]);
                }
            }
            //if(pathCount < 4)
            //{
            //    path.Remove(cell);
            //}

            for (int i = 0; i < path.Count - 1; i++)
            {
                Cell pathCell = path[i];

                Vector3 startingPos = transform.position;
                Vector3 destination_pos = pathCell.transform.position;

                Vector3 destinationDistance = (destination_pos - startingPos);
                //destinationDistance.y = 0;
                float destinationMagnitude = destinationDistance.magnitude;

                float angle = Mathf.Atan2(destinationDistance.x, destinationDistance.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler( new Vector3(0, angle, 0));

                Vector3 currentDistance = (startingPos - transform.position);
                //currentDistance.y = 0;
                float currentMagnitude = currentDistance.magnitude;
                while (destinationMagnitude > currentMagnitude)
                {
                    currentDistance = (startingPos - transform.position);
                    //currentDistance.y = 0;
                    currentMagnitude = currentDistance.magnitude;

                    float heightModifier =  Mathf.Abs(0.7f - Mathf.Abs(0.7f - ((currentMagnitude + 0.001f) / destinationMagnitude) * 1.4f));
                    float movementSpeedModifier = Mathf.Abs(0.85f - ((currentMagnitude + 0.001f) / destinationMagnitude) * 1.7f);
                    float poweredSpeedModifier = movementSpeedModifier * movementSpeedModifier * 3;
                    float addedMovementSpeedModifier = 0.15f + poweredSpeedModifier;
                    transform.position += (destinationDistance) * Time.deltaTime * MovementSpeed * addedMovementSpeedModifier;
                    transform.position = new Vector3(transform.position.x, ((path[i].transform.position.y - path[i+1].transform.position.y) * currentMagnitude) + heightModifier + 0.3f, transform.position.z); 
                    yield return 0;
                }
            }

            transform.position = path[path.Count - 1].transform.position;
            IsMoving = false;
            OnMoveFinished();
            yield return 0;
        }

        protected virtual void OnMoveFinished() { }

        public virtual bool IsCellMovableTo(Cell cell)
        {
            return !cell.IsTaken;
        }

        public virtual bool IsCellTraversable(Cell cell)
        {
            return !cell.IsTaken;
        }
        public HashSet<Cell> GetAvailableDestinations(List<Cell> cells)
        {
            cachedPaths = new Dictionary<Cell, List<Cell>>();

            var paths = CachePaths(cells);
            foreach (var key in paths.Keys)
            {
                if (!IsCellMovableTo(key))
                {
                    continue;
                }
                var path = paths[key];

                var pathCost = path.Sum(c => c.MovementCost);
                if (pathCost <= ActionPoints)
                {
                    cachedPaths.Add(key, path);
                }
            }
            return new HashSet<Cell>(cachedPaths.Keys);
        }

        private Dictionary<Cell, List<Cell>> CachePaths(List<Cell> cells)
        {
            var edges = GetGraphEdges(cells);
            var paths = _pathfinder.findAllPaths(edges, Cell);
            return paths;
        }

        public List<Cell> FindPath(List<Cell> cells, Cell destination)
        {
            if (cachedPaths != null && cachedPaths.ContainsKey(destination))
            {
                return cachedPaths[destination];
            }
            else
            {
                return _fallbackPathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
            }
        }
        protected virtual Dictionary<Cell, Dictionary<Cell, float>> GetGraphEdges(List<Cell> cells)
        {
            Dictionary<Cell, Dictionary<Cell, float>> ret = new Dictionary<Cell, Dictionary<Cell, float>>();
            foreach (var cell in cells)
            {
                if (IsCellTraversable(cell) || cell == Cell)
                {
                    ret[cell] = new Dictionary<Cell, float>();
                    foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                    {
                        ret[cell][neighbour] = neighbour.MovementCost;
                    }
                }
            }
            return ret;
        }

        #region Abstract Methodes
        public abstract void MarkAsDefending(Unit aggressor);

        public abstract void MarkAsAttacking(Unit target);

        public abstract void MarkAsDestroyed();


        public abstract void MarkAsFriendly();

        public abstract void MarkAsReachableEnemy();

        public abstract void MarkAsSelected();

        public abstract void MarkAsFinished();

        public abstract void UnMark();
        #endregion

        [ExecuteInEditMode]
        public void OnDestroy()
        {
            if (Cell != null)
            {
                Cell.IsTaken = false;
            }
        }
        public enum UnitType
        {
            Melee,
            Range,
            Support,
            Universal
        }
    }
    public class MovementEventArgs : EventArgs
    {
        public Cell OriginCell;
        public Cell DestinationCell;
        public List<Cell> Path;

        public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path)
        {
            OriginCell = sourceCell;
            DestinationCell = destinationCell;
            Path = path;
        }
    }
    public class UnitCreatedEventArgs : EventArgs
    {
        public Transform unit;

        public UnitCreatedEventArgs(Transform unit)
        {
            this.unit = unit;
        }
    }
}
