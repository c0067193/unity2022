using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionmgr : MonoBehaviour
{
    // Start is called before the first frame update
    private static positionmgr instance;
    public static positionmgr Instance
    {
        get { return instance; }
    }
    [SerializeField]
    private Transform[] transforms;
    private void Awake()
    {
        instance = this;
        transforms = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            transforms[i] = transform.GetChild(i);
            transform.gameObject.SetActive(false);
        }

    }
    public Vector3 GetRandomPosition
    {
        get { return transforms[Random.Range(0, transforms.Length)].position; }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
