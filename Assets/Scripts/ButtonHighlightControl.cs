using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject NullButton;

    public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
 
    public void OnPointerExit(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(NullButton);
        }

    public void OnDisable()
        {
            if (NullButton != null){
                EventSystem.current.SetSelectedGameObject(NullButton);
            }
        }
 
}
