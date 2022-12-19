public class GoTotable : GAction {

    public override bool PrePerform() {

        
        target = inventory.FindItemWithTag("table");
        // Check that we did indeed get a table
        if (target == null)
        
            return false;
        // All good
        return true;
    }

    public override bool PostPerform() {

       
        GWorld.Instance.GetWorld().ModifyState("TreatingCustomer", 1);
        // Give back the table
        GWorld.Instance.Addtable(target);
     
        inventory.RemoveItem(target);

        GWorld.Instance.GetWorld().ModifyState("Freetable", 1);
        return true;
    }
}
