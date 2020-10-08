using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Sprite dragSprite;
    Sprite normalSprite;
    SpriteRenderer sr;

    Vector2 mousePosition;
    Vector3 originalPosition;
	Vector3 mouseOffset;
	Vector3 screenPosition;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        normalSprite = sr.sprite;
    }

    void OnMouseDown()  //only runs if we're on the collider (also this function apparently uses raycasts)
    {
        if(dragSprite != null) sr.sprite = dragSprite;
        sr.color = new Color(1f,0.7f,0.7f);
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
        sr.sprite = normalSprite;
        sr.color = new Color(1f,1f,1f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        Debug.Log(hits);
        //TODO add snapback, glowing, and hooks to other scripts
        //TODO maybe just the first raycast; not sure if we need to see all the way down
    }
}
