using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : DropTarget
{
	public bool full;

	public void Start()
	{
		full = false;
	}
	
	public override bool CheckObject(GameObject check)
	{
		if(check.tag != "Pot") return false;
		if(!check.GetComponent<Pot>().cooked) return false;
		return true;
	}

    public override void OnDrop(GameObject dropped)
    {
    	full = true;
    	dropped.GetComponent<Pot>().table = this;
    }
}
