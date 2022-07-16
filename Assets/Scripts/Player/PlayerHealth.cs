using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : Health
{
    public HealthText healthText;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.UpdateHealthBar();
        healthText.UpdateHealthText();
    }
}
