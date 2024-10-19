using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    public GameObject bloodMask;

    void Start()
    {
        GameController.OnGameReset += () => Destroy(this.gameObject);
        bloodMask = Instantiate(bloodMask, this.transform.position, this.transform.rotation);
        bloodMask.transform.parent = this.transform;
    }

}
