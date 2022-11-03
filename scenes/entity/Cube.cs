using Godot;

public class Cube : KinematicBody {
  public enum Axis {
    None = 0,
    X,
    Y
  }

  [Export]
  public Axis MovementAxis { get; set; } = Axis.None;
}
