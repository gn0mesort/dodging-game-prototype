using Godot;

/**
 * @brief Behavior script for HealthPickup entities.
 */
public class HealthPickup : Pickup {
  /**
   * @brief Applies the Pickup's effect to the Player.
   *
   * @param player The Player to apply the effect to.
   */
  public override void ApplyEffect(Player player) {
    player.RestoreHealth(Bonus);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    GetNode<AnimationPlayer>("AnimationPlayer").Play("Rotate");
  }
}
