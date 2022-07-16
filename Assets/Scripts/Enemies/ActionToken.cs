using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionToken : MonoBehaviour
{
    private GameLoop gameLoop;

    private bool hasActionToken = false;
    private bool hasActedThisTurn = false;
    private bool hasMovedThisTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetHasToken()
    {
        return this.hasActionToken;
    }

    public bool GetHasActedThisTurn()
    {
        return this.hasActedThisTurn;
    }

    public void SetHasActedThisTurn(bool value)
    {
        this.hasActedThisTurn = value;
    }

    public bool GetHasMovedThisTurn()
    {
        return this.hasMovedThisTurn;
    }

    public void SetHasMovedThisTurn(bool value)
    {
        this.hasMovedThisTurn = value;
    }

    public void StartAction()
    {
        this.hasActionToken = true;
    }

    public void EndAction()
    {
        this.hasActedThisTurn = true;
        this.hasMovedThisTurn = true;
        this.hasActionToken = false;
        this.gameLoop.PassOnToken();
    }

    public void Reset()
    {
        this.hasActedThisTurn = false;
        this.hasMovedThisTurn = false;
        this.hasActionToken = false;
    }
}
