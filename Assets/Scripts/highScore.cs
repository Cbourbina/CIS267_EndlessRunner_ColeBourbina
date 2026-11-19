using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts
{
    internal class HighScore
    {
        private int score;
        private string date;
        private string name;

        public HighScore()
        {
        }

        public HighScore(int score, string date, string name)
        {
            this.score = score;
            this.date = date;
            this.name = name;
        }

        public void setScore(int playerScore)
        {
            score = playerScore;
        }

        public void setDate(string currentDate)
        {
            date = currentDate;
        }

        public void setName(string playerName)
        {
            name = playerName;
        }

        public int getScore()
        {
            return score;
        }

        public string getDate()
        {
            return date;
        }

        public string getName()
        {
            return name;
        }
    }
}
