using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{
    #region Singleton

    public static Highscores instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Highscores found!");
            return;
        }
        instance = this;
    }
    #endregion
    public Text score1;
    public Text score2;
    public Text score3;
    public Text score4;
    public Text score5;

    public void UpdateScores(int one, int two, int three, int four, int five){
        if (one != 0){score1.text = one + "";}
        if (two != 0){score2.text = two + "";}
        if (three != 0){score3.text = three + "";}
        if (four != 0){score4.text = four + "";}
        if (five != 0){score5.text = five + "";}
    }
}
