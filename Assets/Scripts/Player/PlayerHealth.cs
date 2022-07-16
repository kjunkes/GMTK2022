using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : Health
{
    public HealthBar healthBar;
    public HealthText healthText;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.UpdateHealthBar();
        healthText.UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
