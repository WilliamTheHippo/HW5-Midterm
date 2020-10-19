using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : DropTarget
{
	public List<Ingredient.FoodType> required;
	List<Ingredient.FoodType> contains;

	public Table table;
	public int mealNumber;
	public Color mealColor;
	//public Meal partOfMeal;

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
	Animator animator;
	AudioSource sound;
	public AudioClip dump;
	public AudioClip cook;

	Vector3 originalPosition;

	public void Start()
	{
		mealNumber = 0; //"0" means unassigned
		originalPosition = transform.position;
		table = null;
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		sound = GetComponent<AudioSource>();
		emptySprite = sr.sprite;
		contains = new List<Ingredient.FoodType>();
		PopulateIndicators();
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
		animator.SetBool("Empty", false);
		if(required.Count > 0)
		{
			sound.clip = dump;
			sound.Play();
		}
		Ingredient.FoodType bowlContents = dropped.GetComponent<Bowl>().contains;
		if(required.Contains(bowlContents))
		{
			for(int i = 0; i < required.Count + contains.Count; i++)
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
		if(required.Count == 0)
		{
			sound.clip = cook;
			sound.Play();
			StartCoroutine("Cook");
		}
	}

	IEnumerator Cook()
	{
		//TODO instantiate timer?
		cooking = true;

		sr.sprite = cookingSprite;
		//sr.color = new Color(1f,.5f,.5f);
		animator.SetBool("Cooking", true);
		for(int i = cookingTime; i > 0; i--)
		{
			yield return new WaitForSeconds(1);
		}
		sr.color = mealColor;
		sr.sprite = cookedSprite;

		cooked = true;
		GetComponent<Draggable>().enabled = true;
		StartCoroutine("Cool");
		yield return null;
	}

	IEnumerator Cool()
	{
		animator.SetBool("Cooling", true);
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
			GameObject respawn = Instantiate(this.gameObject, originalPosition, Quaternion.identity);
			respawn.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
			respawn.GetComponent<SpriteRenderer>().sprite = emptySprite;
			respawn.GetComponent<Pot>().required = contains;
			respawn.GetComponent<Pot>().PopulateIndicators();
			//wtf why do I need to do this all of a sudden
			respawn.GetComponent<Pot>().enabled = true;
			respawn.GetComponent<AudioSource>().enabled = true;
			respawn.GetComponent<Animator>().enabled = true;
			respawn.GetComponent<BoxCollider2D>().enabled = true;
			//respawn.contains = new List<Ingredient.FoodType>(); //handled by Start()?
		}
		yield return null;
	}

	public void OnDestroy()
	{
		if(table != null) table.GetComponent<Table>().full = false;
	}

	public void PopulateIndicators()
	{
		for(int i = 0; i < required.Count; i++)
		{
			SpriteRenderer tmp_sr = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
			tmp_sr.sprite = ingredientSprites[(int)required[i]];
			tmp_sr.enabled = true;
		}
	}

	public Vector3 StartingPosition() {return originalPosition;}
	public void RefreshColor() {sr.color = mealColor;}
}
