using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : DropTarget
{
	public Ingredient.FoodType contains;
	public bool full;
	public bool prepping;

	//TODO replace these sprites with an animation
	public Sprite prepSprite;
	public Sprite fullSprite;
	public float prepSpeed;
	float prepSpeedModified;

	public GameObject respawnPrefab;

	SpriteRenderer sr;
	Vector3 respawnPosition;
	Vector3 respawnScale;

    public void Start()
    {
    	full = false;
    	prepping = false;
    	GetComponent<Draggable>().enabled = false;
    	sr = GetComponent<SpriteRenderer>();
    }

    public override bool CheckObject(GameObject check)
    {
    	if(full) return false;
    	if(check.tag != "Ingredient") return false;
    	return true;
    }

    public override void OnDrop(GameObject dropped)
    {
    	respawnPosition = dropped.GetComponent<Draggable>().OriginalPosition();
    	respawnScale = dropped.transform.localScale;
    	contains = dropped.GetComponent<Ingredient>().type;
    	prepSpeedModified = prepSpeed * dropped.GetComponent<Ingredient>().speedModifier;
    	StartCoroutine("Prep");
    }

    IEnumerator Prep()
    {
    	prepping = true;

    	//TODO replace this color shift with an animation
    	sr.sprite = prepSprite;
    	sr.color = new Color(1f,.5f,.5f);
    	for(float i = .5f; i <= 1; i += prepSpeedModified)
    	{
    		sr.color = new Color(1,i,i);
    		yield return new WaitForSeconds(.1f);
    	}
    	sr.color = new Color(1,1,1);
    	sr.sprite = fullSprite;

    	full = true;
    	prepping = false;
    	GetComponent<Draggable>().enabled = true;
    	yield return null;
    }

    public void OnDestroy()
    {
    	if(gameObject.scene.isLoaded) //editor panics if we don't do this check
    	{
	    	GameObject respawn = Instantiate(respawnPrefab, respawnPosition, Quaternion.identity);
	    	respawn.transform.localScale = respawnScale;
	    	respawn.GetComponent<Ingredient>().type = contains;
	    }
    }
}
