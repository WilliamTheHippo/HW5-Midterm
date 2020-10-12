using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : DropTarget
{
    public List<Ingredient.FoodType> required;
    List<Ingredient.FoodType> contains;

    public bool cooking;
    public int cookingTime;

    public void Start()
    {
    	contains = new List<Ingredient.FoodType>();
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
    		required.Remove(bowlContents);
    		contains.Add(bowlContents);
    	}
    	if(required.Count == 0) StartCoroutine("Cook");
    }

    IEnumerator Cook()
    {
    	//TODO instantiate timer?
    	cooking = true;
    	StartCoroutine("Cool");
    	yield return null;
    }

    IEnumerator Cool()
    {
    	//TODO instantiate cooling indicator?
    	yield return null;
    }
}
