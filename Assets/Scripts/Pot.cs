using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : DropTarget
{
    public List<Ingredient.FoodType> required;
    List<Ingredient.FoodType> contains;

    public bool cooking;
    public bool cooked;
    public int cookingTime;
    public float coolingTime;

    //TODO replace these sprites with an animation
    public Sprite cookingSprite;
    public Sprite cookedSprite;
    public Sprite[] ingredientSprites;
    Sprite emptySprite;

    SpriteRenderer sr;

    public void Start()
    {
    	sr = GetComponent<SpriteRenderer>();
    	emptySprite = sr.sprite;
    	contains = new List<Ingredient.FoodType>();
    	for(int i = 0; i < required.Count; i++)
    	{
    		SpriteRenderer tmp_sr = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
    		tmp_sr.sprite = ingredientSprites[(int)required[i]];
    		tmp_sr.enabled = true;
    	}
    	cooking = false;
    	GetComponent<Draggable>().enabled = false;
    }

    public override bool CheckObject(GameObject check)
    {
    	if(cooking) return false;
    	if(check.tag != "Bowl") return false;
    	if(!check.GetComponent<Bowl>().full) return false;
    	if(!required.Contains(check.GetComponent<Bowl>().contains)) return false;
    	return true;
    }

    public override void OnDrop(GameObject dropped)
    {
    	Ingredient.FoodType bowlContents = dropped.GetComponent<Bowl>().contains;
    	if(required.Contains(bowlContents))
    	{
    		for(int i = 0; i < required.Count; i++)
    		{
    			SpriteRenderer tmp_sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
    			if(tmp_sr.sprite == ingredientSprites[(int)bowlContents])
    			{
    				tmp_sr.enabled = false;
    			}
    		}
    		required.Remove(bowlContents);
    		contains.Add(bowlContents);
    	}
    	if(required.Count == 0) StartCoroutine("Cook");
    }

    IEnumerator Cook()
    {
    	//TODO instantiate timer?
    	cooking = true;

    	//TODO replace this color change with an animation
    	sr.sprite = cookingSprite;
    	sr.color = new Color(1f,.5f,.5f);
    	for(int i = cookingTime; i > 0; i--)
    	{
    		yield return new WaitForSeconds(1);
    	}
    	sr.color = new Color(1,1,1);
    	sr.sprite = cookedSprite;

    	cooked = true;
    	GetComponent<Draggable>().enabled = true;
    	StartCoroutine("Cool");
    	yield return null;
    }

    IEnumerator Cool()
    {
    	//TODO instantiate cooling indicator?
    	for(float i = 0; i < coolingTime/10; i += .01f)
    	{
    		if(!GetComponent<Draggable>().IsBeingDragged())
    		{
    			sr.color = new Color(1-i/5, 1-i/5, 1);
    		}
    		yield return new WaitForSeconds(.1f);
    	}

    	if(!GetComponent<Draggable>().IsBeingDragged())
    	{
    		Destroy(this.gameObject);
    		GameObject respawn = Instantiate(this.gameObject, transform.position, Quaternion.identity);
    		respawn.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
    		respawn.GetComponent<SpriteRenderer>().sprite = emptySprite;
    		respawn.GetComponent<Pot>().required = contains;
    		//respawn.contains = new List<Ingredient.FoodType>(); //handled by Start()?
    	}
    	yield return null;
    }
}
