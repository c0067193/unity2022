using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld {

   
    private static readonly GWorld instance = new GWorld();
    
    private static WorldStates world;
    
    private static Queue<GameObject> Customers;
    
    private static Queue<GameObject> tables;

    static GWorld() {

        // Create our world
        world = new WorldStates();

        Customers = new Queue<GameObject>();

        tables = new Queue<GameObject>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("table");

        foreach (GameObject c in cubes) {

            tables.Enqueue(c);
        }

        // Inform the state
        if (cubes.Length > 0) {
            world.ModifyState("Freetable", cubes.Length);
        }


        Time.timeScale = 5.0f;
    }

    private GWorld() {

    }

    // Add Customer
    public void AddCustomer(GameObject p) {


        Customers.Enqueue(p);
    }

  
    public GameObject RemoveCustomer() {

        if (Customers.Count == 0) return null;
        return Customers.Dequeue();
    }

    
    public void Addtable(GameObject p) {

        // Add the Customer to the Customers Queue
        tables.Enqueue(p);
    }

 
    public GameObject Removetable() {

        // Check we have something to remove
        if (tables.Count == 0) return null;
        return tables.Dequeue();
    }

    public static GWorld Instance {

        get { return instance; }
    }

    public WorldStates GetWorld() {

        return world;
    }
}
