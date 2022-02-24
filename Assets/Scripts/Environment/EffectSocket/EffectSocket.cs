using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSocket : MonoBehaviour
{
    public Action StopTimeForEffect;
    public Action ContinueTimeForEffect;

    public static EffectSocket Instance;

    private List<Effect> effects = new List<Effect>();

    public void Start()
    {
        Instance = this;
    }

    public void CreateEffect(GameObject prefab, Vector3 position)
    {
        GameObject gameObject = Instantiate(prefab, transform);
        gameObject.transform.position = position;

        Effect effect = gameObject.GetComponent<Effect>();

        if(effect)
        {
            effect.Setup(this);
            effects.Add(effect);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnAddedEffect(Effect effect)
    {
        effect.OnFinishedAction += OnDestroyedEffect;
    }

    public void OnDestroyedEffect(Effect effect)
    {
        effects.Remove(effect);
        effect.DestroyEffect(this);
    }

    public void OnAddedEffectWithTimeStop(Effect effect)
    {
        effect.OnFinishedAction += OnDestroyedEffectWithTimeStop;

        StopTimeForEffect?.Invoke();
    }

    public void OnDestroyedEffectWithTimeStop(Effect effect)
    {
        effects.Remove(effect);
        effect.DestroyEffect(this);

        ContinueTimeForEffect?.Invoke();
    }
}
