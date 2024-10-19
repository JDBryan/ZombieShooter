using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoor : Interactable
{
    [SerializeField] private Animator leftAnimator;
    [SerializeField] private Animator rightAnimator;
    public bool open;
    public List<string> connectedRooms;

    void Start()
    {
        this.Init();
        this.myRenderers.Add(this.transform.GetChild(0).GetComponent<SpriteRenderer>());
        this.myRenderers.Add(this.transform.GetChild(1).GetComponent<SpriteRenderer>());
        this.open = false;
    }

    private void Close(){
        leftAnimator.SetBool("Open", false);
        rightAnimator.SetBool("Open", false);
        this.open = false;
    }

    private void Open(){
        leftAnimator.SetBool("Open", true);
        rightAnimator.SetBool("Open", true);
        this.open = true;
        this.DisplayPrompt(false);
    }

    public override void Interact(){
        if (this.open == false){
            if (Player.Instance.money >= this.baseCost){
                Player.Instance.ChangePlayerMoney(-this.baseCost);
                this.Open();
                GameController.Instance.UpdateRooms(connectedRooms);
            }
            else {
                this.TriggerFailedInteract();
            }
        }
    }
}
