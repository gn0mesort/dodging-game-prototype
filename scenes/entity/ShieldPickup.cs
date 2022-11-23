using Godot;

/**
 * @brief Behavior script for ShieldPickup entities.
 */
public class ShieldPickup : Pickup {

  /**
   * @brief Applies the Pickup's effect to the Player.
   *
   * @param player The Player to apply the effect to.
   */
  public override void ApplyEffect(Player player) {
    player.RestoreShield(Bonus);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    GetNode<AnimationPlayer>("AnimationPlayer").Play("Rotate");
  }
}
