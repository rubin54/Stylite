using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOfferContentGrabbableImageObserver : MonoBehaviour
{
    [SerializeField]
    public ShopOffer shopOffer;

    [SerializeField]
    public RawImage rawImage;

    private void Start()
    {
        if(shopOffer)
        {
            RawImage rawImage = shopOffer.Content.GetComponent<RawImage>();
            if(rawImage)
            {
                this.rawImage.texture = rawImage.texture;
            }
        }
    }

}
