using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Invalid = -1,
    Money,
    Count
}

public class Resource : MonoBehaviour
{
    public Action<float, float> ChangedAmount; 

    [SerializeField]
    private ResourceType type;

    [SerializeField]
    private float amount;

    [SerializeField]
    private float minValue;

    public ResourceType Type
    {
        get => type;
    }

    public float Amount
    {
        get => amount;
    }

    public void Setup(ResourceType type)
    {
        this.type = type;
    }

    public void Add(float amount)
    {
        this.amount += amount;
        ChangedAmount?.Invoke(this.amount, amount);
    }

    public bool Subtract(float amount)
    {
        float amountPreChange = this.amount;

        this.amount -= amount;

        if(minValue > this.amount)
        {
            this.amount = amountPreChange;
            return false;
        }

        ChangedAmount?.Invoke(this.amount, amount * -1);

        return true;
    }

    public bool Contains(float amount)
    {
        return this.amount >= amount;
    }
}
