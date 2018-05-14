using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{
	// Update is called once per frame
	void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(xMovement, 0.0f, zMovement);

        GetComponent<CharacterController>().Move(movement * Time.deltaTime * 10.0f);

        //GetComponent<Rigidbody>().velocity = new Vector3(xMovement, 0.0f, zMovement);
    }
}
