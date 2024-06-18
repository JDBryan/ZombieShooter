using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform playerTransform;
    public GameController gameController;
    public bool isMoving;
    private bool isAnimating;
    public Vector3 target;
    Vector3 moveVelocity;

    void LateUpdate()
    {
        if (!isAnimating & !isMoving){
            this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z);
        }
        else if (isMoving & !isAnimating){
            Vector3 positionMove = Vector3.SmoothDamp(this.transform.position, target, ref moveVelocity, 0.5f, 10f);
            this.transform.position = new Vector3(positionMove.x, positionMove.y, this.transform.position.z);
        }
    }

    public void GetNewTransform() 
    {
        this.playerTransform = FindObjectOfType<Player>().transform;
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z);
    }

    public void DeathCameraAnimation(bool dead){
        isAnimating = dead;
        this.GetComponent<Animator>().SetBool("playerDead", dead);
    }

    public void ShowDeathMenu(){
        gameController.SetDeathMenuActive();
    }
}