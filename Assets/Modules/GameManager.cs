using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public float score;
    public float cash;
    public string difficulty;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI cashText;

    public void Start()
    {
        score = 0;
        cash = 0;

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            score += 1;
            cash += 1;
        }
        scoreText.text = new string("Score: " + score.ToString());
        cashText.text = new string("Tokens: " + cash.ToString());
    }

    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreToAdd = 0;
    }
}