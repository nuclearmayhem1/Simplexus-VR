using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : MonoBehaviour
{
    public string name = "Unnamed";
    public GameObject Terrain;
    public GameObject PlacedObjects;
}
