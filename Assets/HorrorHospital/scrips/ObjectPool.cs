using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objprefab;
    public int createOnStart;

    private List<GameObject> poolObjs = new List<GameObject>();

    private void Start()
    {
        for(int x = 0; x < createOnStart; x++)
        {
            CreateNewObject();
        }
    }
    public GameObject CreateNewObject(){

        GameObject obj = Instantiate(objprefab);
        obj.SetActive(false);
        poolObjs.Add(obj);

        return obj;
    }

    public GameObject GetObject()
    {
        GameObject obj = poolObjs.Find(x => x.activeInHierarchy == false);
       
        if (obj == null)
        {
            
            obj = CreateNewObject();
        }

        obj.SetActive(true);
       
        return obj;

    }
}
