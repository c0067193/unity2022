using System.Collections.Generic;
using UnityEngine;

public class GInventory {
   
    public List<GameObject> items = new List<GameObject>();

 
    public void AddItem(GameObject i) {

        items.Add(i);
    }

    // Method to search for a particular item
    public GameObject FindItemWithTag(string tag) {

      
        foreach (GameObject i in items) {

            // Found a match
            if (i.tag == tag) {

                return i;
            }
        }
       
        return null;
    }

   
    public void RemoveItem(GameObject i) {

        int indexToRemove = -1;

        // Search through the list to see if it exists
        foreach (GameObject g in items) {

       
            indexToRemove++;
         
            if (g == i) {

                break;
            }
        }

        if (indexToRemove >= 1) {

            //  remove the item at indexToRemove
            items.RemoveAt(indexToRemove);
        }
    }
}
