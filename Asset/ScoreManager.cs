using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager: MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText; 
    [SerializeField] public TextMeshProUGUI highscoreText;   
    [SerializeField] public TextMeshProUGUI gameOverScore;
    int score = 0;
    int highscore = 0;
    public void Start(){
        highscore= PlayerPrefs.GetInt("highscore",0);
        scoreText.text = score.ToString() + " POINTS"; 
        highscoreText.text = "HIGHSCORE: "+highscore.ToString();
        gameOverScore.text = "YOUR SCORE : " + score.ToString() + " POINTS"; 
    }
// Start is called before the first frame update {
        
// Update is called once per frame
    public void Update(){
        scoreText.text = score.ToString() + " POINTS"; 
        highscoreText.text = "HIGHSCORE: "+highscore.ToString();
        gameOverScore.text = "YOUR SCORE : " + score.ToString() + " POINTS";
    }

    public void scoreUpdate(int Score){
        Debug.Log(Score);
        score = Score;
        if(score> highscore){
            highscore = score;
            PlayerPrefs.SetInt("highscore", score);
        }
    }
}