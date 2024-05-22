using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool Instance;
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public GameObject Initialization(string colorName, GameObject obj, Transform pos)
    {
        if (pool.TryGetValue(colorName, out Queue<GameObject> queue))
        {
            if (queue.Count == 0)
            {
                return CreateNewJewel(obj, pos);
            }
            else
            {
                GameObject newJewel = queue.Dequeue();
                newJewel.transform.position = pos.position;
                newJewel.transform.SetParent(pos);
                newJewel.SetActive(true);
                return newJewel;
            }
        }
        else
            return CreateNewJewel(obj, pos);
    }

    private GameObject CreateNewJewel(GameObject obj, Transform pos)
    {
        GameObject newJewel = Instantiate(obj, pos);
        newJewel.gameObject.SetActive(true);
        return newJewel;
    }

    public void ReturnJewels(string colorName, GameObject obj)
    {
        if (pool.TryGetValue(colorName, out Queue<GameObject> queue))
        {
            queue.Enqueue(obj);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(obj);
            pool.Add(colorName, newQueue);
        }
        obj.gameObject.SetActive(false);
    }
}