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

    // Start is called before the first frame update
    void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
        button = gameObject.GetComponent(typeof(Button)) as Button;
        buttonText = button.gameObject.transform.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
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
        }
        else if(this.gameLoop.GetPlayerActionState() == GameLoop.PlayerActionState.WALKING)
        {
            this.button.enabled = false;
            this.buttonText.text = "Skip Walking";
            this.gameLoop.IncrementTurnState();
        }

        //PURELY FOR DEBUGGING PURPOSES
        //REMOVE FROM FINAL GAME
        this.gameLoop.IncrementTurnState();
    }
}
