using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {
    public bool Fallen = false;
    private float initialX;
    private float initialZ;
    private readonly float FallThreshold = 50f;
    

    // Use this for initialization
    void Start() {
        Reset();
    }

    private void Reset() {
        initialX = 0;
        initialZ = 0;
    }

    // Update is called once per frame
    void Update() {
        if (!Fallen) {
            var distanceX = Mathf.Abs(initialX - transform.rotation.eulerAngles.x % 360);
            var distanceZ = Mathf.Abs(initialZ - transform.rotation.eulerAngles.z % 360);
            if(distanceX + distanceZ > FallThreshold) {
                Fallen = true;
                GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().PinFall();
            }
        }
    }
}
