using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightControl : 
    MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    ISelectHandler
{
    [SerializeField] private GameObject NullButton;

    public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

    public void OnSelect(BaseEventData eventData)
        {
            GameObject.Find("UserInterface").GetComponent<UserInterface>().PlayButtonSelectSound();
        }
 
    public void OnPointerExit(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(NullButton);
        }

    public void OnEnable()
        {
            if (NullButton != null){
                EventSystem.current.SetSelectedGameObject(NullButton);
            }
        }
 
}
