using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoor : Interactable
{
    [SerializeField] private Animator leftAnimator;
    [SerializeField] private Animator rightAnimator;
    public bool open;
    private GameController gameController;
    public List<string> connectedRooms;

    // Start is called before the first frame update
    void Start()
    {
        this.Init();
        this.myRenderers.Add(this.transform.GetChild(0).GetComponent<SpriteRenderer>());
        this.myRenderers.Add(this.transform.GetChild(1).GetComponent<SpriteRenderer>());
        this.open = false;
        this.gameController = FindObjectOfType<GameController>();
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

    public override void Interact(Player player){
        if (this.open == false){
            if (player.money >= this.baseCost){
                player.ChangePlayerMoney(-this.baseCost);
                this.Open();
                this.gameController.UpdateRooms(connectedRooms);
            }
            else {
                this.TriggerFailedInteract();
            }
        }
    }
}
