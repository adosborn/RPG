using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClassButtonHandler : MonoBehaviour
{
    #region singleton
    public static ClassButtonHandler instance;
    void Awake() {
        instance = this;
    }
    #endregion
    
    public Button ClassButton;
    public Button OutterButton;
    public GameObject ClassSelecterPrefab;
    public GameObject UIOverlay;
    public GameObject ClassButtonHolder;
    public Sprite WizardIcon;
    public Sprite KnightIcon;
    public GameObject CreatedButtonPrefab;
    public List<GameObject> invisibleButtons;
    public GameMaster gm;
    int numSlots;

    void Start() {
        if (ClassButtonHolder != null && CreatedButtonPrefab != null) {
            GenerateClassButtons();
            ShowOutter();
            Deselected();
        } 
    }

    public void GenerateClassButtons() {
        numSlots = System.Enum.GetValues(typeof(Player.Classes)).Length;
        int numCreated = 0;
        foreach (Player.Classes playerClass in Enum.GetValues(typeof(Player.Classes))){
            Vector3 rotationVector = new Vector3(0,0,numCreated*(360/numSlots));
            Quaternion rotation = Quaternion.Euler(rotationVector);
            var newButton = Instantiate(ClassSelecterPrefab, OutterButton.transform.position, rotation);
            var newSelecter = Instantiate(CreatedButtonPrefab, new Vector3(OutterButton.transform.position.x, OutterButton.transform.position.y, OutterButton.transform.position.z), rotation);
            newButton.transform.SetParent(OutterButton.transform, true);
            newSelecter.transform.SetParent(ClassButtonHolder.transform, true);
            newSelecter.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0,0,numCreated*(360/numSlots)));
            newButton.GetComponent<Image>().fillAmount = 1.0f/numSlots;
            if (playerClass == Player.Classes.Wizard) {
                newButton.GetComponent<Image>().color = new Color(0.286f, 0.690f, 0.890f, 0.5f);
                newButton.transform.GetChild(0).GetComponent<Image>().sprite = WizardIcon;
                newButton.transform.GetComponent<NameProperty>().ClassName = "Wizard";
                newSelecter.transform.GetComponent<ClassSelecter>().ClassName = "Wizard";
            }
            if (playerClass == Player.Classes.Knight) {
                newButton.GetComponent<Image>().color = new Color(0.690f, 0.651f, 0.647f, 0.5f);
                newButton.transform.GetChild(0).GetComponent<Image>().sprite = KnightIcon;
                newButton.transform.GetComponent<NameProperty>().ClassName = "Knight";
                newSelecter.transform.GetComponent<ClassSelecter>().ClassName = "Knight";
            }
            invisibleButtons.Add(newSelecter);
            numCreated ++;
        }
    }
    
    public void ShowOutter() {
        foreach (GameObject invisible in invisibleButtons) {
            invisible.SetActive(true);
        }
        OutterButton.gameObject.SetActive(true);
        ClassButton.interactable = false;
    }

    public void Deselected() {
        foreach (GameObject invisible in invisibleButtons) {
            invisible.SetActive(false);
        }
        OutterButton.gameObject.SetActive(false);
        ClassButton.interactable = true;
    }
    
    public void Select(string className) {
        Debug.Log(className);
        if (className == "Wizard") { gm.SwitchToWizard(); }
        if (className == "Knight") { gm.SwitchToKnight(); }
       
    }
}
