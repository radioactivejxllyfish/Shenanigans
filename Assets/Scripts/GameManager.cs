using System;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public float score;
    public float cash;
    public string difficulty;
    public Text scoreText;
    public Text cashText;

    public void Start()
    {
        score = 0;
        cash = 0;
        
    }

    public void Update()
    {
        scoreText.text = new string("Score: " + score.ToString());
        cashText.text = new string("Cash: " + cash.ToString());
    }

    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreToAdd = 0;
    }
}