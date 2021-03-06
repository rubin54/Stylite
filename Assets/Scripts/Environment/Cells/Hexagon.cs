using System.Collections.Generic;
using UnityEngine;

namespace Cells
{
    public abstract class Hexagon : Cell
    {
        List<Cell> neighbours = null;

        [HideInInspector]
        public HexGridType HexGridType;

        public Vector3 CubeCoord
        {
            get
            {
                Vector3 ret = new Vector3();
                switch (HexGridType)
                {
                    case HexGridType.odd_q:
                        {
                            //original
                            //ret.x = OffsetCoord.x;
                            //ret.z = OffsetCoord.y - (OffsetCoord.x + (Mathf.Abs(OffsetCoord.x) % 2)) / 2;
                            //ret.y = -ret.x - ret.z;
                            
                            ret.x = OffsetCoord.x;
                            ret.z = OffsetCoord.y - (OffsetCoord.x + (Mathf.Abs(OffsetCoord.x) % 2)) / 2;
                            ret.y = -ret.x - ret.z;
                            break;
                        }
                    case HexGridType.even_q:
                        {
                            ret.x = OffsetCoord.x;
                            ret.z = OffsetCoord.y - (OffsetCoord.x - (Mathf.Abs(OffsetCoord.x) % 2)) / 2;
                            ret.y = -ret.x - ret.z;
                            break;
                        }
                }
                return ret;
            }
        }

        protected Vector2 CubeToOffsetCoords(Vector3 cubeCoords)
        {
            Vector2 ret = new Vector2();

            switch (HexGridType)
            {
                case HexGridType.odd_q:
                    {
                        ret.x = cubeCoords.x;
                        ret.y = cubeCoords.z + (cubeCoords.x + (Mathf.Abs(cubeCoords.x) % 2)) / 2;
                        break;
                    }
                case HexGridType.even_q:
                    {
                        ret.x = cubeCoords.x;
                        ret.y = cubeCoords.z + (cubeCoords.x - (Mathf.Abs(cubeCoords.x) % 2)) / 2;
                        break;
                    }
            }
            return ret;
        }

        public static readonly Vector3[] _directions =  {
        new Vector3(+1, -1, 0), new Vector3(+1, 0, -1), new Vector3(0, +1, -1),
        new Vector3(-1, +1, 0), new Vector3(-1, 0, +1), new Vector3(0, -1, +1)};

        public override int GetDistance(Cell other)
        {
            var _other = other as Hexagon;
            int distance = (int)(Mathf.Abs(CubeCoord.x - _other.CubeCoord.x) + Mathf.Abs(CubeCoord.y - _other.CubeCoord.y) + Mathf.Abs(CubeCoord.z - _other.CubeCoord.z)) / 2;
            return distance;
        }//Distance is given using Manhattan Norm.

        public override List<Cell> GetNeighbours(List<Cell> cells)
        {
            if (neighbours == null)
            {
                neighbours = new List<Cell>(6);
                foreach (var direction in _directions)
                {
                    //var neighbour = cells.Find(c => c.OffsetCoord.Equals(CubeToOffsetCoords(CubeCoord + direction)));
                    var neighbour = cells.Find(c => ((Hexagon)c).CubeCoord.Equals(CubeCoord + direction));
                    if (neighbour == null) continue;
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;

        }//Each hex cell has six neighbors, which positions on grid relative to the cell are stored in _directions constant.

        public static Vector3 GetCellPosition(Cell cell)
        {
            return ((Hexagon)cell).CubeCoord;
        }

    }

    public enum HexGridType
    {
        even_q,
        odd_q,
        even_r,
        odd_r
    };
}