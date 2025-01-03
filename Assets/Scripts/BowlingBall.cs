using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlingBall : MonoBehaviour {
    private bool finished = false;
    public Image ArrowFill;

    
    //Hides the arrow indicator
    public void HideArrow() {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    //Shows the arrow indicator
    public void ShowArrow() {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    //Fills the arrow based on the power given from the user
    public void SetPower(float power) {
        ArrowFill.fillAmount = power;
    }

    //Trigger when ball passes a certain point
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "FinishLine" && !finished) {
            finished = true;
            Invoke("FinishRound", 5f);
        }
    }

    //Finish the round when the ball passed a certain point
    private void FinishRound() {
        finished = false;
        GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().RoundEnd();
    }

    
}
