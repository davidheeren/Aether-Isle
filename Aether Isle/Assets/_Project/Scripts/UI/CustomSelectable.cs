using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game
{
    public class CustomSelectable : Selectable, IPointerClickHandler, ISubmitHandler
    {
        InputAction deltaMouse;

        public void OnPointerClick(PointerEventData eventData)
        {
            print("Click: " + name);
            
        }

        public void OnSubmit(BaseEventData eventData)
        {
            print("SUBMIT: " + name);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
