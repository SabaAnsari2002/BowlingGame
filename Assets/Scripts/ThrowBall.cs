using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour {

    public GameObject ballPrefab;
    public GameObject pinPrefab;
    public GameObject cam;
    public Vector3 camSpawn;
    private Vector3 offset;
    public Transform ballSpawn;
    public Transform[] pinSpawn;
    [Range(0,100)]
    public float rotationSpeed = 5f;
    [Range(0, 10000)]
    public float powerSpeed = 100f;
    public Vector2 powerLimits = new Vector2(1000, 10000);
    public float currentPower;
    public Vector2 rotationLimits = new Vector2(-40, 40);
    private GameObject ball;
    private GameObject[] pins;
    private bool controlsEnabled;
    private bool startedToThrow = false;

    

    //The game begins
    public void Play() {
        Reset();
        EnableControls();
    }

    //When the second ball is to be thrown, this function sets the scene.
    //It resets the ball and hides the fallen pins
    public void PlayAgain() {
        ball.transform.position = ballSpawn.position;
        ball.transform.rotation = ballSpawn.rotation;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        cam.transform.position = camSpawn;
        HideFallenPins();
        EnableControls();
    }

    //Hide the fallen pins
    private void HideFallenPins() {
        for (int i = 0; i < pinSpawn.Length; i++) {
            if (pins[i].GetComponent<Pin>().Fallen) {
                pins[i].SetActive(false);
            }
        }
    }

    //When the round ends, the scene resets
    private void Reset() {
        ball.transform.position = ballSpawn.position;
        ball.transform.rotation = ballSpawn.rotation;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        cam.transform.position = camSpawn;

        for (int i = 0; i < pinSpawn.Length; i++) {
            pins[i].transform.position = pinSpawn[i].position;
            pins[i].transform.rotation = pinSpawn[i].rotation;
            pins[i].SetActive(true);
            pins[i].GetComponent<Pin>().Fallen = false;
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    //Enables the firing system
    private void EnableControls() {
        controlsEnabled = true;
        currentPower = 1000;
        startedToThrow = false;
        ball.GetComponent<BowlingBall>().ShowArrow();
        ball.GetComponent<BowlingBall>().SetPower(currentPower / powerLimits.y);
    }

    //Launches the ball
    private void Fire() {
        ball.GetComponent<Rigidbody>().AddForce(currentPower * ball.transform.forward);
        ball.GetComponent<BowlingBall>().HideArrow();
        controlsEnabled = false;
    }

    //When the ball is thrown, the camera follows, until it reaches z=7.5
    private void cameraFollow() {
        if (cam.transform.position.z > 7.5) {
            cam.transform.position = cam.transform.position;
        } else {
            cam.transform.position = ball.transform.position - offset;
        }
    }



    // Use this for initialization
    void Start () {
        ball = Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);
        pins = new GameObject[pinSpawn.Length];
        for(int i=0; i<pinSpawn.Length; i++) {
            pins[i] = Instantiate(pinPrefab, pinSpawn[i].position, pinSpawn[i].rotation);
        }
        camSpawn.x = 0f;
        camSpawn.y = 2.5f;
        camSpawn.z = -10f;
        offset = ball.transform.position - camSpawn;
        
    }

    // Update is called once per frame
    void Update() {
        cameraFollow();
        if (controlsEnabled) {
            var power = Input.GetAxis("Fire1");
            // Fire
            if (power == 0 && startedToThrow) {
                Fire();
                return;
            }

            // Power
            if (power > 0) {
                startedToThrow = true;
                currentPower += power * powerSpeed * Time.deltaTime;
                if (currentPower > powerLimits.y) {
                    currentPower = powerLimits.x;
                }
                ball.GetComponent<BowlingBall>().SetPower(currentPower / powerLimits.y);
            }

            // Rotation
            if (power == 0) {
                var horizontal = Input.GetAxis("Mouse X");
                if (Mathf.Abs(horizontal) > 0) {
                    var targetRotation = ball.transform.rotation.eulerAngles;
                    var targetY = targetRotation.y + horizontal * rotationSpeed * Time.deltaTime;
                    if (targetY < 0) targetY += 360;
                    targetY %= 360;
                    if (targetY > rotationLimits.y && targetY < 360 + rotationLimits.x) {
                        if (targetY > (rotationLimits.y + 360 + rotationLimits.x) / 2) {
                            targetY = rotationLimits.x;
                        } else {
                            targetY = rotationLimits.y;
                        }
                    }
                    targetRotation.y = targetY;
                    ball.transform.rotation = Quaternion.Euler(targetRotation);
                }
            }
        }
    }
}
