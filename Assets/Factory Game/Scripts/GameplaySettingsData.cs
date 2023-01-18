using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Settings Data")]
public class GameplaySettingsData : ScriptableObject
{
    [SerializeField] 
    private List<GameplaySettingsPreset> settingsConfig;

    private Dictionary<GameDifficulty, GameplaySettings> cachedData = new();

    private void OnValidate() => CacheSerializeData();

    private void CacheSerializeData()
    {
        cachedData.Clear();

        foreach (var item in settingsConfig)
        {   
            if (!cachedData.ContainsKey(item.difficulty))
                cachedData.Add(item.difficulty, item.gameplaySettings);
        }
    }

    public GameplaySettings GetGameplaySettings(GameDifficulty difficulty)
    {
        bool containsRequestedSettings;
        containsRequestedSettings = cachedData.TryGetValue(difficulty, out GameplaySettings result);
        
        return containsRequestedSettings ? result : null;
    }
}

[System.Serializable]
public class GameplaySettingsPreset
{
    public GameDifficulty difficulty;
    [Space]
    public GameplaySettings gameplaySettings;
}

[System.Serializable]
public class GameplaySettings
{
    public int timeLimit;
    public int requiredRepairs;
    public int spawnLimit;
    public Vector2 spawnDelayRange;
}