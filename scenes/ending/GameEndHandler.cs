using Godot;

public class GameEndHandler : Node {
  [Export]
  public NodePath Target { get; set; } = "";

  [Export]
  public string Trigger { get; set; } = "";

  [Export]
  public RootScenes NextScene { get; set; } = RootScenes.Menu;

  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private GameMain _main = null;

  private void _OnTrigger() {
    EmitSignal("TransitionRoot", NextScene);
  }

  public override void _EnterTree() {
    _main = GetNode<GameMain>("/root/Main");
    Connect("TransitionRoot", _main, "TransitionRoot");
  }

  public override void _Ready() {
    GetNode(Target).Connect(Trigger, this, "_OnTrigger");
  }
}
