using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllScreen : MonoBehaviour
{
    int time;
   void Start()
    {
        PlayerPrefs.DeleteKey("Hp");
        PlayerPrefs.DeleteKey("Coins");
        PlayerPrefs.DeleteKey("Enemy");
    }
    void Update()
    {
        time++;
        if (time > 1000) SceneManager.LoadScene(0);
    }
}
