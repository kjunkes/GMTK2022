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

    // Panel Description Movement
    private Vector3 iconPosition;
    private Vector3 newPanelPosition;

    // Start is called before the first frame update
    void Start()
    {
        gameObjectName = gameObject.name;
        // Ability Description
        // Depends on button name of object!
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

        // Calculate Ability Panel Description
        iconPosition = gameObject.transform.position;

        newPanelPosition = abilityDescriptionPanel.transform.position;
        newPanelPosition.y = iconPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        abilityDescriptionPanel.SetActive(true);
        abilityName.text = currentAbilityName;
        abilityCost.text = currentAbilityCost;
        abilityDescriptionPanel.transform.position = newPanelPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abilityDescriptionPanel.SetActive(false);
    }
}
