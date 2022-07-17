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
    public SpriteRenderer attackRangeIndicator;

    public Ability punch;
    public Ability roundhouseKick;
    public Ability stoneThrow;
    public Ability shot;
    public Ability selfBuff;
    public Ability enemyDefenseNerf;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.UpdateEnergyBar();
        energyText.UpdateEnergyText();
    }

    // Update is called once per frame
    void Update()
    {
        energyBar.UpdateEnergyBar();
        energyText.UpdateEnergyText();
    }

    public void InitialRoundEnergyUI(float roundEnergy)
    {
        energy = roundEnergy;
        maxEnergy = roundEnergy;
        energyBar.UpdateEnergyBar();
        energyText.UpdateEnergyText();
    }

    public Ability GetAbilityOfType(AbilityManager.AbilityType type)
    {
        switch (type)
        {
            case AbilityManager.AbilityType.PUNCH:
                return punch;
            case AbilityManager.AbilityType.ROUNDHOUSE_KICK:
                return roundhouseKick;
            case AbilityManager.AbilityType.STONE_THROW:
                return stoneThrow;
            case AbilityManager.AbilityType.SHOT:
                return shot;
            case AbilityManager.AbilityType.SELF_BUFF:
                return selfBuff;
            case AbilityManager.AbilityType.ENEMY_DEFENSE_NERF:
                return enemyDefenseNerf;
            default:
                return null;
        }
    }
}
