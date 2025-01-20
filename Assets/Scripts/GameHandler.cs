using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class GameHandler : MonoBehaviour {

    public ThrowBall controls;
    public int currentRound;
    private int currentBall;
    private int currentScore;
    private int prevScore;
    private bool roundEnd;
    public Text pinsText;
  
    private string[] semiFrameBoard;
    private string[] frameBoard;

    public Text semiFrameBoard0;
    public Text semiFrameBoard1;
    public Text semiFrameBoard2;
    public Text semiFrameBoard3;
    public Text semiFrameBoard4;
    public Text semiFrameBoard5;

    public Text frameBoard0;
    public Text frameBoard1;
    public Text frameBoard2;
    public Text frameBoard3;

    private int[] semiFrame;
    private int semiFrameIndex;
    private int[] frame;

    private Player currentPlayer ;

    public GameObject endGameWindow; 
    public Text endGameMessage;  
    public Button nextLevelButton;   
    public int currentLevel = 1;

    public Button quitButton;
    public AudioSource backgroundMusic;

    public IEnumerator StartGame() {

        yield return new WaitForSeconds(1f);

        while (currentRound <= 3) {
            pinsText.text = "";
            roundEnd = false;
            currentScore = 0;
            controls.Play();
            yield return new WaitUntil(() => roundEnd); // Wait until we know the result

            if (currentScore != 10) {

                prevScore = currentScore;
                currentPlayer.AddScore(currentScore);

                semiFrame[semiFrameIndex] = currentScore;
                semiFrameBoard[semiFrameIndex] = currentScore.ToString();
                semiFrameIndex++;
            
                currentBall++;
                roundEnd = false;
                currentScore = 0;
                controls.PlayAgain();
                currentPlayer.AddScore(currentScore);

                yield return new WaitUntil(() => roundEnd); // Wait until we know the result

                if (currentScore + prevScore == 10) {

                    pinsText.text = "Spare!";
                    semiFrame[currentRound * 2 - 1] = currentScore;
                    semiFrameBoard[currentRound * 2 - 1] = "/";
                    frame[currentRound - 1] = 10;
                    frameBoard[currentRound - 1] = frame.Sum().ToString(); 
                    semiFrameIndex++;
                } else {

                    semiFrame[currentRound * 2 - 1] = currentScore;
                    semiFrameBoard[currentRound * 2 - 1] = currentScore.ToString();
                    frame[currentRound - 1] = semiFrame[currentRound * 2 - 1] + semiFrame[currentRound * 2 - 2];
                    frameBoard[currentRound - 1] = frame.Sum().ToString();
                    semiFrameIndex++;
                }
              
            } else {

                pinsText.text = "Strike!";
                semiFrame[semiFrameIndex] = 10;
                semiFrameBoard[semiFrameIndex++] = "X";
                frame[currentRound - 1] = 10;
                semiFrameBoard[semiFrameIndex++] = "";
                frameBoard[currentRound - 1] = frame.Sum().ToString(); 
            }

            currentBall = 1;
            prevScore = 0;

            yield return new WaitForSeconds(5f);            

            currentRound++;
        }
        
        frame[3] = 0;
        frame[3] = frame.Sum();
        frameBoard[3] = frame[3].ToString();

        if (currentRound > 3) {
        StopAllCoroutines();
        ShowEndGameWindow(frame[3]);  
        }    
    }

    public int GetRequiredScore() {
        return currentLevel * 5; 
    }

    private void ShowEndGameWindow(int totalScore) {
        endGameWindow.SetActive(true); 

        if (currentLevel == 5) { 
            if (totalScore == 30) {
                endGameMessage.text = "You Win!";
                endGameMessage.color = Color.green; 
                nextLevelButton.gameObject.SetActive(false);
            } else {
                endGameMessage.text = "Game Over!";
                endGameMessage.color = Color.red; 
                nextLevelButton.gameObject.SetActive(false); 
            }
        } else {
            if (totalScore >= GetRequiredScore()) {
                endGameMessage.text = "You Win!";
                endGameMessage.color = Color.green; 
                nextLevelButton.gameObject.SetActive(true);
                nextLevelButton.onClick.RemoveAllListeners();
                nextLevelButton.onClick.AddListener(() => LoadNextLevel());
            } else {
                endGameMessage.text = "Game Over!";
                endGameMessage.color = Color.red; 
                nextLevelButton.gameObject.SetActive(false); 
            }
        }

        quitButton.gameObject.SetActive(true);  
        quitButton.onClick.AddListener(() => QuitToMenu());  
    }


    private void LoadNextLevel() {
        currentLevel++;  
        PlayerPrefs.SetInt("CurrentLevel", currentLevel); 
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");  
    }

    private void QuitToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel() {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }

    // Use this for initialization
    void Start () {
        if (backgroundMusic != null) {
            backgroundMusic.Play();
        }
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); 
        currentPlayer = new Player() ;
        endGameWindow.SetActive(false);
        prevScore = 0;
        currentRound = 1;
        currentBall = 1;
        semiFrame = new int[6];
        semiFrameIndex = 0;
        frame = new int[4];

        semiFrameBoard = new string[6];
        frameBoard = new string[4];
      
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update() {

        BoardShow();

        if (currentRound > 3) {
            StopAllCoroutines();
        }         
    }

    void BoardShow() {

        semiFrameBoard0.text= semiFrameBoard[0];
        semiFrameBoard1.text= semiFrameBoard[1];
        semiFrameBoard2.text= semiFrameBoard[2];
        semiFrameBoard3.text= semiFrameBoard[3];
        semiFrameBoard4.text= semiFrameBoard[4];
        semiFrameBoard5.text= semiFrameBoard[5];

        frameBoard0.text = frameBoard[0];
        frameBoard1.text = frameBoard[1];
        frameBoard2.text = frameBoard[2];
        frameBoard3.text = frameBoard[3];
    }

    public void PinFall() {
        currentScore++;
    }

    public void RoundEnd() {
        print("Round ended with " + currentScore + " points.");
        roundEnd = true;
    }
}
