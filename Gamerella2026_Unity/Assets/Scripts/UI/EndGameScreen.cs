using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuScreen = null;
    
    [SerializeField]
    private TMP_Text _timeSurvivedText = null;

    [SerializeField]
    private TMP_Text _highScoreText = null;
    
    [SerializeField]
    private Color _highScoreAchievedColor = Color.green;
    
    private const string HighScoreKey = "HighScore";
    
    private const float HighScoreScale = 1.5f;
    private const float HighScoreScaleTimeSeconds = 1f;
    
    private Tween _showHighScoreTween = null;

    public void Show(EndGameData endGameData)
    {
        gameObject.SetActive(true);
        _timeSurvivedText.text = $"You survived: {GetFormattedTimeString(endGameData.TimeSurvivedSeconds)}";
        
        float currHighScore = PlayerPrefs.GetFloat(HighScoreKey, 0);
        
        _highScoreText.fontStyle = FontStyles.Normal;
        _highScoreText.color = Color.white;
        _highScoreText.transform.localScale = Vector3.one;
        
        if (endGameData.TimeSurvivedSeconds > currHighScore)
        {
            currHighScore = endGameData.TimeSurvivedSeconds;
            PlayerPrefs.SetFloat(HighScoreKey, endGameData.TimeSurvivedSeconds);
            
            _highScoreText.fontStyle = FontStyles.Underline;
            _highScoreText.color = _highScoreAchievedColor;
            
            _showHighScoreTween = _highScoreText.transform.DOScale(new Vector3(HighScoreScale, HighScoreScale, 1f), HighScoreScaleTimeSeconds)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        _highScoreText.text = $"High score: {GetFormattedTimeString(currHighScore)}";

    }

    public void Hide()
    {
        _showHighScoreTween?.Kill();
        
        gameObject.SetActive(false);
        _mainMenuScreen.SetActive(true);
    }

    private static string GetFormattedTimeString(float timeSurvivedSeconds)
    {
        return TimeSpan.FromSeconds(timeSurvivedSeconds).ToString(@"mm\:ss");
    }

    public class EndGameData
    {
        public float TimeSurvivedSeconds { get; set; }

        public EndGameData(float timeSurvivedSeconds)
        {
            TimeSurvivedSeconds = timeSurvivedSeconds;
        }
    }
}
