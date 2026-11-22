using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static GameModifiers;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar = null;

    [SerializeField]
    private TMP_Text _healthText = null;

    [SerializeField]
    private TMP_Text _numTokensText = null;

    [SerializeField]
    private TMP_Text _lastLootText = null;

    [SerializeField]
    private TMP_Text _elapsedTimeText = null;
    
    [SerializeField]
    private TMP_Text _lootboxPct = null;
    
    [SerializeField]
    private Color _goodModifierColor = Color.green;
    
    [SerializeField]
    private Color _badModifierColor = Color.red;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
       UpdateUI();
    }

    private void UpdateUI()
    {
        // Health bar and tokens
        _healthBar.fillAmount = (float) Game.Instance.Player.Health / Game.Instance.Player.BaseHealth;
        _healthText.text = $"Health: {Game.Instance.Player.Health}";
        _numTokensText.text = $"Tokens: {Game.Instance.Player.NumTokens}";
        
        // Lootbox chance
        _lootboxPct.text = $"Next Lootbox Is Positive? {Game.Instance.Casino.lootBoxChance}%";
        _lootboxPct.color = Game.Instance.Casino.lootBoxChance >= 0.5f ? _goodModifierColor : _badModifierColor;
        
        // Elapsed time
        _elapsedTimeText.text = $"Interval: {Game.Instance.EnemySpawner.SpawnIntervalNum}, Elapsed Time: {EndGameScreen.GetFormattedTimeString(Game.Instance.ElapsedTimeSeconds)}";

        // Last loot text
        List<(ModifierType ModifierType, float Value)> lastModifiers = Game.Instance.GameModifiers.LastModifiers;
        if (lastModifiers.Count > 0)
        {
            int numRepeats = 0;
            (ModifierType lastModifierType, float lastModifierValue) = lastModifiers[^1];
            
            for (int i = lastModifiers.Count - 2; i >= 0; i--)
            {
                if (lastModifiers[i].ModifierType == lastModifierType)
                {
                    numRepeats++;
                }
                else
                {
                    break;
                }
            }
            
            bool isPositiveModifier = lastModifierValue >= 0f;
            Dictionary<ModifierType, string> modifiersText = isPositiveModifier ? GoodModifiersText : BadModifiersText;
            _lastLootText.text = $"Last Loot: {modifiersText[lastModifierType]}" + (numRepeats == 0 ? string.Empty : $" ({numRepeats + 1})");
        }
    }
}
