public class GetTreated : GAction {

    public override bool PrePerform() {

 
        target = inventory.FindItemWithTag("table");
        // Check that we did indeed get a table
        if (target == null)
         
            return false;
      
        return true;
    }

    public override bool PostPerform() {

        // Add a new state 
        GWorld.Instance.GetWorld().ModifyState("Treated", 1);
  
        beliefs.ModifyState("isCured", 1);
    
        inventory.RemoveItem(target);
        return true;
    }
}
