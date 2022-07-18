using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityDescriptionMouseOver : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public GameObject abilityDescriptionPanel;
    public TMP_Text abilityName;
    public TMP_Text abilityCost;

    private string gameObjectName;
    private string currentAbilityName;
    private string currentAbilityCost;

    // Start is called before the first frame update
    void Start()
    {
        gameObjectName = gameObject.name;

        switch (gameObjectName)
        {
            case "PunchButton":
                currentAbilityName = "Punch";
                currentAbilityCost = "1";
                break;
            case "RoundeHouseKickButton":
                currentAbilityName = "Roundhouse Kick";
                currentAbilityCost = "5";
                break;
            case "StoneThrowButton":
                currentAbilityName = "Throw Stone";
                currentAbilityCost = "2";
                break;
            case "ShotButton":
                currentAbilityName = "Shoot";
                currentAbilityCost = "6";
                break;
            case "SelfBuffButton":
                currentAbilityName = "Buff Yourself";
                currentAbilityCost = "3";
                break;
            case "EnemyDefenseNerfButton":
                currentAbilityName = "Debuff Enemy";
                currentAbilityCost = "1";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Test");
        abilityDescriptionPanel.SetActive(true);
        abilityName.text = currentAbilityName;
        abilityCost.text = currentAbilityCost;        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abilityDescriptionPanel.SetActive(false);
    }
}
