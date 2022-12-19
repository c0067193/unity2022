using UnityEngine;

public class Spawn : MonoBehaviour {

   
    public GameObject CustomerPrefab;
    // Number of Customers to spawn
    public int numCustomers;

    void Start() {

       
        Invoke("SpawnCustomer", 5.0f);
    }

    void SpawnCustomer() {

      
        Instantiate(CustomerPrefab, this.transform.position, Quaternion.identity);
        // Invoke this method at random intervals
        Invoke("SpawnCustomer", Random.Range(2.0f, 10.0f));
    }

    // Update is called once per frame
    void Update() {

    }
}
