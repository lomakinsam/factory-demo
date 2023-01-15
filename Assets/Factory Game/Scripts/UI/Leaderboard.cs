public class Leaderboard
{
    public int highestTopScore { get; private set; }
    public int lowestTopScore { get; private set; }

    private const int size = 5;

    public PlayerResult[] PlayerResults { get; private set; }

    public Leaderboard()
    {
        PlayerResults = new PlayerResult[size];

        for (int i = 0; i < size; i++)
            PlayerResults[i] = new();

        lowestTopScore = 0;
        highestTopScore = 0;
    }

    public Leaderboard(PlayerResult[] playerResults)
    {
        PlayerResults = playerResults;

        for (int i = 0; i < size; i++)
        {
            if (PlayerResults[i].score > highestTopScore)
                highestTopScore = PlayerResults[i].score;

            if (PlayerResults[i].score < lowestTopScore)
                lowestTopScore = PlayerResults[i].score;
        }
    }

    public bool ApplyToLeaderboard(PlayerResult playerResult)
    {
        bool isApplied = false;

        if (playerResult.score > lowestTopScore)
        {
            PlayerResults[size - 1] = playerResult;
            lowestTopScore = playerResult.score;

            if (playerResult.score > highestTopScore)
                highestTopScore = playerResult.score;

            for (int i = size - 2; i >= 0; i--)
            {
                if (PlayerResults[i].score < lowestTopScore)
                    lowestTopScore = PlayerResults[i].score;

                if (PlayerResults[i].score < playerResult.score)
                    (PlayerResults[i], PlayerResults[i + 1]) = (PlayerResults[i + 1], PlayerResults[i]);
                else break;
            }

            isApplied = true;
        }

        return isApplied;
    }
}

[System.Serializable]
public class PlayerResult
{
    public int score;
    public int timeInSeconds;

    public PlayerResult()
    {
        score = 0;
        timeInSeconds = 0;
    }

    public PlayerResult(int score, int timeInSeconds)
    {
        this.score = score;
        this.timeInSeconds = timeInSeconds;
    }
}