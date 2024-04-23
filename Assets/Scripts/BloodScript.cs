using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    public GameObject bloodMask;
    private GameController gameController;
    private GameObject bloodParent;

    // Start is called before the first frame update
    void Start()
    {
        this.gameController = GameObject.FindObjectOfType<GameController>();
        this.bloodParent = this.gameController.GetComponent<GameController>().bloodParent;

        bloodMask = Instantiate(bloodMask, this.transform.position, this.transform.rotation);
        bloodMask.transform.parent = this.transform;

        this.transform.parent = this.bloodParent.transform;

    }

}
