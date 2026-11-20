using Assets.Scripts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GUIButtonHandler : MonoBehaviour
{
    private int randomNum;
    [SerializeField]
    private GameObject scoreScreen;
    private List<HighScore> listOfHighScores = new List<HighScore>();
    [SerializeField]
    private TMP_Text[] topScores;
    [SerializeField]
    private TMP_Text[] topScoreDates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Exiting Game");
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenuPlay()
    {
        SceneManager.LoadScene("Game");
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void how2Play()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void loadScoreScreen()
    {
        readScores("highScores.txt");
        scoreScreen.SetActive(true);
        for (int i = 0; i < listOfHighScores.Count; i++)
        {
            if (i != 5)
            {
                topScores[i].text = listOfHighScores[i].getScore().ToString();
                topScoreDates[i].text = listOfHighScores[i].getDate();
            }
        }
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
