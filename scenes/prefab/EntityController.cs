using Godot;

public class EntityController : Node {
  private readonly Controller _controller = new Controller();

  private Player _target = null;

  [Export]
  public NodePath Target { get; set; } = "";

  public override void _Ready() {
    _target = GetNode<Player>(Target);
  }

  public override void _Input(InputEvent ev) {
    if (ev.IsEcho())
    {
      return;
    }
    if (ev is InputEventKey evKey)
    {
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_up"), Controller.Control.UP));
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_left"), Controller.Control.LEFT));
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_down"), Controller.Control.DOWN));
      _controller.SetControlIf(evKey.IsPressed(),
                               Controller.ToControlIf(evKey.IsAction("move_right"), Controller.Control.RIGHT));
    }
    _target.IntegrateControls(_controller);
  }

}
