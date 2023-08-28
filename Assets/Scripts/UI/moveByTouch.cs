using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveByTouch : MonoBehaviour
{
    public bool right = false;
    public bool left = false;
    public Rigidbody2D friend;
    public float lateralForce;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Should be going right");
            right = true;
        }
        if (Input.GetKey(KeyCode.D)) { left = true; }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //touch.position to get info about where it is
            //touch.phase to get info about the state of the touch i.e. (Began, Ended, Moved, Stationary, Canceled)
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f;
            transform.position = touchPos;
        }
        //if (right) { transform.position = new Vector3(transform.position.x+0.1f, transform.position.y, transform.position.z); 
        if (right) { friend.AddForce(transform.right * lateralForce * Time.deltaTime); }
        if (left) { friend.AddForce(-transform.right * lateralForce * Time.deltaTime); }

        //for (int i = 0; i < Input.touchCount; i++) {
        //Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);      
        //Debug.DrawLine(Vector3.zero, touchPos, Color.red);
        //} 
    }
    public void moveRight()
    {
        Debug.Log("moveing right");
        right = true; 
    }
    public void moveLeft()
    {
        Debug.Log("moveing left");
        left = true;
    }
}
