using UnityEngine;

public class ResetHighScoreButton : MonoBehaviour
{
    public void ResetHighScore()
    {
        // Reset the saved high score
        HighScoreManager.ResetHighScore();

        // Also update GameManager's copy and UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RefreshHighScoreFromSave();
        }

        Debug.Log("High score reset!");
    }
}
