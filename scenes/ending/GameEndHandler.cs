using Godot;

/**
 * @brief Node for handling transitions out of the game ending scenes.
 */
public class GameEndHandler : Node {
  /**
   * @brief The Target node that will trigger the transition.
   */
  [Export]
  public NodePath Target { get; set; } = "";

  /**
   * @brief The name of the Signal in the Target node that will trigger the transition.
   */
  [Export]
  public string Trigger { get; set; } = "";

  /**
   * @brief The root scene to transition to when triggered.
   */
  [Export]
  public RootScenes NextScene { get; set; } = RootScenes.Menu;

  /**
   * @brief A Signal emitted when this node requests a transition of the root scene.
   */
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  private Main _main = null;

  private void _OnTrigger() {
    EmitSignal("TransitionRoot", NextScene);
  }

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _main = GetNode<Main>("/root/Main");
    Connect("TransitionRoot", _main, "TransitionRoot");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    GetNode(Target).Connect(Trigger, this, "_OnTrigger");
  }
}
