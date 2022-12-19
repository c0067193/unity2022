using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour {


    public Text states;

    void LateUpdate() {

    
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
        // Clear out the states text
        states.text = "";
      
        foreach (KeyValuePair<string, int> s in worldStates) {

            states.text += s.Key + ", " + s.Value + "\n";
        }
    }
}
