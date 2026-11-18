using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int playerScore = 0;
    private int scoreModifier = 10;
    private float timer;
    public bool stopScore = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3)
        {
            if (!stopScore)
            {
                playerScore += ((5 * scoreModifier) / 10);
            }
            if (scoreModifier < 50)
            {
                scoreModifier += 1;
            }
            timer = 0f;
        }
    }

    public void resetScoreMod()
    {
        scoreModifier = 10;
    }

    public void addScore(int points)
    {
        if (!stopScore)
        {
            playerScore += ((points * scoreModifier) / 10);
        }
    }

    public int getScore()
    {
        return playerScore;
    }

    public float getScoreMod()
    {
        return scoreModifier;
    }
}
