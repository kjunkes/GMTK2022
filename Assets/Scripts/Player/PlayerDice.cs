using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerDice : MonoBehaviour
{
    public int normalDiceCount;
    public int highDiceCount;
    public int lowDiceCount;
    public int doubleDiceCount;

    public Color normalDiceColor;
    public Color highDiceColor;
    public Color lowDiceColor;
    public Color doubleDiceColor;

    public GameLoop gameLoop;
    public GameObject dices;
    public GameObject diceSelectionWindow;
    public PlayerEnergy playerEnergy;

    public class Dice
    {
        public int[] numbers;
        public Color color;
    }

    public Dice[] dice;

    // Start is called before the first frame update
    void Start()
    {
        int diceCount = normalDiceCount + highDiceCount
                        + lowDiceCount + doubleDiceCount;
        // Create Dice Array
        dice = new Dice[diceCount];

        // Create normal dice 1-6
        int[] normalDice = new int[] { 1, 2, 3, 4, 5, 6 };
        for (int i = 0; i < normalDiceCount; i++)
        {
            dice[i] = new Dice();
            dice[i].numbers = normalDice;
            dice[i].color = normalDiceColor;
        }

        // Create high dice 4-6
        int[] highDice = new int[] { 4, 5, 6 };
        int indexStart = normalDiceCount;
        int indexEnd = normalDiceCount + highDiceCount;
        for (int i = indexStart; i < indexEnd; i++)
        {
            dice[i] = new Dice();
            dice[i].numbers = highDice;
            dice[i].color = highDiceColor;
        }

        // Create low dice 1-3
        int[] lowDice = new int[] { 1, 2, 3 };
        indexStart = indexEnd;
        indexEnd = indexStart + lowDiceCount;
        for (int i = indexStart; i < indexEnd; i++)
        {
            dice[i] = new Dice();
            dice[i].numbers = lowDice;
            dice[i].color = lowDiceColor;
        }

        // Create double dice 2-12
        int[] doubleDice = new int[] { 2, 4, 6, 8, 10, 12 };
        indexStart = indexEnd;
        indexEnd = indexStart + doubleDiceCount;
        for(int i = indexStart; i < indexEnd; i++)
        {
            dice[i] = new Dice();
            dice[i].numbers = doubleDice;
            dice[i].color = doubleDiceColor;
        }

        foreach(Transform child in dices.transform)
        {
            DiceVisual dv = child.GetComponent(typeof(DiceVisual)) as DiceVisual;
            dv.InitDice();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRollingDice()
    {
        diceSelectionWindow.SetActive(true);
    }

    public void EndRollingDice(float energy)
    {
        diceSelectionWindow.SetActive(false);
        playerEnergy.InitialRoundEnergyUI(energy);
        gameLoop.IncrementTurnState();
    }
}
