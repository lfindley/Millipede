using UnityEngine;

public static class HighScoreManager
{
    private const string KEY = "HighScore";

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(KEY, 0);
    }

    // Returns true if a new high score was saved
    public static bool TrySetHighScore(int score)
    {
        int current = GetHighScore();
        if (score > current)
        {
            PlayerPrefs.SetInt(KEY, score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(KEY);
        PlayerPrefs.Save();
    }
}

