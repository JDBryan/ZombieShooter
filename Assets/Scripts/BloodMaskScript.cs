using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMaskScript : MonoBehaviour
{
    private SpriteRenderer bloodRenderer;

    public void DestroyMask(){

        bloodRenderer = GetComponentInParent<SpriteRenderer>();
        bloodRenderer.maskInteraction = SpriteMaskInteraction.None;
        Destroy(this.gameObject);
    }

}
