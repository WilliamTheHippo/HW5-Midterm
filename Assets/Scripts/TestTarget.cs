using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : DropTarget
{
    public override void OnDrop(GameObject dropped)
    {
    	Debug.Log("test drop, received " + dropped.name);
    }
}
