using Godot;

/**
 * @brief Abstract base class for in game pickup items (e.g., health).
 */
public abstract class Pickup : StaticBody {
  /**
   * @brief The bonus that will be applied by the Pickup.
   */
  [Export]
  public uint Bonus { get; set; } = 0;

  /**
   * @brief Applies the Pickup's effect to the Player.
   *
   * @param player The Player to apply the effect to.
   */
  public abstract void ApplyEffect(Player player);
}
