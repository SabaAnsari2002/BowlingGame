using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public ThrowBall controls;
    public int currentRound;
    private int currentBall;
    private int currentScore;
    private int prevScore;
    private bool roundEnd;
    public Text pinsText;
    public Text[] semiFrameBoard;
    public Text[] frameBoard;
    private int[] semiFrame;
    private int semiFrameIndex;
    private int[] frame;
    public Button restart;

    public IEnumerator StartGame() {
        yield return new WaitForSeconds(1f);
        ResetGame();

        while (currentRound <= 10) {
            pinsText.text = "";
            roundEnd = false;
            currentScore = 0;
            controls.Play();
            yield return new WaitUntil(() => roundEnd);

            if (currentScore != 10) {
                prevScore = currentScore;
                semiFrame[semiFrameIndex] = currentScore;
                semiFrameBoard[semiFrameIndex].text = currentScore.ToString();
                semiFrameIndex++;
                
                currentBall++;
                roundEnd = false;
                currentScore = 0;
                controls.PlayAgain();
                yield return new WaitUntil(() => roundEnd);

                if (currentScore + prevScore == 10) {
                    pinsText.text = "Spare!";
                    semiFrame[currentRound * 2 - 1] = currentScore;
                    semiFrameBoard[currentRound * 2 - 1].text = "/";
                    frame[currentRound - 1] = 10;
                } else {
                    semiFrame[currentRound * 2 - 1] = currentScore;
                    semiFrameBoard[currentRound * 2 - 1].text = currentScore.ToString();
                    frame[currentRound - 1] = semiFrame[currentRound * 2 - 1] + semiFrame[currentRound * 2 - 2];
                }

                frameBoard[currentRound - 1].text = frame.Sum().ToString();
                semiFrameIndex++;
                prevScore = 0;
                currentBall = 1;
            } else {
                semiFrame[semiFrameIndex] = 10;
                semiFrameBoard[semiFrameIndex].text = "X";
                frame[currentRound - 1] = 10;
                semiFrameIndex += 2;
                pinsText.text = "Strike!";
                prevScore = 0;
            }

            yield return new WaitForSeconds(5f);
            currentRound++;
        }
    }

    void Start() {
        restart.gameObject.SetActive(false);
        ResetGame();
        StartCoroutine(StartGame());
    }

    void Update() {
        if (currentRound > 10) {
            frameBoard[10].text = frame.Sum().ToString();
            StopAllCoroutines();
            restart.gameObject.SetActive(true);
            restart.onClick.AddListener(() => Restart());
        } else {
            restart.gameObject.SetActive(false);
        }
    }

    public void PinFall() {
        currentScore++;
    }

    public void RoundEnd() {
        roundEnd = true;
    }

    public void Restart() {
        ResetGame();
        StartCoroutine(StartGame());
    }

    private void ResetGame() {
        prevScore = 0;
        currentRound = 1;
        currentBall = 1;
        semiFrame = new int[21];
        semiFrameIndex = 0;
        frame = new int[11];
        Array.Clear(semiFrame, 0, semiFrame.Length);
        Array.Clear(frame, 0, frame.Length);
    }
}
