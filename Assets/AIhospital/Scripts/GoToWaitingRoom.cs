public class GoToWaitingRoom : GAction {
    public override bool PrePerform() {

        return true;
    }

    public override bool PostPerform() {

 
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);
       
        GWorld.Instance.AddCustomer(this.gameObject);
        // Inject a state into the agents beliefs
        beliefs.ModifyState("atHospital", 1);

        return true;
    }
}
