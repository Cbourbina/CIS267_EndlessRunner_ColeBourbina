using Assets.Scripts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int playerScore = 0;
    private int scoreModifier = 10;
    private float timer;
    public bool stopScore = false;
    private List<HighScore> listOfHighScores = new List<HighScore>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readScores("highScores.txt");
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

    public void writeScores()
    {
        try
        {
            string fileName = "highScores.txt";
            string baseDirectory = Application.persistentDataPath;
            string filePath = Path.Combine(baseDirectory, fileName);
            StreamWriter writer = new StreamWriter(filePath);
            for (int i = 0; i < listOfHighScores.Count; i++)
            {
                writer.WriteLine($"{listOfHighScores[i].getScore()},{listOfHighScores[i].getDate()},{listOfHighScores[i].getName()}");
            }
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Error. File Not Found.");
        }

    }

    public void evaluateScore(string name)
    {
        DateTime date = DateTime.Now;
        HighScore highScore = new HighScore(playerScore, date.ToShortDateString(), name);
        if (listOfHighScores.Count < 6)
        {
            listOfHighScores.Add(highScore);
        }
        else
        {
            listOfHighScores[5] = highScore;
        }
        listOfHighScores.Sort((a,b) => b.getScore().CompareTo(a.getScore()));
        displayScoresToLog();
        writeScores();
    }

    public void displayScoresToLog()
    {
        for (int i = 0;i < listOfHighScores.Count; i++)
        {
            Debug.Log($"High Score {i + 1}: Score: {listOfHighScores[i].getScore()}, Date: {listOfHighScores[i].getDate()}, Name: {listOfHighScores[i].getName()}");
        }
    }
}
