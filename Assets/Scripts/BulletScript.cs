using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage = 10;
    [SerializeField] private Animator animator; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Damage(damage, this.transform.rotation);
        }
        animator.SetTrigger("Hit");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
    }

    public void DestroyBullet(){
    //this is a functions called by an animation event on the bullet
        Destroy(this.gameObject);
    }
}
