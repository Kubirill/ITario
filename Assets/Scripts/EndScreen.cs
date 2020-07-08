using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Text money;
    public Text enemy;

    void Start()
    {
        money.text = "x " + PlayerPrefs.GetInt("Coins").ToString();
        enemy.text = "x " + PlayerPrefs.GetInt("Eneny").ToString();
        GUI g = GameObject.Find("BlackFon").GetComponent<GUI>();
        g.SetMark(PlayerPrefs.GetInt("Hp"));
    }

    
}
