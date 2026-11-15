using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class Casino : MonoBehaviour
{
    public int moneyAmount = 50;
    public int betAmount = 5;

    public int lootBoxChance = 50;


    [SerializeField] Sprite[] cards;

    [SerializeField] GameObject guessCard;
    private Sprite guessCardSprite;

    [SerializeField] GameObject refCard;
    private Sprite refCardSprite;

    private int cardGenerate;

    private bool canBet = true;

    [SerializeField]private int guessCardValue;
    [SerializeField]private int refCardValue;

    

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI betText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Generate the first ref card
        cardGenerate = Random.Range(0,cards.Length);
        Debug.Log(cardGenerate);

        refCardSprite = cards[cardGenerate];
        Debug.Log(refCardSprite);

        refCard.GetComponent<Image>().sprite = refCardSprite;
        guessCard.GetComponent<Image>().sprite = null;

        string spriteName = refCardSprite.name;
        string[] parts = spriteName.Split('_');

        refCardValue = int.Parse(parts[1]);

        //Debug.Log(refCardValue);

        // set money and bet
        moneyText.SetText(moneyAmount.ToString());
        betText.SetText(betAmount.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            BetLess();
            UpdateMoney();
        }

        if (Input.GetKeyDown("d"))
        {
            BetMore();
            UpdateMoney();
        }

        if (Input.GetKeyDown("w"))
        {
            BetHigh();
        }

        if (Input.GetKeyDown("s"))
        {
            BetLow();
        }
    }

    void BetMore()
    {
        Debug.Log("Bet More");
        moneyAmount-=5;
        betAmount+=5;
    }
    void BetLess()
    {
        Debug.Log("Bet Less");
        moneyAmount+=5;
        betAmount-=5;
    }

    void BetHigh()
    {
        if (canBet == true)
        {
            FlipCard();

            if(guessCardValue > refCardValue)
            {
                Debug.Log("You win");
            }

            else if(guessCardValue == refCardValue)
            {
                Debug.Log("Draw");
            }

            else
            {
                Debug.Log("You loose");
            }
        }
    }

    void BetLow()
    {
        if (canBet == true)
        {
            FlipCard();

            if(guessCardValue > refCardValue)
            {
                Debug.Log("You win");
            }

            else if(guessCardValue == refCardValue)
            {
                Debug.Log("Draw");
            }

            else
            {
                Debug.Log("You loose");
            }
        }
    }



    void FlipCard()
    {
        cardGenerate = Random.Range(0,cards.Length);
        guessCardSprite = cards[cardGenerate];

        guessCard.GetComponent<Image>().sprite = guessCardSprite;

        string spriteName = guessCardSprite.name;
        string[] parts = spriteName.Split('_');

        guessCardValue = int.Parse(parts[1]);

        StartCoroutine(ResetCardsCoroutine(2));
    }


    IEnumerator ResetCardsCoroutine(float duration)
    {
        canBet = false;

        yield return new WaitForSeconds(duration);

        guessCard.GetComponent<Image>().sprite = null;
        refCard.GetComponent<Image>().sprite = guessCardSprite;

        string spriteName = guessCardSprite.name;
        string[] parts = spriteName.Split('_');
        refCardValue = int.Parse(parts[1]);


        canBet = true;
        Debug.Log("changes done");
    }


    void UpdateMoney()
    {
        moneyText.SetText(moneyAmount.ToString());
        betText.SetText(betAmount.ToString());
    }
}


