using Godot;

public abstract class Level : Spatial {
  [Export]
  public string LevelName { get; set; } = "";

  public abstract Vector3 Entrance();
  public abstract Vector3 Exit();
}
