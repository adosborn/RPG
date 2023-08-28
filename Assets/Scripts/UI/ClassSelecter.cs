using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelecter : MonoBehaviour
{
    
    public string ClassName = "";
    public GameObject[] SelecterImages;
    public GameObject SelecterImage;
    void Start() {
        SelecterImages = GameObject.FindGameObjectsWithTag("ClassSelecter");
        foreach (GameObject newImage in SelecterImages) {
            if (newImage.transform.GetComponent<NameProperty>().ClassName == ClassName) {
                SelecterImage = newImage;
            }
        }
        gameObject.SetActive(false);
    }
    public void SelectClass() {
        Debug.Log(ClassName + " is the name");
        ClassButtonHandler.instance.Select(ClassName);
    }
    public void Hover() {
        Color newColor = new Color(0,0,0,1);
        newColor = SelecterImage.GetComponent<Image>().color;
        newColor.a = 0.75f;

        SelecterImage.GetComponent<Image>().color = newColor;
    }

    public void UnHover() {
        Color newColor = new Color(0,0,0,1);
        newColor = SelecterImage.GetComponent<Image>().color;
        newColor.a = 0.5f;
        SelecterImage.GetComponent<Image>().color = newColor;
        SelectClass();
    }

    public void Deselect() {
        Debug.Log("Deselecting");
    }
}
