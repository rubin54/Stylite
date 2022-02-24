using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBuffer
{
    public List<ModifierTemplate> Modifiers;
    public Resource Gold;
}

public class ShopTab : MonoBehaviour
{
    public static ShopTab Instance;

    public static RewardBuffer RewardBuffer;

    [SerializeField]
    private List<GameObject> essentialObjects;

    [SerializeField]
    public Resource Gold;

    [SerializeField]
    public Inventory Inventory;

    [SerializeField]
    public Shop Shop;

    [SerializeField]
    private SceneLoader sceneLoader;

    private void Start()
    {
        if(RewardBuffer != null)
        {
            AddModifiers(RewardBuffer.Modifiers);
            AddResource(RewardBuffer.Gold);
        }
        Instance = this;
    }

    public void ActivateScene()
    {
        foreach (var essentialObject in essentialObjects)
        {
            essentialObject.SetActive(true);
        }
    }

    public void DeactivateScene()
    {
        foreach (var essentialObject in essentialObjects)
        {
            essentialObject.SetActive(false);
        }
    }

    public void OnFinishedMission()
    {
        Shop.RegenerateEveryOffer();
        sceneLoader.levelIndex++;
    }

    public void AddResource(Resource resource)
    {
        Gold.Add(resource.Amount);
    }

    public void AddModifiers(List<ModifierTemplate> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            AddModifier(modifier);
        }
    }

    public void AddModifier(ModifierTemplate modifier)
    {
        Debug.Log(Inventory);
        Debug.Log(modifier);
        Inventory.AddModifier(modifier.gameObject);
    }
}
