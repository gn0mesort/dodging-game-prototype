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
    // This is just a stub for testing.
    var rand = new RandomNumberGenerator();
    rand.Randomize();
    EmitSignal("TransitionRoot", rand.RandiRange(0, 1) == 0 ? RootScenes.GameComplete : RootScenes.GameOver);
  }


}
