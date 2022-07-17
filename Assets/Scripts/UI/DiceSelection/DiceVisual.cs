using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DiceVisual : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI diceText;
    public PlayerDice playerDice;
    
    public int diceId;
    public float diceNumberWaitTime;
    public float selectedDiceTransparency = 0.4f;

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
        if(diceId < playerDice.dice.Length)
        {
            gameObject.SetActive(true);
            currentNumbers = playerDice.dice[diceId].numbers;
            diceText.text = currentNumbers[index].ToString();
            gameObject.GetComponent<Image>().color = playerDice.dice[diceId].color;
        } else
        {
            gameObject.SetActive(false);
            diceText.text = "";
        }
        
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(currentNumbers.Length != 1)
        {
            int finalNumber = currentNumbers[Random.Range(0, currentNumbers.Length)];
            playerDice.dice[diceId].numbers = new int[] { finalNumber };
            currentNumbers = new int[] { finalNumber };

            diceText.text = finalNumber.ToString();

            // Change alpha channel of object
            Color gameObjectColor = gameObject.GetComponent<Image>().color;
            Debug.Log(selectedDiceTransparency);
            gameObjectColor.a = selectedDiceTransparency;
            gameObject.GetComponent<Image>().color = gameObjectColor;

            playerDice.EndRollingDice(finalNumber);
        }
    }
}

