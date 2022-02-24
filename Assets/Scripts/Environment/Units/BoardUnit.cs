using System.Collections;
using System.Collections.Generic;
using Cells;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Battleground
{
    public class BoardUnit : Unit
    {
        public Color PlayerColor;
        public string UnitName;
       

        private float offset = 0;

        public override void Initialize()
        {
            base.Initialize();
           
            SetColor(PlayerColor);
            gameObject.transform.localPosition = Cell.transform.localPosition + new Vector3(0, 0, offset);
        }

        public override void Move(Cell destinationCell, List<Cell> path)
        {
      
            base.Move(destinationCell, path);
        }

        public override void MarkAsAttacking(Unit other)
        {
            
        }
        public override void MarkAsDefending(Unit other)
        {
            StartCoroutine(Glow(new Color(1, 0.5f, 0.5f), 1));
        }
        public override void MarkAsDestroyed()
        {
        }

        private IEnumerator Glow(Color color, float cooloutTime)
        {
            float startTime = Time.time;

            while (startTime + cooloutTime > Time.time)
            {
                SetColor(Color.Lerp(PlayerColor, color, (startTime + cooloutTime) - Time.time));
                yield return 0;
            }

            SetColor(PlayerColor);
        }

        public override void MarkAsFriendly()
        {
           
        }
        public override void MarkAsReachableEnemy()
        {
            
        }
        public override void MarkAsSelected()
        {
            
        }
        public override void MarkAsFinished()
        {
            SetColor(PlayerColor - Color.gray);
            
        }
        public override void UnMark()
        {
            SetColor(PlayerColor);
            
        }
        private void SetColor(Color color)
        {
            //transform.Find("Model").GetComponent<Renderer>().material.color = color;
        }
        protected override void OnMoveFinished()
        {
        }
    }
}