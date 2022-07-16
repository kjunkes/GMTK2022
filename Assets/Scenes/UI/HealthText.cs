using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    public Text m_MyText;
    public PlayerHealth m_Player;

    void Start()
    {

    }

    public void UpdateHealthText()
    {
        m_MyText.text = m_Player.health.ToString() + " / " + m_Player.maxHealth.ToString();
    }
}
