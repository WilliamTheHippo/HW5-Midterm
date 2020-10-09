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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0,0,2));
        Debug.Log(hit.collider);
        //TODO why isn't it hitting?
        //TODO add snapback, glowing, and hooks to other scripts
    }
}
