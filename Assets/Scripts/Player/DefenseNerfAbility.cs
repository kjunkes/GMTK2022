using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseNerfAbility : Ability
{
    public int range = 3;
    public float defenseNerf = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Use(EnemyHealth target, float attackMultiplier)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= range)
        {
            target.SetDefenseNerf(defenseNerf);
            return true;
        }

        return false;
    }
}
