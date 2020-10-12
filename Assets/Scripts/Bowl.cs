using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    //"contains" enum shows what kind of food it contains (food enum shared with Ingredient.cs)
    //draggable enabled only when food is prepped
    //show prepping with color shift for now, animation later

    public void OnDestroy()
    {
    	//respawn contained ingredient
    	//(Draggable.cs should respawn the bowl)
    }
}
