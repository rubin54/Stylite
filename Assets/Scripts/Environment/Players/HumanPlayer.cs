using Grid;
using Grid.GridStates;

namespace Players
{
    public class HumanPlayer : Player
    {
        public override void Play(CellGrid cellGrid)
        {
            cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
        }
    }
}
