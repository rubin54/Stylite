using UnityEngine;
using System;
using System.Collections.Generic;
using Pathfinding.Data;
using Units;

namespace Cells
{

    public abstract class Cell : MonoBehaviour, IGraphNode
    {
        [HideInInspector]
        [SerializeField]
        private Vector2 _offsetCoord;
        public Vector2 OffsetCoord { get { return _offsetCoord; } set { _offsetCoord = value; } }

        [SerializeField]
        private Hitbox hitbox;

        [SerializeField]
        private GameObject firePrefab;

        private GameObject currentFire;

        public int InitiativeModifier;
        public bool IsTaken;
        public bool IsConductive;
        public bool isFlamable;
        private bool onFire;

        public bool OnFire
        {
            set
            {
                if(isFlamable)
                {
                    onFire = value;
                }

                if(OnFire && currentFire == null)
                {
                    currentFire = Instantiate(firePrefab, transform);
                    currentFire.transform.position = transform.position;
                }

                if(!OnFire && currentFire)
                {
                    Destroy(currentFire);
                }
            }
            get
            {
                return onFire;
            }
        }
        public int DamageByForcedTraversal = 0;

        public float MovementCost = 1;

        public Unit CurrentUnit { get; set; }
        #region Events
        public event EventHandler CellClicked;

        public event EventHandler CellHighlighted;
  
        public event EventHandler CellDehighlighted;
        #endregion Events

        #region Mouse Methodes
        public void FocusCell()
        {
            OnMouseEntered();
        }

        public void DefocusCell()
        {
            OnMouseExited();
        }

        protected virtual void OnMouseEntered() => CellHighlighted?.Invoke(this, new EventArgs());

        protected virtual void OnMouseExited() => CellDehighlighted?.Invoke(this, new EventArgs());
        public void OnMouseClicked() => CellClicked?.Invoke(this, new EventArgs());

        #endregion Mouse Methodes
        #region Abstract Methodes

        public abstract void Setup();

        public abstract void UnitStartsTurn(Unit unit);

        public abstract int GetDistance(Cell other);
        public abstract List<Cell> GetNeighbours(List<Cell> cells);

        public abstract void MarkAsReachable();
    
        public abstract void MarkAsPath();
        
        public abstract void MarkAsHighlighted();

        public abstract void MarkAsHighlightedSpells();


        public abstract void UnMark();
        #endregion Abstract methodes

        private void OnEnable()
        {
            if (hitbox)
            {
                hitbox.MouseClicked += OnMouseClicked;
                hitbox.MouseEntered += OnMouseEntered;
                hitbox.MouseExited += OnMouseExited;
            }
        }

        private void OnDisable()
        {
            if (hitbox)
            {
                hitbox.MouseClicked -= OnMouseClicked;
                hitbox.MouseEntered -= OnMouseEntered;
                hitbox.MouseExited -= OnMouseExited;
            }
        }

        public void DehighlightCell() => CellDehighlighted?.Invoke(this, new EventArgs());

        public int GetDistance(IGraphNode other)
        {
            return GetDistance(other as Cell);
        }

        public void OnCellClicked()
        {
            CellClicked?.Invoke(this, new EventArgs());
        }

        public abstract Content GetContent();
    }
}