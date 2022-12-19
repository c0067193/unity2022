using UnityEngine;

public class GetCustomer : GAction {

    
    GameObject resource;

    public override bool PrePerform() {

   
        target = GWorld.Instance.RemoveCustomer();
      
        if (target == null)
          
            return false;
    
        resource = GWorld.Instance.Removetable();
   
        if (resource != null) {

            // Yes we have a table
            inventory.AddItem(resource);
        } else {

            // No free tables so release the Customer
            GWorld.Instance.AddCustomer(target);
            target = null;
            return false;
        }

        //take away one table being available from the world state
        GWorld.Instance.GetWorld().ModifyState("Freetable", -1);
        return true;
    }

    public override bool PostPerform() {

        
        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);
        if (target) {

            target.GetComponent<GAgent>().inventory.AddItem(resource);
        }
        return true;
    }
}
