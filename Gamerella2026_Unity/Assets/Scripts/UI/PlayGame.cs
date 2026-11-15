using UnityEngine;

public class PlayGame : MonoBehaviour
{
    [SerializeField]
    private Game _gamePrefab = null;
    
    [SerializeField]
    private EndGameScreen _endGameScreen = null;
    
    public void Play()
    {
        Game game = Instantiate(_gamePrefab);
        game.Init(endGameData => _endGameScreen.Show(endGameData));
    }
}
