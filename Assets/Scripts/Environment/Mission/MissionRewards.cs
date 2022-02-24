using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionRewards : MonoBehaviour
{
    [SerializeField]
    public Resource Gold;

    [SerializeField]
    public int modifierCount = 0;

    [SerializeField]
    public List<ModifierTemplate> Modifiers;

    private List<ModifierTemplate> chosenModifiers;

    public List<ModifierTemplate> ChosenModifiers
    {
        get
        {
            if(chosenModifiers == null)
            {
                chosenModifiers = new List<ModifierTemplate>();

                for (int i = 0; i < modifierCount; i++)
                {
                    chosenModifiers.Add(GetRandomModifier());
                }
            }

            return chosenModifiers;
        }
    }

    public void ReceiveRewards()
    {
        if(ShopTab.Instance)
        {
            ShopTab.Instance.OnFinishedMission();
            ShopTab.Instance.AddResource(Gold);
            foreach (var modifier in ChosenModifiers)
            {
                ShopTab.Instance.AddModifier(modifier);
            }
        }
        else
        {
            ShopTab.RewardBuffer = new RewardBuffer();
            DontDestroyOnLoad(Gold);
            ShopTab.RewardBuffer.Gold = Gold;

            foreach (var modifier in ChosenModifiers)
            {
                DontDestroyOnLoad(modifier.gameObject);
                ShopTab.RewardBuffer.Modifiers = ChosenModifiers;
            }
        }
    }

    public ModifierTemplate GetRandomModifier() => Instantiate(Modifiers[Mathf.RoundToInt(Random.Range(0, Modifiers.Count - 1))]);
}
