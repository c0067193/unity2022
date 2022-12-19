using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    public Button returnbutton;
    // Start is called before the first frame update
    void Start()
    {
        returnbutton.onClick.AddListener(SwitchScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SwitchScene()
    {
        SceneManager.LoadScene(0);
    }
}
