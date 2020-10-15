using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meal : MonoBehaviour
{
	public List<List<Ingredient.FoodType>> dishes;
	public GameObject potPrefab;

	public GameManager gameManager;

	List<GameObject> pots;
	List<Vector3> spawnPosList;

	public void SpawnPots()
	{
		pots = new List<GameObject>();
		spawnPosList = new List<Vector3>();
		float tmp = 10/gameManager.potSlots;
		for(int i = 0; i < gameManager.potSlots; i++)
		{
			spawnPosList.Add(new Vector3(0,i*tmp-5,0));
		}
		dishes.ForEach(SpawnPot);
	}

	void SpawnPot(List<Ingredient.FoodType> ingredients)
	{
		potPrefab.GetComponent<Pot>().required = ingredients;
		Vector3 spawnPos = new Vector3(0,-5,0);
		for(int i = 0; i < spawnPosList.Count; i++)
		{
			if(Physics2D.OverlapBox(spawnPosList[i], new Vector2(1,1), 0) == null)
			{
				spawnPos = spawnPosList[i];
				break;
			}
		}
		GameObject pot = Instantiate(potPrefab, spawnPos, Quaternion.identity);
		pots.Add(pot);
	}

	public void Serve()
	{
		pots.ForEach(Destroy);
	}
}
