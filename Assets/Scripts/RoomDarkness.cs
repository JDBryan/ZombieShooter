using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDarkness : MonoBehaviour
{
    public void DestroyDarkness(){
        //This is a function to be called by an event in the animation FadeOut attached to area darkness
        Destroy(this.gameObject);
    }
}
