using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public GameObject wizardButton;
    public bool isOpen = false;

    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetButtonDown("Inventory"))
    }

    void UpdateUI()
    {
       for (int i=0; i<slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
    public void OpenButton(GameObject classButton) {
        if (isOpen == false) {
            classButton.transform.position = new Vector3(classButton.transform.position.x - 375, classButton.transform.position.y, classButton.transform.position.z);
            isOpen = true;
        }
    }
    public void CloseButton(GameObject classButton) {
        Debug.Log(classButton.transform.position.x);
        if (isOpen == true && classButton.transform.position.x < 1200) {
            classButton.transform.position = new Vector3(classButton.transform.position.x + 375, classButton.transform.position.y, classButton.transform.position.z);
            isOpen = false;
        }
    }
    public void ShowCloseButton(GameObject closeButton) {
        if (isOpen == false) {
            closeButton.SetActive(true);
        }
    }

}
