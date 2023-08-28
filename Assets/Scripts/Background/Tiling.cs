using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 1; //offset so we dont get any weird errors 
    public bool hasARightBuddy = false; // these are used to check if we need to instantiate
    public bool hasALeftBuddy = false;
    public bool reverseScale = false; // used if the object is not tilable 
    private float spriteWidth = 0f; // the width of our element
    private Camera cam;
    private Transform myTransform;
    public Transform parents;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x * myTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // does it still need a buddy
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            // calculate the camara's extent (half the width) of what the camara can see in world cords
            float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;
            //calculate the x pos where the cam can see the edge of sprite
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtent;

            //checking if we can see the edge of the element and then calling MakeNewBuddy if we can 
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
                //Debug.Log("added right buddy");
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
                //Debug.Log("added left buddy");
            }
        }
    }
    // function that creates a buddy on the side required
    void MakeNewBuddy(int rightOrLeft) 
    {
        //calculating new position for buddy
        Vector3 oldScale = new Vector3(myTransform.localScale.x, myTransform.localScale.y, myTransform.localScale.z);
        Vector3 newPosition = new Vector3(myTransform.position.x + (spriteWidth-10) * rightOrLeft - rightOrLeft * (spriteWidth / 1000), myTransform.position.y, myTransform.position.z);
        //instantiating new buddy and storing him in a variable 
        Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation);
        // if not tilable reverse x size of object to get rid of any ugly seems
        newBuddy.localScale = new Vector3(myTransform.localScale.x, myTransform.localScale.y, myTransform.localScale.z);
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(oldScale.x * -1, oldScale.y, oldScale.z);
        }

        newBuddy.transform.parent = parents.transform;

        if (rightOrLeft < 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
            newBuddy.GetComponent<Tiling>().hasARightBuddy = false;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = false;
        }
    }
}
