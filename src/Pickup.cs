using Godot;

public abstract class Pickup : StaticBody {
  [Export]
  public uint Bonus { get; set; } = 0;

  public abstract void ApplyEffect(Player player);
}
