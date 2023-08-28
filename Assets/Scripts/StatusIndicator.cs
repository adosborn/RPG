using UnityEngine.UI;
using UnityEngine;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private RectTransform xpBarRect;
    [SerializeField]
    private Text xpText;
    [SerializeField]
    private Text coinText;
    [SerializeField]
    private RectTransform staminaRect;
    [SerializeField]
    private Text scoreText;

    void Start()
    {
        if (healthBarRect == null)
        {
            Debug.LogError("STATUS INDICATOR: No health bar object referenced!");

        }
        if (healthText == null)
        {
            Debug.LogError("STATUS INDICATOR: No health text object referenced!");

        }
    }

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _cur + "/" + _max + "HP";
    }

    public void SetXP(int lv, int _cur, int _max)
    {
        float _value = (float)_cur / _max;

        xpBarRect.localScale = new Vector3(_value, xpBarRect.localScale.y, xpBarRect.localScale.z);
        xpText.text = "Lv. " + lv + "     " + _cur + "/" + _max + "XP";
    }

    public void SetCoins(int coins) {
        coinText.text = coins + "";
    }

    public void SetScore(int score) {
        scoreText.text = score + "";
    }

    public void SetStamina(float _cur, int _max){
        float _value = (float)_cur / _max;
        if (_max != 0){
            staminaRect.localScale = new Vector3(_value, staminaRect.localScale.y, staminaRect.localScale.z);
        }
    }
}
