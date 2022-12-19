using System.Collections.Generic;


[System.Serializable]
public class WorldState {

    public string key;
    public int value;
}

public class WorldStates {

    // Constructor
    public Dictionary<string, int> states;

    public WorldStates() {

        states = new Dictionary<string, int>();
    }

   
    // Check  key
    public bool HasState(string key) {

        return states.ContainsKey(key);
    }

    // Add  dictionary
    private void AddState(string key, int value) {

        states.Add(key, value);
    }

    public void ModifyState(string key, int value) {

      
        if (HasState(key)) {

            // Add the value 
            states[key] += value;
            // If it's less than zero then remove it
            if (states[key] <= 0) {

            
                RemoveState(key);
            }
        } else {

            AddState(key, value);
        }
    }

    // Method to remove
    public void RemoveState(string key) {

        // Check if it frist exists
        if (HasState(key)) {

            states.Remove(key);
        }
    }

    // Set a state
    public void SetState(string key, int value) {

        // Check if it exists
        if (HasState(key)) {

            states[key] = value;
        } else {

            AddState(key, value);
        }
    }

    public Dictionary<string, int> GetStates() {

        return states;
    }
}
