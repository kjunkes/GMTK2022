using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnButton : MonoBehaviour
{
    private const float PRESSING_CD = 0.5f;

    private GameLoop gameLoop;
    private Button button;
    private TextMeshProUGUI buttonText;

    private float lastPressed = 0f;

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
        if(Time.time - lastPressed < PRESSING_CD)
        {
            return;
        }

        if(this.gameLoop.GetPlayerActionState() == GameLoop.PlayerActionState.WALKING)
        {
            this.buttonText.text = "End Turn";
            this.gameLoop.IncrementTurnState();
            return;
        }
        else if(this.gameLoop.GetPlayerActionState() == GameLoop.PlayerActionState.WALKING)
        {
            this.button.enabled = false;
            this.buttonText.text = "Skip Walking";
            this.gameLoop.IncrementTurnState();
            return;
        }

        lastPressed = Time.time;

        //PURELY FOR DEBUGGING PURPOSES
        //REMOVE FROM FINAL GAME
        this.gameLoop.IncrementTurnState();
    }
}
