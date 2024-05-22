using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    private SpriteRenderer myRenderer;
    [HideInInspector] public bool selected;
    public float selectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init(){
        this.myRenderer = GetComponent<SpriteRenderer>();
        this.selected = false;
    }

    public void Select(){
        this.selected = true;
        this.myRenderer.material = selectedMaterial;
    }

    public void Deselect(){
        this.selected = false;
        this.myRenderer.material = defaultMaterial;
    }

    public virtual void Interact(){}
}
