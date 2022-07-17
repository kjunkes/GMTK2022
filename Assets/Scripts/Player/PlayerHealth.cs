using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : Health
{
    public HealthText healthText;
    public HealthBar healthBar;

    private AbilityManager abilityManager;

    // Start is called before the first frame update
    void Start()
    {
        abilityManager = FindObjectOfType<AbilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.UpdateHealthBar();
        healthText.UpdateHealthText();
    }

    public override void SetHealth(float value)
    {
        this.health = value;
    }

    public override void Buff(float value)
    {
        this.abilityManager.SetAttackMultiplier(value);
    }
}
