using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropTarget : MonoBehaviour
{
    public abstract void OnDrop(GameObject dropped);
}
