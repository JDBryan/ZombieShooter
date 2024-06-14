using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material failMaterial;
    [HideInInspector] public List<SpriteRenderer> myRenderers;
    [HideInInspector] public bool selected;
    [HideInInspector] private bool failed;
    [HideInInspector] private float waitTime;
    [SerializeField] public int baseCost;
    [HideInInspector] public Player player;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init(){
        this.myRenderers = new List<SpriteRenderer>();
        if (GetComponent<SpriteRenderer>() != null){
            this.myRenderers.Add(GetComponent<SpriteRenderer>());
        }
        this.selected = false;

        this.failed = false;
        this.waitTime = 0f;
    }

    private void LateUpdate(){
        //this changes the material when a purchace fails for a small amount of time. 
        if (this.failed == true){
            this.waitTime += Time.deltaTime;
            foreach (SpriteRenderer renderer in this.myRenderers){
                renderer.material = failMaterial;
            }
            if (this.waitTime > 0.1f){
                this.waitTime = 0.0f;
                this.failed = false;
            }
        }
    }

    public void Select(){
        this.selected = true;
        foreach (SpriteRenderer renderer in this.myRenderers){
            renderer.material = selectedMaterial;
        }
    }

    public void Deselect(){
        this.selected = false;
        foreach (SpriteRenderer renderer in this.myRenderers){
            renderer.material = defaultMaterial;
        }
    }

    public void TriggerFailedInteract(){
        this.failed = true;
    }

    public virtual void Interact(Player player){}
}
