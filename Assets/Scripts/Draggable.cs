using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
	//NB: I couldn't get OverlapCollider to work, so you have to use a box, sorry
	public bool respawn;
	public bool destroy;

	public GameObject respawnPrefab;

	public Sprite dragSprite;
	Sprite normalSprite;
	SpriteRenderer sr;
	BoxCollider2D bc;
	Animator animator;

	Vector2 mousePosition;
	Vector3 originalPosition;
	Vector3 mouseOffset;
	Vector3 screenPosition;

	GameObject currentTarget;

	bool clicked;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		normalSprite = sr.sprite;
		currentTarget = null;
	}

	void OnMouseDown()  //only runs if we're on the collider (also this function apparently uses raycasts)
	{
		if(!enabled) return; //unity why
		clicked = true;
		if(dragSprite != null) sr.sprite = dragSprite;
		if(animator != null) animator.SetBool("Dragging", true);
		sr.color = new Color(1f,0.7f,0.7f);
		originalPosition = transform.position;
		mousePosition = Input.mousePosition;
		mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(mousePosition);
	}

	void OnMouseDrag()
	{
		if(!enabled || !clicked) return;
		mousePosition = Input.mousePosition;
		screenPosition = mousePosition;
		transform.position = Camera.main.ScreenToWorldPoint(screenPosition) + mouseOffset;
		Collider2D[] hits = Physics2D.OverlapBoxAll(
			transform.position,
			new Vector2(
				bc.size.x * transform.localScale.x,
				bc.size.y * transform.localScale.y
			),
			0
		);
		if( hits.Length > 1 &&
			hits[1].gameObject.GetComponent<DropTarget>() &&
			hits[1].gameObject.GetComponent<DropTarget>().CheckObject(this.gameObject)
		  )
		{
			if(currentTarget != null && currentTarget != hits[1].gameObject)
			{
				currentTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f);
			}
			currentTarget = hits[1].gameObject;
			currentTarget.GetComponent<SpriteRenderer>().color = new Color(0.7f,1f,0.7f);
		}
		else
		{
			if(currentTarget != null) currentTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f);
			currentTarget = null;
		}
	}

	void OnMouseUp()
	{
		if(!enabled || !clicked) return;
		clicked = false;
		sr.sprite = normalSprite;
		if(animator != null) animator.SetBool("Dragging", false);
		sr.color = new Color(1f,1f,1f);
		if(currentTarget != null)
		{
			currentTarget.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f);
			transform.position = currentTarget.transform.position;
			currentTarget.GetComponent<DropTarget>().OnDrop(this.gameObject);
			if(respawn)
			{
				if(respawnPrefab != null) Instantiate(respawnPrefab, originalPosition, Quaternion.identity);
				Instantiate(this.gameObject, originalPosition, Quaternion.identity);
			}
			if(destroy) Destroy(this.gameObject);
		}
		else transform.position = originalPosition;
	}

	public Vector3 OriginalPosition() {return originalPosition;}
	public bool IsBeingDragged() {return clicked;}
	public void SetNormalSprite(Sprite s) {normalSprite = s;}
}
