using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicked : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Should have jumped early");
    }
    public void WasClicked()
    {
        Debug.Log("Should have jumped now");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Should have jumped repete");
    }
}
