using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyclone : MonoBehaviour
{
    public List<GameObject> points;
    public GameObject enemy;
    public float waitTime;
    public GameObject enemyClone;//Clone an enemy and place it under an empty object

    private void Start()
    {
        StartCoroutine(Clone());
    }

    IEnumerator Clone()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            //ramdom point
            GameObject e = Instantiate(enemy.gameObject, points[Random.Range(0, points.Count )].transform.position, Quaternion.identity);
            e.transform.SetParent(enemyClone.transform);
      
            
        }
    }
}
