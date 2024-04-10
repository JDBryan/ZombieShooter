using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoor : MonoBehaviour
{

    [SerializeField] private Animator leftAnimator;
    [SerializeField] private Animator rightAnimator;

    public bool open;

    // Start is called before the first frame update
    void Start()
    {
        this.open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.open){
            leftAnimator.SetBool("Open", true);
            rightAnimator.SetBool("Open", true);
        }
    }
}
