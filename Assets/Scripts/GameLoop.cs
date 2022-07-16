using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameLoop : MonoBehaviour
{
    public enum Turn {
        PLAYER,
        ENEMY
    }

    public enum PlayerActionState
    {
        DICE_SELECTION,
        WALKING,
        ACTION
    }

    public GameObject enemies;

    public string nextLevel;

    private PlayerDice playerDice;
    private PlayerHealth playerHealth;
    private EndTurnButton endTurnButton;

    //List of all those enemies that are yet to act in the current turn
    private List<ActionToken> idleEnemies = new List<ActionToken>();

    //Game State
    private Turn currentTurn = Turn.PLAYER;
    private PlayerActionState playerActionState = PlayerActionState.DICE_SELECTION;

    // Start is called before the first frame update
    void Start()
    {
        playerDice = FindObjectOfType<PlayerDice>();
        endTurnButton = FindObjectOfType<EndTurnButton>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementTurnState()
    {
        switch (this.currentTurn)
        {
            case Turn.PLAYER:
                switch (this.playerActionState)
                {
                    case PlayerActionState.DICE_SELECTION:
                        this.playerActionState = PlayerActionState.WALKING;
                        this.endTurnButton.enabled = true;
                        return;
                    case PlayerActionState.WALKING:
                        this.playerActionState = PlayerActionState.ACTION;
                        return;
                    case PlayerActionState.ACTION:
                        this.currentTurn = Turn.ENEMY;

                        foreach (Transform child in enemies.transform)
                        {
                            this.idleEnemies.Add(child.GetComponent(typeof(ActionToken)) as ActionToken);
                        }

                        if (this.idleEnemies.Count > 0)
                        {
                            this.idleEnemies[0].StartAction();
                        }
                        return;
                    default:
                        break;
                }
                break;
            case Turn.ENEMY:
                this.currentTurn = Turn.PLAYER;
                this.playerActionState = PlayerActionState.DICE_SELECTION;

                this.playerDice.StartRollingDice();
                return;
            default:
                break;
        }
    }

    public bool CanWalk()
    {
        return this.currentTurn == Turn.PLAYER && this.playerActionState == PlayerActionState.WALKING;
    }

    public void PassOnToken()
    {
        //should always be true
        if (this.idleEnemies.Count > 0)
        {
            this.idleEnemies.RemoveAt(0);
        }

        CheckForPlayerDeath();

        //if enemies are yet to act, give them the token
        if(this.idleEnemies.Count > 0)
        {
            this.idleEnemies[0].StartAction();
        }
        else
        {
            IncrementTurnState();
        }
    }

    public void CheckForPlayerDeath()
    {
        if (playerHealth.GetHealth() <= 0)
        {
            //RESET
        }
    }

    public void EndLevel()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public Turn GetCurrentTurn()
    {
        return this.currentTurn;
    }

    public void SetCurrentTurn(Turn value)
    {
        this.currentTurn = value;
    }

    public PlayerActionState GetPlayerActionState()
    {
        return this.playerActionState;
    }

    public void SetPlayerActionState(PlayerActionState value)
    {
        this.playerActionState = value;
    }
}
