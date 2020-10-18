using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
	public enum FoodType {Type1, Type2, Type3, Type4, Type5}
	public FoodType type;
	public float speedModifier = 1;
	public Sprite[] sprites;

	public void Start()
	{
		GetComponent<SpriteRenderer>().sprite = sprites[(int)type]; //hacky hacky
		GetComponent<Draggable>().SetNormalSprite(sprites[(int)type]);
	}

	public Sprite TypeToSprite(FoodType type)
	{
		return sprites[(int)type];
	}

	public static List<FoodType> NewRecipe(int length)
	{
		List<FoodType> recipe = System.Enum.GetValues(typeof(FoodType))
			.Cast<FoodType>().ToList();
		while(recipe.Count > length)
		{
			recipe.Remove(recipe[Random.Range(0,recipe.Count)]);
		}
		return recipe;
	}
}
