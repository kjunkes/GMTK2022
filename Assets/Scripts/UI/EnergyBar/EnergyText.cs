using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyText : MonoBehaviour
{
    public Text energyText;
    public PlayerEnergy playerEnergy;

    void Start()
    {

    }

    public void UpdateEnergyText()
    {
        energyText.text = playerEnergy.energy.ToString() + " / " + playerEnergy.maxEnergy.ToString();
    }
}
