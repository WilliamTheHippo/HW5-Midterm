using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector2 mousePosition;
    Vector3 originalPosition;
	Vector3 mouseOffset;
	Vector3 screenPosition;

    void OnMouseDown()  //only runs if we're on the collider
    {
        originalPosition = transform.position;
    	mousePosition = Input.mousePosition;
    	mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void OnMouseDrag()
    {
    	mousePosition = Input.mousePosition;
    	screenPosition = mousePosition;
    	transform.position = Camera.main.ScreenToWorldPoint(screenPosition) + mouseOffset;
    }

    void OnMouseUp()
    {
        
    }
}
