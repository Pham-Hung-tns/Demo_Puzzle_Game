using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelsStorage : MonoBehaviour
{
    public List<JewelsController> ShapeObject;
    public List<Transform> positionSpawn;
    public static int amountOfJewel;

    private void Start()
    {
        EventsInGame.CreateJewels -= CreateNewJewels;
        EventsInGame.CreateJewels += CreateNewJewels;
        CreateNewJewels();
    }

    /// <summary>
    /// Create new jewels base on amountOfJewel
    /// </summary>
    public void CreateNewJewels()
    {
        foreach (var pos in positionSpawn)
        {
            var shapeIndex = Random.Range(0, ShapeObject.Count);
            GameObject jewelInstance = Pool.Instance.Initialization(ShapeObject[shapeIndex].gridColor.ToString(), ShapeObject[shapeIndex].gameObject
                , pos);
            amountOfJewel++;
        }
    }
}