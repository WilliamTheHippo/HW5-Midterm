using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : DropTarget
{
	public int currentMeal;
	public bool full;

	public Pot pot;

	public void Start()
	{
		//currentMeal = 0; //WTFrickityFrak??
		full = false;
	}
	
	public override bool CheckObject(GameObject check)
	{
		if(check.tag != "Pot") return false;
		if(!check.GetComponent<Pot>().cooked) return false;
		if(check.GetComponent<Pot>().mealNumber != currentMeal) return false;
		return true;
	}

	public override void OnDrop(GameObject dropped)
	{
		full = true;
		dropped.GetComponent<Animator>().SetBool("Plated", true);
		dropped.GetComponent<Pot>().table = this;
		pot = dropped.GetComponent<Pot>();
	}
}
