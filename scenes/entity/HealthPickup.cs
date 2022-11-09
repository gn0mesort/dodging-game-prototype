using Godot;

public class HealthPickup : Pickup {
  public override void ApplyEffect(Player player) {
    player.RestoreHealth(Bonus);
  }

  public override void _Ready() {
    GetNode<AnimationPlayer>("AnimationPlayer").Play("Rotate");
  }
}
