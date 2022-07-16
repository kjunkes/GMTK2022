using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image energyBar;
    public PlayerEnergy playerEnergy;

    void Start()
    {
        energyBar.fillAmount = Mathf.Clamp(0.5f, 0, 1);
    }
    public void UpdateEnergyBar()
    {
        energyBar.fillAmount = Mathf.Clamp(playerEnergy.energy / playerEnergy.maxEnergy, 0, 1f);
    }
}