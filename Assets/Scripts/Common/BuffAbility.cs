using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAbility : Ability
{
    public float factor = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(Health target)
    {
        target.Buff(factor);
    }
}
