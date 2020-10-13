using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Meal> meals;
    public List<Table> tables;
    public int potSlots = 3;

    int freeSlots;

    public void Start()
    {
    	freeSlots = potSlots;
    	StartCoroutine("Serve");
    }

    IEnumerator Serve()
    {
    	while(meals.Count > 0)
    	{
    		if(meals[0].dishes.Count <= freeSlots)
    		{
    			freeSlots -= meals[0].dishes.Count;
    			Meal meal = meals[0];
    			//instantiate gameobject with meal in it?
    			meals.Remove(meal);
    		}
    		//re-increment freeSlots!
    		yield return null; //wait until ???
    	}
    }
}
