using Cells;
using Units;

namespace Grid.GridStates
{
    public abstract class CellGridState
    {
        protected CellGrid _cellGrid;

        protected CellGridState(CellGrid cellGrid)
        {
            _cellGrid = cellGrid;
        }
        public virtual void OnUpdate()
        {

        }

        public virtual void OnUnitClicked(Unit unit)
        {
        }
        public virtual void OnUnitSelected(Unit unit)
        {
        }
        public virtual void OnUnitDeselected(Unit unit)
        {
        }

        public virtual void OnCellDeselected(Cell cell)
        {
            cell.UnMark();
        }
        public virtual void OnCellSelected(Cell cell)
        {
            cell.MarkAsHighlighted();
        }
        public virtual void OnCellClicked(Cell cell)
        {
        }

        public virtual void OnStateEnter()
        {
            foreach (var cell in _cellGrid.Cells)
            {
                cell.UnMark();
            }
        }
        public virtual void OnStateExit()
        {
        }
    }
}