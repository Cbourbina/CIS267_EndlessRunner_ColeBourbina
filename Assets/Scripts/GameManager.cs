using Assets.Scripts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
    private GameObject gameOverPanel;
    [SerializeField]
    private TMP_Text startTimer;
    [SerializeField]
    private TMP_Text playerHealthDisplay;
    [SerializeField]
    private TMP_Text playerScoreDisplay;
    [SerializeField]
    private TMP_Text playerScoreModDisplay;
    [SerializeField]
    private TMP_Text playerScoreGameOver;
    [SerializeField]
    private TMP_Text[] topScores;
    [SerializeField]
    private TMP_Text[] topScoreDates;
    private float scoreMod;
    private bool gameOverBool = false;
    private List<HighScore> listOfHighScores = new List<HighScore>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverBool == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && GetComponent<PlayerScore>().getScore() == 0)
            {
                startGame = true;
            }
            if (startGame || endGame)
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
                        endGame = false;
                        gameOverBool = true;
                        gameOver();
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
    }

    public void gameOver()
    {
        GetComponent<PlayerScore>().evaluateScore("");
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        gameInfo.SetActive(false);
        readScores("highScores.txt");
        for (int i = 0; i < listOfHighScores.Count; i++)
        {
            if (i != 5)
            {
                topScores[i].text = listOfHighScores[i].getScore().ToString();
                topScoreDates[i].text = listOfHighScores[i].getDate();
            }
        }

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

    public void readScores(string fileName)
    {
        try
        {
            string baseDirectory = Application.persistentDataPath;
            string filePath = Path.Combine(baseDirectory, fileName);
            StreamReader reader = new StreamReader(filePath);
            char delim = ',';
            int delimPosition;
            string line = reader.ReadLine();
            while (line != null)
            {
                HighScore highScore = new HighScore();
                delimPosition = line.IndexOf(delim);
                highScore.setScore(Int32.Parse(line.Substring(0, delimPosition)));

                line = line.Substring(delimPosition + 1);
                delimPosition = line.IndexOf(delim);
                highScore.setDate(line.Substring(0, delimPosition));


                line = line.Substring(delimPosition + 1);
                highScore.setName(line);

                listOfHighScores.Add(highScore);
                line = reader.ReadLine();
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Error. File Not Found.");
        }
    }
}
