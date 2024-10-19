using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public bool isMoving;
    private bool isAnimating;
    public Vector3 target;
    Vector3 moveVelocity;

    // Singleton instance
    public static PlayerCamera Instance { get; private set; }

    void Start()
    {
        Instance = this;
        GameController.OnGameReset += OnGameReset;
        Player.OnKilled += OnPlayerKilled;
    }

    void LateUpdate()
    {
        if (!isAnimating && !isMoving){
            this.transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, this.transform.position.z);
        }
        else if (isMoving && !isAnimating){
            Vector3 positionMove = Vector3.SmoothDamp(this.transform.position, target, ref moveVelocity, 0.5f, 10f);
            this.transform.position = new Vector3(positionMove.x, positionMove.y, this.transform.position.z);
        }
    }

    void OnGameReset()
    {
        this.DeathCameraAnimation(false);
        this.isMoving = false;
    }

    void OnPlayerKilled()
    {
        // Begins moving camera for death animation
        this.target = Player.Instance.transform.position + Vector3.Normalize(Player.Instance.transform.rotation * new Vector3(0f,1f,0f));
        this.isMoving = true;
    }

    public void DeathCameraAnimation(bool dead)
    { 
        this.isAnimating = dead;
        //Triggers the Camera Death Animation Attached to the Main Camera
        this.GetComponent<Animator>().SetBool("playerDead", dead);
    }

    // Triggered by an event in the middle of the Camera Death animation
    public void ShowDeathMenu()
    { 
        GameController.Instance.SetDeathMenuActive();
    }
}