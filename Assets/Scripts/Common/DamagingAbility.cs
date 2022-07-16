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

    public void Use(Health target)
    {
        float distanceRounded = Mathf.RoundToInt(Vector3.Distance(transform.position, target.transform.position));

        if(distanceRounded < range)
        {
            target.SetHealth(target.GetHealth() - damage);
        }
    }
}
