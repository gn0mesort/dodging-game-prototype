using Godot;

public abstract class Level : Spatial {
  public abstract Vector3 Entrance();
  public abstract Vector3 Exit();
}
