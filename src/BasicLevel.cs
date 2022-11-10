using Godot;

public class BasicLevel : Level {
  private Position3D _entrance = null;
  private Position3D _exit = null;

  [Export]
  public string Name { get; set; } = "";

  [Export]
  public NodePath EntrancePosition { get; set; } = null;

  [Export]
  public NodePath ExitPosition { get; set; } = null;

  public override Vector3 Entrance() {
    return _entrance.Translation;
  }

  public override Vector3 Exit() {
    return _exit.Translation;
  }

  public override void _Ready() {
    _entrance = GetNode<Position3D>(EntrancePosition);
    _exit = GetNode<Position3D>(ExitPosition);
  }
}
