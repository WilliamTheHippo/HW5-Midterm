using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public enum FoodType {Type1, Type2, Type3, Type4, Type5}
    public FoodType type;
    public float speedModifier = 1;
}
