using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public Rigidbody rb;
	[SerializeField]
	 float power;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		rb.AddForce(Vector3.forward * power);
	}
}
