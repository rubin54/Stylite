using Cells;
using Units;
using UnityEngine;

namespace Battleground
{
    class MyHexagon : Hexagon
    {
        [SerializeField]
        Renderer hexagonRenderer;

        [SerializeField]
        public int HealthShiftForStandingUnit;

        [SerializeField]
        private string headerContent;

        [SerializeField]
        private string bodyContent;

        public void Awake()
        {
            if(!hexagonRenderer)
            {
                hexagonRenderer = GetComponent<Renderer>();
            }
        }

        public override void MarkAsReachable()
        {
            //transform.localScale = new Vector3(1, 2, 1);
            hexagonRenderer.material.SetFloat("_Glow", 1.0f);
            hexagonRenderer.material.SetColor("_GlowColor", Color.green);
        }
        public override void MarkAsPath()
        {
            //transform.localScale = new Vector3(1, 3, 1);
            hexagonRenderer.material.SetFloat("_Glow", 1.0f);
            hexagonRenderer.material.SetColor("_GlowColor", Color.red);

        }
        // for edge cases
        public override void MarkAsHighlightedSpells()
        {
            hexagonRenderer.material.SetFloat("_Glow", 1.0f);
            hexagonRenderer.material.SetColor("_GlowColor", Color.yellow);
        }
        public override void MarkAsHighlighted()
        {
            hexagonRenderer.material.SetFloat("_Glow", 1.0f);
            hexagonRenderer.material.SetColor("_GlowColor", Color.yellow);
        }
        public override void UnMark()
        {
            hexagonRenderer.material.SetFloat("_Glow", 0.0f);

            //transform.localScale = new Vector3(1, 1, 1);
        }

        public override void Setup()
        {
            Vector2 calculatedValue = new Vector2();
            calculatedValue.x = Mathf.RoundToInt(transform.position.x / 1.5f);
            calculatedValue.y = Mathf.RoundToInt((transform.position.z / 1.73205f) - (Mathf.Abs(calculatedValue.x) % 2 == 1 ? 0.5f : 0f)) * -1 +14;

            HexGridType = Mathf.Abs(calculatedValue.x) % 2 == 1 ? HexGridType.odd_q : HexGridType.odd_q;

            OffsetCoord = calculatedValue;
        }

        public override void UnitStartsTurn(Unit unit)
        {
            if(HealthShiftForStandingUnit > 0)
            {
                unit.Heal(HealthShiftForStandingUnit);
            }
            else if(HealthShiftForStandingUnit < 0)
            {
                unit.ReceiveDamage(Mathf.Abs(HealthShiftForStandingUnit));
            }

            if(OnFire)
            {
                unit.ReceiveDamage(2);
            }
        }

        public override Content GetContent()
        {
            return new Content(headerContent, bodyContent);
        }
    }
}
