using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Casino : MonoBehaviour
{
    public int moneyAmount = 50;
    public int betAmount = 5;

    public GameObject[] dealtCard;

    public int refCard;

    public int cardColorGenerate;
    public int cardGenerate;
    

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI betText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        Debug.Log("High");
        FlipCard();
    }
    void BetLow()
    {
        Debug.Log("Low");
        FlipCard();
    }

    void FlipCard()
    {
        cardColorGenerate = Random.Range(0,3);
        Debug.Log(cardColorGenerate);

        cardGenerate = Random.Range(1,13);
        Debug.Log(cardGenerate);

        for ( int i = 0; i< dealtCard.Length; i++)
        {
            dealtCard[i].SetActive(false);
        }
        
        dealtCard[cardGenerate].SetActive(true);
    }


    void UpdateMoney()
    {
        moneyText.SetText(moneyAmount.ToString());
        betText.SetText(betAmount.ToString());
    }
}


