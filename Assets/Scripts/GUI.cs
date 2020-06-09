using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Texture[] marks;
    private float varSpeed;
    public void Start()
    {

        
        
    }
    public void OffFon()
    {
        gameObject.SetActive(false);
        PlayerMove pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        pl.active = true;
    }
    public void SetMark(int mark)
    {
        GameObject child = GameObject.Find("Mark");
        child.GetComponent<RawImage>().texture = marks[mark-1];
    }
}
