using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private float movX;
    [SerializeField] private float movY;

    [SerializeField] private bool touched;
    [SerializeField] private RectTransform joystickBack;
    [SerializeField] private RectTransform joystick;

    private float radius;

    private void Start()
    {
        radius = joystickBack.rect.width * 0.5f;
    }
    private void Update() {
        if(movX != 0 || movY != 0)
            PlayerController.Instance.Move(movX, movY);
    }

    void OnTouch(Vector2 touch)
    {
        Vector2 vec = new Vector2 (touch.x - joystickBack.position.x, touch.y - joystickBack.position.y);
        vec = Vector2.ClampMagnitude(vec, radius);
        joystick.localPosition = vec;
        Vector2 normal = vec.normalized; 
        movX = normal.x;
        movY = normal.y;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        touched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.localPosition = Vector3.zero;
        movX = 0;
        movY = 0;
        touched = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        touched = true;
    }
}
