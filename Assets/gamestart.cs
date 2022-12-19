using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamestart : MonoBehaviour
{
    public Button horrorhospital;
    public Button hospital;
    public Button GobangChess;
    // Start is called before the first frame update
    void Start()
    {
        horrorhospital.onClick.AddListener(SwitchhorrorScene);
        hospital.onClick.AddListener(SwitchhospitalScene);
        GobangChess.onClick.AddListener(SwitchchessScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SwitchhorrorScene()
    {
        SceneManager.LoadScene(1);
    }
    void SwitchhospitalScene()
    {
        SceneManager.LoadScene(2);
    }
    void SwitchchessScene()
    {
        SceneManager.LoadScene(3);
    }
}
