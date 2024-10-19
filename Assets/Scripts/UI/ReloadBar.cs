using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    [Range(0f,1f)]
    //this is the variable to access the slider position, It's a slider in the inspector
    //cause its easier to test, but feel free to remove that when you impliment it.
    public float progress; 
    private GameObject slider;
    private float startPosition;
    private float endPosition;
    // Start is called before the first frame update
    void Start()
    {
        this.progress = 0f;
        this.slider = this.transform.GetChild(0).gameObject;
        this.startPosition = -0.7f;
        this.endPosition = 0.7f;
    }

    void Update()
    {
        this.slider.transform.localPosition = new Vector3(xPosition(),this.slider.transform.localPosition.y,0f);
    }

    private float xPosition()
    {
        float scale = this.endPosition - this.startPosition;
        return (this.progress * scale) + this.startPosition; 
    }
}
