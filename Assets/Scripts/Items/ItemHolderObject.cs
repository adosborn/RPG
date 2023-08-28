using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemHolderObject : MonoBehaviour
{
    public GameObject ItemHolder;
    public Item item;
    public bool hasBeenPickedUp;
    public Vector3 position;
    public string holderName;
    public int scene;
    
    public ItemHolderObject(GameObject newHolder, Item newItem, bool pickedUp, 
    Vector3 newPosition, string newHolderName, int newSceneNum) {
        ItemHolder = newHolder;
        item = newItem;
        hasBeenPickedUp = pickedUp;
        position = newPosition;
        holderName = newHolderName;
        scene = newSceneNum;
    }

    void Start() {
        if (hasBeenPickedUp == true || SceneManager.GetActiveScene().buildIndex != scene) {
            ItemHolder.SetActive(false);
        }
    }
}
