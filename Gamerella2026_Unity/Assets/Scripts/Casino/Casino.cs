using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Casino : MonoBehaviour
{
    public int moneyAmount = 75;
    public int betAmount = 6;

    public float lootBoxChance = 10f;
    [SerializeField] float winProbability;

    [SerializeField] float MaxLootBoxChance = 100f;
    [SerializeField] float MinLootBoxChance = 0f;
    [SerializeField] float MaxBet = 1000f;

    [SerializeField] float MaxChanceIncrease = 15f;
    [SerializeField] float MaxChanceDecrease = 10f;
    [SerializeField] float RiskPower = 2f;


    [SerializeField] Sprite[] cards;

    [SerializeField] GameObject guessCard;
    private Sprite guessCardSprite;

    [SerializeField] GameObject refCard;
    private Sprite refCardSprite;

    [SerializeField] Sprite backCardSprite;

    private int cardGenerate;

    private bool canBet = true;

    [SerializeField]private int guessCardValue;
    [SerializeField]private int refCardValue;
    
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI betText;

    [SerializeField] private TextMeshProUGUI lootText;

    public enum BetDirection
    {
        Higher,
        Lower
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Generate the first ref card
        cardGenerate = Random.Range(0,cards.Length);
        refCardSprite = cards[cardGenerate];

        refCard.GetComponent<Image>().sprite = refCardSprite;
        guessCard.GetComponent<Image>().sprite = backCardSprite;

        string spriteName = refCardSprite.name;
        string[] parts = spriteName.Split('_');

        refCardValue = int.Parse(parts[1]);

        // set money and bet
        moneyText.SetText(moneyAmount.ToString());
        betText.SetText(betAmount.ToString());
        lootText.SetText(lootBoxChance.ToString());

        UpdateMoney();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            BetLess();
        }

        if (Input.GetKeyDown("d"))
        {
            BetMore();
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
        if (moneyAmount >= 5)
        {
            moneyAmount-=5;
            betAmount+=5;
        }

        else if (moneyAmount < 5)
        {
            betAmount+=moneyAmount;
            moneyAmount-=moneyAmount;
        }
        UpdateMoney();
    }

    void BetLess()
    {
        if (betAmount > 5)
        {
            moneyAmount+=5;
            betAmount-=5;
        }

        else if (betAmount < 4 && betAmount > 0)
        {
            moneyAmount+=betAmount-1;
            betAmount-=betAmount-1;
        }
        
        UpdateMoney();
    }

    void BetHigh()
    {
        if (canBet == true)
        {
            FlipCard();

            if(guessCardValue > refCardValue)
            {
                float winProbability = CalculateWinProbability(refCardValue,BetDirection.Higher);
                WinBet();
            }

            else if(guessCardValue == refCardValue)
            {
                Draw();
            }

            else
            {
                LooseBet();
            }
        }
    }

    void BetLow()
    {
        if (canBet == true)
        {
            FlipCard();

            if(guessCardValue < refCardValue)
            {
                float winProbability = CalculateWinProbability(refCardValue,BetDirection.Lower);
                WinBet();
            }

            else if(guessCardValue == refCardValue)
            {
                Draw();
            }

            else
            {
                LooseBet();
            }
        }
    }

    public float CalculateWinProbability(int currentCard, BetDirection direction)
    {
        int totalCards = cards.Length;
        int remainingCards = totalCards - 1;

        int winningCards = direction == BetDirection.Higher
            ? (13 - currentCard) * 4
            : (currentCard - 1) * 4;

        return (float)winningCards / remainingCards;
    }

    public void UpdateLootBoxChance(float winProbability, float betAmount)
    {
        // Convert percentage to a value between 0 and 1
        float probability = Mathf.Clamp01(winProbability);
    
        // Calculate risk
        float risk = 1f - probability;
    
        // Apply nonlinear risk scaling
        float riskReward = Mathf.Pow(risk, RiskPower);
    
        // Normalize bet amount
        float betFactor = Mathf.Clamp01(betAmount / MaxBet);
    
        // Calculate LootBoxChance increase
        float chanceIncrease = MaxChanceIncrease * riskReward * betFactor;
        //float chanceDecrease = MaxChanceDecrease * riskReward * betFactor;
    
        // Update and clamp LootBoxChance
        lootBoxChance = Mathf.Clamp(
            lootBoxChance + chanceIncrease,
            0f,
            MaxLootBoxChance
        );
    }

    void WinBet()
    {
        Debug.Log("You win");

        float probability = Mathf.Clamp01(winProbability);
        float risk = 1f - probability;
        float riskReward = Mathf.Pow(risk, RiskPower);
        float betFactor = Mathf.Clamp01(betAmount / MaxBet);
    
        // Calculate LootBoxChance increase
        float chanceIncrease = MaxChanceIncrease * riskReward * betFactor;
        //float chanceDecrease = MaxChanceDecrease * riskReward * betFactor;
    
        // Update and clamp LootBoxChance
        lootBoxChance = Mathf.Clamp(
            lootBoxChance + chanceIncrease,
            MinLootBoxChance,
            MaxLootBoxChance
        );

        //lootBoxChance += betAmount;
        lootText.SetText(lootBoxChance.ToString());
    }

    void Draw()
    {
        Debug.Log("Draw");
        betAmount = 0;
        BetMore();
    }

    void LooseBet()
    {
        Debug.Log("You loose");

        float probability = Mathf.Clamp01(winProbability);
        float risk = 1f - probability;
        float riskReward = Mathf.Pow(risk, RiskPower);
        float betFactor = Mathf.Clamp01(betAmount / MaxBet);
    
        // Calculate LootBoxChance increase
        //float chanceIncrease = MaxChanceIncrease * riskReward * betFactor;
        float chanceDecrease = MaxChanceDecrease * riskReward * betFactor;
    
        // Update and clamp LootBoxChance
        lootBoxChance = Mathf.Clamp(
            lootBoxChance + chanceDecrease,
            MinLootBoxChance,
            MaxLootBoxChance
        );
        //lootBoxChance -= betAmount;
        lootText.SetText(lootBoxChance.ToString());

        betAmount = 0;
        BetMore();
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

        guessCard.GetComponent<Image>().sprite = backCardSprite;
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
