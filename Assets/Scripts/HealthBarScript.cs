using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Player player;
    private float startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = 7.76f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.SetLocalPositionAndRotation(new Vector3(GetHealthMaskPosition(), 0f, 0f), this.transform.rotation);

    }

    private float GetHealthMaskPosition(){
        float healthPercent = player.GetHealth() / 100f;
        float newHealthPosition = healthPercent * startingPosition;
        return newHealthPosition;

    }
}
