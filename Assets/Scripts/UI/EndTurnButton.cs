using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnButton : MonoBehaviour
{
    private GameLoop gameLoop;
    private Button button;
    private TextMeshProUGUI buttonText;
    private AbilityManager abilityManager;

    // Start is called before the first frame update
    void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
        abilityManager = FindObjectOfType<AbilityManager>();
        button = gameObject.GetComponent<Button>();
        buttonText = button.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(EndTurn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndTurn()
    {
        if(this.gameLoop.GetPlayerActionState() == GameLoop.PlayerActionState.WALKING)
        {
            this.buttonText.text = "End Turn";
            this.gameLoop.IncrementTurnState();
            return;
        }
        else if(this.gameLoop.GetPlayerActionState() == GameLoop.PlayerActionState.ACTION)
        {
            this.abilityManager.EndAbilityPhase();
            return;
        }
    }

    public void SetButtonText(string value)
    {
        this.buttonText.text = value;
    }
}
