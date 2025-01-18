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
    public Button restart;

    private Player currentPlayer ;

    public IEnumerator StartGame() {

        yield return new WaitForSeconds(1f);

        while (currentRound <= 3) {

            restart.gameObject.SetActive(false);

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
        
    }


    // Use this for initialization
    void Start () {
        currentPlayer = new Player() ;
        restart.gameObject.SetActive(false);
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
            restart.gameObject.SetActive(true);
            restart.onClick.AddListener(() => Restart());
        } else {
            restart.gameObject.SetActive(false);
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

    //Restart the game
    public void Restart() {

        restart.gameObject.SetActive(false);
        prevScore = 0;
        currentRound = 1;
        currentBall = 1;
        semiFrameIndex = 0;
        Debug.Log("prevScore  " + prevScore);
        Debug.Log("currentRound  " + currentRound);
        Debug.Log("currentBall  " + currentBall);
        Debug.Log("semiFrameIndex  " + semiFrameIndex);

        for(int i = 0; i < 6; i++) {
            semiFrame[i] = 0;
            semiFrameBoard[i] = "";
            Debug.Log("semiFrame  " + semiFrame[i]);
            Debug.Log("semiFrameBoard  " + semiFrameBoard[i]);
        }
        for(int j = 0; j < 4; j++) {
            frame[j] = 0;
            frameBoard[j] = "";
            Debug.Log("frame  " + frame[j]);
            Debug.Log("frameBoard  " + frameBoard[j]);
        }
        StartCoroutine(StartGame());
    }


}
