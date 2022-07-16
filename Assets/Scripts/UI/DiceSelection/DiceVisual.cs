using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceVisual : MonoBehaviour
{
    public TextMeshProUGUI diceText;
    public PlayerDice playerDice;
    
    public int diceId;
    public float diceNumberWaitTime;

    // Next update in second
    private float nextUpdate = 0.1f;
    private int index = 0;
    private int[] currentNumbers;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitDice()
    {
        currentNumbers = playerDice.dice[diceId].numbers;
        diceText.text = currentNumbers[index].ToString();
        Debug.Log(currentNumbers[1].ToString());
    }
    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second + x)
            nextUpdate = Time.time + diceNumberWaitTime;
            // Call your fonction
            UpdateEverySecond();
        }

    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        if(index >= currentNumbers.Length)
        {
            index = 0;
            diceText.text = currentNumbers[index].ToString();
            index++;
        }
        else
        {
            diceText.text = currentNumbers[index].ToString();
            index++;
        }
    }
}
