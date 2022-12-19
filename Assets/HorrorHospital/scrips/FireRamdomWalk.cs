using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class FireRamdomWalk : MonoBehaviour
{
    //walk speed and roated speed
    public float movespeed;
    public float rotatespeed;

    public int key = 0;

    public bool temp = true;

    private void Start()
    {
        key = 1;

        //StartCoroutine("wait");
    }
    private void Update()
    {
        if(temp  == false)
        {
            return;
        }
        //start walk
        switch (key)
        {
            case 1:
                //forward
                transform.Translate(0, 0, 1 * movespeed * Time.deltaTime, Space.Self);
                //rotate 1
                transform.Rotate(0, 1 * rotatespeed * Time.deltaTime, 0, Space.Self);
                break;
            case 2:
                transform.Translate(0, 0, 1 * movespeed * Time.deltaTime, Space.Self);
                transform.Rotate(0, 1 * rotatespeed * Time.deltaTime, 0, Space.Self);
                break;
            case 3:
                transform.Translate(0, 0, 1 * movespeed * Time.deltaTime, Space.Self);
                transform.Rotate(0, 1 * rotatespeed * Time.deltaTime, 0, Space.Self);
                break;
            case 4:
                transform.Translate(0, 0, 1 * movespeed * Time.deltaTime, Space.Self);
                transform.Rotate(0, 1 * rotatespeed * Time.deltaTime, 0, Space.Self);
                break;
        }
    }
    IEnumerator Wait()
    {
        //run  Timer for two seconds
        while (true)
        {
            yield return new WaitForSeconds(2);
            Timer();

        }
    }

    void Timer()
    {
        //make ramdom1-3
        int i = Random.Range(0, 4);
        //the probably of walk is 3/2
        if (i > 1)
        {
            temp = true;
            //rotate back 180
            transform.Rotate(0, 180, 0, Space.Self);
            return;
        }
        else
        {
            temp = false;
        }
        //change walk
        key++;
        if(key == 5)
        {
            key = 1;
        }

    }
}
