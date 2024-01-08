using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform playerTransform;

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z);
    }

    public void GetNewTransform() 
    {
        this.playerTransform = FindObjectOfType<Player>().transform;
    }
}