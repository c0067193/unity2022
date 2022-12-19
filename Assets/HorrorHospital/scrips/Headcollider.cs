using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headcollider : MonoBehaviour
{
    public Enemy2 enemy2;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeHeadDamage(int damageToTake)
    {
        enemy2.health -= damageToTake * 5;


        
    }
}
