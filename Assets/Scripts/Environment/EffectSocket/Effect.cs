using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public Action<Effect> OnFinishedAction;

    [SerializeField]
    public List<ParticleSystem> ParticleSystems;

    public bool startedPlaying = false;

    public void Setup(EffectSocket socket)
    {
        foreach (var ParticleSystem in ParticleSystems)
        {
            ParticleSystem.Play();
        }
        startedPlaying = true;
        OnSetup(socket);
    }

    public virtual void OnSetup(EffectSocket socket)
    {
        socket.OnAddedEffect(this);
    }

    public virtual void DestroyEffect(EffectSocket socket)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(startedPlaying)
        {

            foreach (var ParticleSystem in ParticleSystems)
            {
                if (ParticleSystem.isPlaying) return;
            }

            startedPlaying = false;
            OnFinishedAction?.Invoke(this);
        }
    }
}
