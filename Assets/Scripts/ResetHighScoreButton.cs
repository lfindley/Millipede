using UnityEngine;

public class ResetHighScoreButton : MonoBehaviour
{
    public void ResetHighScore()
    {
        HighScoreManager.ResetHighScore();
        Debug.Log("High score reset!");
    }
}
