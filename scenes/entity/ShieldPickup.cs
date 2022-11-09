using Godot;

public class ShieldPickup : Pickup {
  public override void ApplyEffect(Player player) {
    player.RestoreShield(Bonus);
  }

  public override void _Ready() {
    GetNode<AnimationPlayer>("AnimationPlayer").Play("Rotate");
  }
}
