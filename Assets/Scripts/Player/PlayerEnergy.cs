using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public float energy;
    public float maxEnergy;
    public EnergyBar energyBar;
    public EnergyText energyText;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.UpdateEnergyBar();
        energyText.UpdateEnergyText();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
