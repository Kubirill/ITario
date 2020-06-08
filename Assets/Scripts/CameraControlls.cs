using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class CameraControls : MonoBehaviour
{
    //private PixelPerfectRendering cam;
    private Rigidbody2D rbody;
    private Vector2 movement = new Vector2(0, 0);
    public float speed = 5.0f;
   // public Text snappingTextLabel;

    public void ToggleSnapping()
    {
        //scam.pixelSnapping = !cam.pixelSnapping;

        UpdateSnappingText();
    }

    private void UpdateSnappingText()
    {
      //  snappingTextLabel.text = "PIXEL SNAPPING: " + (cam.pixelSnapping ? "ON" : "OFF");
    }

    // Start is called before the first frame update
    void Start()
    {
       //cam = GetComponent<PixelPerfectCamera>();
        rbody = GetComponent<Rigidbody2D>();
        UpdateSnappingText();
    }

    // Update is called once per frame
    void Update()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        movement.x = moveX;
        movement.y = moveY;

        rbody.AddForce(movement * speed * Time.deltaTime);
    }
}