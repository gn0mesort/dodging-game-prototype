using Godot;

public class PlayRoot : Spatial {
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private GameMain _main = null;

  public override void _EnterTree() {
    _main = GetNode<GameMain>("/root/Main");
    Connect("TransitionRoot", _main, "TransitionRoot");
  }

  public override void _Ready() {
  }


}
