using Grid;
using UnityEngine;

namespace Players
{
    public abstract class Player : MonoBehaviour
    {
        public int PlayerNumber;
 
        public abstract void Play(CellGrid cellGrid);
    }
}