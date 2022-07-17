using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAbility : Ability
{
    public int range = 1;
    public int damage = 5;

    private EnemyHealth[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(float attackMultiplier)
    {
        foreach(EnemyHealth enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if(distance <= range)
            {
                enemy.SetHealth(enemy.GetHealth() - damage * attackMultiplier * enemy.GetDefenseNerf());
            }
        }
    }
}
