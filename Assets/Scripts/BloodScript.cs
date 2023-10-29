using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    public GameObject bloodMask;

    // Start is called before the first frame update
    void Start()
    {
        bloodMask = Instantiate(bloodMask, this.transform.position, this.transform.rotation);
        bloodMask.transform.parent = this.transform;

    }

}
