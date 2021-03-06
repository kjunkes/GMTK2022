using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingAbility : Ability
{
    public float damage = 5f;
    public int range = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Use(Health target, float attackMultiplier)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if(distance <= range)
        {
            target.SetHealth(target.GetHealth() - damage * attackMultiplier);
            return true;
        }

        return false;
    }
}
