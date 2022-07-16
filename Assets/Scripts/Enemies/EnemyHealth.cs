using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    private ActionToken actionToken;
    private Transform healthbar;
    private Vector3 relativePosition;

    // Start is called before the first frame update
    void Start()
    {
        actionToken = this.GetComponent<ActionToken>();
        healthbar = transform.Find("EnemyHealthBar");
        relativePosition = healthbar.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float healthQuotient = health / maxHealth;
        Vector3 currentHealthbarScale = healthbar.transform.localScale;
        healthbar.transform.localScale = new Vector3(healthQuotient, currentHealthbarScale.y, currentHealthbarScale.z);
        float horizontalOffset = (1 - healthQuotient) * 0.5f;
        healthbar.transform.position = transform.position + relativePosition + new Vector3(-horizontalOffset, 0, 0);
    }

    public new void SetHealth(float value)
    {
        health = Mathf.Max(value, 0);
        
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
