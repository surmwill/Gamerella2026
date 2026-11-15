using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Casino : MonoBehaviour
{
    public enum BetDirection { Higher, Lower }
    private enum RoundResult { Win, Lose, Draw }

    [Header("Money")]
    public int moneyAmount
    {
        get => Game.Instance.Player.NumTokens;
        set => Game.Instance.Player.NumTokens = value;
    }
    
    [Tooltip("Starting stake. It is taken out of moneyAmount when a game starts.")]
    public int betAmount = 5;
    [SerializeField] int betStep = 5;
    [SerializeField] int minBet = 1;

    [SerializeField]
    private TMP_Text _resultsText;

    [Header("Loot box")]
    public float lootBoxChance = 10f;
    private float MinLootBoxChance = 0f;
    private float MaxLootBoxChance = 100f;
    [SerializeField] int MaxBet = 100;
    [SerializeField] float MaxChanceIncrease = 15f;
    [SerializeField] float MaxChanceDecrease = 10f;
    [SerializeField] float RiskPower = 2f;
    [Tooltip("Minimum share of MaxChanceIncrease a win always gives, even on the safest bet (0 to 1).")]
    [Range(0f, 1f)] [SerializeField] float MinWinReward = 0.2f;

    [Header("Cards")]
    [SerializeField] Sprite[] cards;
    [SerializeField] Sprite backCardSprite;
    [SerializeField] GameObject guessCard;
    [SerializeField] GameObject refCard;
    [SerializeField] float revealDuration = 2f;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI betText;
    [SerializeField] TextMeshProUGUI lootText;
    [Tooltip("Optional. Shown when the game is over.")]
    [SerializeField] GameObject gameOverPanel;

    [Header("Debug (read only)")]
    [SerializeField] float winProbability;
    [SerializeField] int refCardValue;
    [SerializeField] int guessCardValue;

    private Image guessImage;
    private Image refImage;
    private int[] cardValues;      // parsed once from sprite names, same order as cards[]
    private int refCardIndex;
    private bool canBet = true;
    private bool isGameOver;

    // Values from the Inspector, remembered so a new game can restore them
    private int startingMoney;
    private int startingBet;
    private float startingLootBoxChance;

    void Awake()
    {
        guessImage = guessCard.GetComponent<Image>();
        refImage = refCard.GetComponent<Image>();

        startingMoney = moneyAmount;
        startingBet = betAmount;
        startingLootBoxChance = lootBoxChance;

        // Parse "Something_<value>" sprite names once
        cardValues = new int[cards.Length];
        for (int i = 0; i < cards.Length; i++)
            cardValues[i] = ParseCardValue(cards[i]);
    }

    void Start()
    {
        NewGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) BetLess();
        if (Input.GetKeyDown(KeyCode.D)) BetMore();
        if (Input.GetKeyDown(KeyCode.W)) PlayRound(BetDirection.Higher);
        if (Input.GetKeyDown(KeyCode.S)) PlayRound(BetDirection.Lower);

        if (Input.GetKeyDown(KeyCode.E)) ResetLootBoxChance();   // placeholder for a real event
        if (Input.GetKeyDown(KeyCode.R) && isGameOver) NewGame();
        
        UpdateUI();
    }

    // ------------------------------------------------------------- game flow

    public void NewGame()
    {
        StopAllCoroutines();
        
        betAmount = Mathf.Min(startingBet, moneyAmount, MaxBet);
        moneyAmount -= betAmount;

        lootBoxChance = startingLootBoxChance;

        isGameOver = false;
        canBet = true;

        // First reference card
        refCardIndex = Random.Range(0, cards.Length);
        refCardValue = cardValues[refCardIndex];
        refImage.sprite = cards[refCardIndex];
        guessImage.sprite = backCardSprite;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        UpdateUI();
    }

    void GameOver()
    {
        isGameOver = true;
        canBet = false;
        Debug.Log("Game over - press R for a new game");

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Call this from any event (for now: the E key)
    public void ResetLootBoxChance()
    {
        lootBoxChance = startingLootBoxChance;
        UpdateUI();
    }

    // ---------------------------------------------------------------- betting

    void BetMore()
    {
        if (!canBet) return;

        // Can't add more than the player has, or more than MaxBet allows
        int step = Mathf.Min(betStep, moneyAmount, MaxBet - betAmount);
        if (step <= 0) return;

        moneyAmount -= step;
        betAmount += step;
        UpdateUI();
    }

    void BetLess()
    {
        if (!canBet) return;

        // Never go below the minimum bet
        int step = Mathf.Min(betStep, betAmount - minBet);
        if (step <= 0) return;

        betAmount -= step;
        moneyAmount += step;
        UpdateUI();
    }

    void PlayRound(BetDirection direction)
    {
        if (!canBet || isGameOver || betAmount <= 0) return;
        canBet = false;

        winProbability = CalculateWinProbability(refCardValue, direction);

        // Draw the new card (never the exact same card as the reference) and show it right away
        int guessIndex = DrawGuessIndex();
        guessCardValue = cardValues[guessIndex];
        guessImage.sprite = cards[guessIndex];

        RoundResult result;
        if (guessCardValue == refCardValue) result = RoundResult.Draw;
        else if ((guessCardValue > refCardValue) == (direction == BetDirection.Higher)) result = RoundResult.Win;
        else result = RoundResult.Lose;

        StartCoroutine(ResolveRound(result, guessIndex));
    }

    int DrawGuessIndex()
    {
        if (cards.Length < 2) return 0;

        int index;
        do { index = Random.Range(0, cards.Length); }
        while (index == refCardIndex);
        return index;
    }

    IEnumerator ResolveRound(RoundResult result, int guessIndex)
    {
        _resultsText.gameObject.SetActive(true);
        _resultsText.text = $"{result}!\nPlease wait for the next round";

        switch (result)
        {
            case RoundResult.Win:
                _resultsText.color = Color.green;
                break;
            
            case RoundResult.Draw:
                _resultsText.color = Color.yellow;
                break;
            
            case RoundResult.Lose:
                _resultsText.color = Color.red;
                break;
        }
        
        yield return new WaitForSeconds(revealDuration);
        _resultsText.gameObject.SetActive(false);

        int stake = betAmount;
        betAmount = 0;

        switch (result)
        {
            case RoundResult.Win:
                Debug.Log("You win");
                moneyAmount += stake * 2;               // stake back + winnings
                ChangeLootChance(+1, stake);
                break;

            case RoundResult.Lose:
                Debug.Log("You lose");                   // stake is gone
                ChangeLootChance(-1, stake);
                break;

            case RoundResult.Draw:
                Debug.Log("Draw");                       // stake is lost on a draw
                break;
        }

        // The revealed card becomes the new reference card
        refCardIndex = guessIndex;
        refCardValue = guessCardValue;
        refImage.sprite = cards[refCardIndex];
        guessImage.sprite = backCardSprite;

        // Re-stake: same amount after a win/loss, back to betStep (5) after a draw
        int desiredBet = (result == RoundResult.Draw) ? betStep : stake;
        int nextBet = Mathf.Min(desiredBet, moneyAmount, MaxBet);
        betAmount = nextBet;
        moneyAmount -= nextBet;

        UpdateUI();

        if (betAmount <= 0)
        {
            GameOver();
            yield break;
        }

        canBet = true;
    }

    // ---------------------------------------------------------- probabilities

    // The ref card itself can't be drawn, so the pool is the deck minus one card.
    // (Other suits with the same value can still be drawn, which gives a draw.)
    public float CalculateWinProbability(int currentValue, BetDirection direction)
    {
        int winning = 0;
        for (int i = 0; i < cardValues.Length; i++)
        {
            if (direction == BetDirection.Higher && cardValues[i] > currentValue) winning++;
            else if (direction == BetDirection.Lower && cardValues[i] < currentValue) winning++;
        }
        return (float)winning / Mathf.Max(1, cardValues.Length - 1);
    }

    // sign = +1 on a win, -1 on a loss
    void ChangeLootChance(int sign, int stake)
    {
        float probability = Mathf.Clamp01(winProbability);
        float betFactor = Mathf.Clamp01((float)stake / MaxBet);

        float delta;
        if (sign > 0)
        {
            // Winning a risky (unlikely) bet gives the biggest boost,
            // but even the safest bet always gives at least MinWinReward
            float risk = 1f - probability;
            float riskReward = Mathf.Max(MinWinReward, Mathf.Pow(risk, RiskPower));
            delta = MaxChanceIncrease * riskReward * betFactor;
        }
        else
        {
            // Losing a safe (likely) bet hurts the most
            delta = -MaxChanceDecrease * Mathf.Pow(probability, RiskPower) * betFactor;
        }

        lootBoxChance = Mathf.Clamp(lootBoxChance + delta, MinLootBoxChance, MaxLootBoxChance);
    }

    // ---------------------------------------------------------------- helpers

    static int ParseCardValue(Sprite sprite)
    {
        string[] parts = sprite.name.Split('_');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int value))
        {
            Debug.LogError($"Sprite '{sprite.name}' doesn't match the 'Name_Value' format.");
            return 0;
        }
        return value;
    }

    void UpdateUI()
    {
        moneyText.SetText("Tokens available: " + moneyAmount.ToString());
        betText.SetText("Bet tokens: " + betAmount.ToString());
        lootText.SetText(lootBoxChance.ToString("F2") + "%");
    }
}
