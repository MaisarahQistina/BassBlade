using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoring : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI FinalScoreText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }


    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
    
    public void UpdateScore()
    {
        ScoreText.text = "Score: " + score;
        FinalScoreText.text = "Final Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }
}
