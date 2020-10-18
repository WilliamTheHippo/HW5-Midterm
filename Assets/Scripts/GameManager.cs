using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public List<Table> tables;
	public List<Pot> pots;
	public int maxMeals;

	List<List<Ingredient.FoodType>> order;

	public GameObject potPrefab;

	int mealNumber; //1-indexed for consistency, 0 means undefined
	int mealSize;
	List<Color> mealColors;

	public void Start()
	{
		mealNumber = 0;
		mealColors = new List<Color>();
		StartCoroutine("OrderUp");
		//TODO lose if order takes too long
	}

	IEnumerator OrderUp()
	{
		while(mealNumber < maxMeals)
		{
			mealNumber++;
			mealColors.Add(new Color(
				Random.Range(.7f,1f),
				Random.Range(.7f,1f),
				Random.Range(.7f,1f)
			));
			tables.ForEach(Serve);
			List<Pot> emptyPots = pots.FindAll(delegate(Pot pot){
				return pot.mealNumber == 0;
			});
			order = new List<List<Ingredient.FoodType>>();
			for(int i = 0; i < Random.Range(1,emptyPots.Count+1); i++)
			{
				order.Add(Ingredient.NewRecipe(Random.Range(1,4)));
			}
			mealSize = order.Count;
			emptyPots.ForEach(Assign);
			yield return new WaitUntil(MealReady);
		}
		//TODO success
		Debug.Log("Win");
	}

	bool MealReady() //has to be lean; run a lot
	{
		if(tables.FindAll(delegate(Table table){
			return table.pot != null &&
				table.pot.mealNumber == mealNumber;
		}).Count == mealSize) return true;
		else return false;
	}

	void Assign(Pot pot)
	{
		if(order.Count > 0)
		{
			pot.mealNumber = mealNumber;
			pot.mealColor = mealColors[mealNumber-1]; //because 1-indexing
			pot.RefreshColor();
			pot.required = order[0];
			pot.gameObject.transform.position = pot.StartingPosition();
			order.Remove(order[0]);
			pot.PopulateIndicators();
		}
	}

	void Serve(Table table)
	{
		if(table.pot != null)
		{
			GameObject pot = Instantiate(
				potPrefab,
				table.pot.StartingPosition(),
				Quaternion.identity
			);
			pots.Add(pot.GetComponent<Pot>());
			Destroy(table.pot.gameObject);
			pots.Remove(table.pot);
		}
		table.currentMeal++;
	}
}
