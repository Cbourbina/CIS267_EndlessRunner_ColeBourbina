using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int playerHealth;
    private float timer;
    private bool startGame = false;
    private bool endGame = false;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject gameInfo;
    [SerializeField]
    private TMP_Text startTip;
    [SerializeField]
    private GameObject upgradePanel;
    [SerializeField]
    private TMP_Text startTimer;
    [SerializeField]
    private TMP_Text playerHealthDisplay;
    [SerializeField]
    private TMP_Text playerScoreDisplay;
    [SerializeField]
    private TMP_Text playerScoreModDisplay;
    private float scoreMod;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startGame = true;
        }
        if (startGame || endGame )
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= 3)
            {
                timer = 0f;
                if (startGame)
                {
                    startPanel.SetActive(false);
                    gameInfo.SetActive(true);
                    upgradePanel.SetActive(false);
                    Time.timeScale = 1f;
                    startGame = false;
                }
                else
                {
                    gameOver();
                    endGame = false;
                }
            }
            if (startGame)
            {
                startTip.text = "Starting In";
                if (timer > 1)
                {
                    startTimer.text = "2";
                }
                if (timer > 2)
                {
                    startTimer.text = "1";
                }
            }
        }
        if (playerHealth <= 0)
        {
            endGame = true;
        }
        playerHealthDisplay.text = playerHealth.ToString();
        playerScoreDisplay.text = GetComponent<PlayerScore>().getScore().ToString();
        scoreMod = GetComponent<PlayerScore>().getScoreMod() / 10;
        if (scoreMod == 1)
        {
            playerScoreModDisplay.text = "MIN";
        }
        else if (scoreMod == 5)
        {
            playerScoreModDisplay.text = "MAX";
        }
        else
        {
            playerScoreModDisplay.text = scoreMod.ToString();
        }
    }

    public void gameOver()
    {
        Time.timeScale = 0f;
    }

    public void decreaseHealth()
    {
        playerHealth -= 10;
        GetComponent<PlayerScore>().resetScoreMod();
    }

    public void setHealth(int health)
    {
        playerHealth = health;
    }

    public int getHealth()
    {
        return playerHealth;
    }
}
