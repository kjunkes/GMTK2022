using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
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
