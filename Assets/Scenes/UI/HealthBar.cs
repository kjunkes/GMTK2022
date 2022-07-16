using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image m_healthBar;
    public PlayerHealth playerHealth;

    void Start()
    {
        m_healthBar.fillAmount = Mathf.Clamp(0.5f, 0, 1);
    }
    public void UpdateHealthBar()
    {
        m_healthBar.fillAmount = Mathf.Clamp(playerHealth.health / playerHealth.maxHealth, 0, 1f);
    }
}