using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Resource resource;

    [SerializeField]
    private List<GameObject> spellOfferPrefabs;

    [SerializeField]
    private List<GameObject> unitOfferPrefabs;

    [SerializeField]
    private Transform spellOfferSocket;

    [SerializeField]
    private Transform unitOfferSocket;

    [SerializeField]
    private int spellOfferCount = 5;

    [SerializeField]
    private int unitOfferCount = 3;

    [SerializeField]
    private Inventory inventory;

    private List<ShopOffer> currentUnitOffers = new List<ShopOffer>();

    private List<ShopOffer> currentSpellOffers = new List<ShopOffer>();

    private void Start()
    {
        inventory.UnitInventory.ReachedLimit += OnUnitInventoryReachedLimit;
        inventory.UnitInventory.Reopened += OnUnitInventoryReopened;

        GenerateOffers(currentSpellOffers, spellOfferPrefabs, spellOfferSocket, spellOfferCount);
        GenerateOffers(currentUnitOffers, unitOfferPrefabs, unitOfferSocket, unitOfferCount);
    }

    private void GenerateOffers(List<ShopOffer> container, List<GameObject> prefabs, Transform socket, int amount)
    {
        foreach (ShopOffer offer in container.ToArray())
        {
            offer.Destroy();

            container.Clear();
        }

        for (int i = 0; i < amount; i++)
        {
            generateOffer(prefabs, socket, container);
        }
    }

    public void RegenerateEveryOffer()
    {
        GenerateOffers(currentSpellOffers, spellOfferPrefabs, spellOfferSocket, spellOfferCount);
        GenerateOffers(currentUnitOffers, unitOfferPrefabs, unitOfferSocket, unitOfferCount);

        CheckForUnitInventoryCapacity();
    }

    public void RegenerateOffers()
    {
        GenerateOffers(currentSpellOffers, spellOfferPrefabs, spellOfferSocket, spellOfferCount);
    }

    private void generateOffer(List<GameObject> offerPrefabs, Transform offerSocket, List<ShopOffer> category)
    {
        GameObject chosenPrefab = offerPrefabs[Random.Range(0, offerPrefabs.Count)];

        GameObject createdObject = Instantiate(chosenPrefab, offerSocket);

        ShopOffer shopOffer = createdObject.gameObject.GetComponent<ShopOffer>();

        if(shopOffer)
        {
            shopOffer.SuccessfullPurchase += OnSuccessfullPurchase;
            shopOffer.Setup(resource);

            category.Add(shopOffer);
        }
        else
        {
            Debug.LogError("Object " + createdObject.name + " doesn't contain a ShopOffer Script");
        }
    }

    public void OnSuccessfullPurchase(ShopOffer offer)
    {
        if(currentSpellOffers.Contains(offer) || currentUnitOffers.Contains(offer))
        {
            GameObject createdContent = Instantiate(offer.Content);

            switch (offer.Type)
            {
                case ContentType.Spell:
                    inventory.AddSpell(createdContent);
                    break;
                case ContentType.Unit:
                    inventory.AddUnit(createdContent);
                    break;
                default:
                    Destroy(createdContent);
                    break;
            }

            if(currentSpellOffers.Contains(offer)) currentSpellOffers.Remove(offer);
            if(currentUnitOffers.Contains(offer)) currentUnitOffers.Remove(offer);
            offer.Destroy();
        }
    }

    public void CheckForUnitInventoryCapacity()
    {
        if(inventory.UnitInventory.IsFull())
        {
            OnUnitInventoryReachedLimit();
        }
    }

    public void OnUnitInventoryReachedLimit()
    {
        foreach (var currentUnitOffer in currentUnitOffers)
        {
            currentUnitOffer.Deactivate();
        }
    }

    public void OnUnitInventoryReopened()
    {
        foreach (var currentUnitOffer in currentUnitOffers)
        {
            currentUnitOffer.Activate();
        }
    }
}
