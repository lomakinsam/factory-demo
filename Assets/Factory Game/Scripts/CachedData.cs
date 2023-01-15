using UnityEngine;
using System.IO;

public class CachedData : MonoBehaviour
{
    public static CachedData Instance { get; private set; }

    public Leaderboard Leaderboard { get; private set; }
    public GameDifficulty PreferredDifficulty { get; private set; }

    private const string playerResultsFileName = "playerResults";
    private const string preferredDifficultyKey = "preferredDifficulty";

    private void Awake()
    {
        InitSingleton();
        LoadCachedData();
    }

    private void InitSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void LoadCachedData()
    {
        string playerResultsFilePath = Application.persistentDataPath + $"/{playerResultsFileName}.json";

        if (File.Exists(playerResultsFilePath))
        {
            string json = File.ReadAllText(playerResultsFilePath);
            PlayerBestResuts playerBestResuts = JsonUtility.FromJson<PlayerBestResuts>(json);
            Leaderboard = new(playerBestResuts.value);
        }
        else
            Leaderboard = new();

        if (PlayerPrefs.HasKey(preferredDifficultyKey))
            PreferredDifficulty = (GameDifficulty)PlayerPrefs.GetInt(preferredDifficultyKey);
        else
            PreferredDifficulty = GameDifficulty.Low;            
    }

    public void SavePlayerResult(PlayerResult playerResult)
    {
        bool isApplied = Leaderboard.ApplyToLeaderboard(playerResult);

        if (isApplied)
        {
            string playerResultsFilePath = Application.persistentDataPath + $"/{playerResultsFileName}.json";
            PlayerBestResuts playerBestResuts = new(Leaderboard.PlayerResults);
            string json = JsonUtility.ToJson(playerBestResuts);
            File.WriteAllText(playerResultsFilePath, json);
        }
    }

    public void SavePreferredDifficulty(GameDifficulty preferredDifficulty)
    {
        PreferredDifficulty = preferredDifficulty;
        PlayerPrefs.SetInt(preferredDifficultyKey, (int)preferredDifficulty);
    }
}

[System.Serializable]
public class PlayerBestResuts
{
    public PlayerResult[] value;

    public PlayerBestResuts(PlayerResult[] playerResults)
    {
        value = playerResults;
    }
}

public enum GameDifficulty { Low, Normal, Hard}