using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoor : Interactable
{
    [SerializeField] private Animator leftAnimator;
    [SerializeField] private Animator rightAnimator;
    public bool open;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
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
    }

    public override void Interact(){
        if (this.open == false){
            if (this.player.money >= this.baseCost){
                this.player.ChangePlayerMoney(-this.baseCost);
                this.Open();
            }
            else {
                this.TriggerFailedInteract();
            }
        }
    }
}
