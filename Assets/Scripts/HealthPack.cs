using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healthAmount;
    private List<KnockBack> activeKnockBacks = new List<KnockBack>();

    void Start()
    {
        GameController.OnGameReset += () => Destroy(this.gameObject);
    }

    void FixedUpdate() {
        List<KnockBack> finishedKnockbacks = new List<KnockBack>();
        foreach(KnockBack knockback in this.activeKnockBacks) {
            GetComponent<Rigidbody2D>().MovePosition(transform.position + knockback.GetMoveDistance());
            if (knockback.IsOver()) {
                finishedKnockbacks.Add(knockback);
            }
        }   

        foreach(KnockBack knockback in finishedKnockbacks) {
            this.activeKnockBacks.Remove(knockback);
        }
    }

    public void AddKnockBack(Vector3 direction, float distance, float moveTime){
        this.activeKnockBacks.Add(new KnockBack(direction, distance, moveTime));
    }
}
