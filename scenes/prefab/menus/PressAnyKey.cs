using Godot;

public class PressAnyKey : Label {
  [Signal]
  public delegate void Pressed();

  public override void _Ready() {
    var animations = GetNode<AnimationPlayer>("Animations");
    animations.Play("Blink");
  }

  public override void _UnhandledInput(InputEvent ev) {
    if (ev is InputEventKey || ev is InputEventMouseButton || ev is InputEventJoypadButton)
    {
      GetTree().SetInputAsHandled();
      EmitSignal("Pressed");
    }
  }
}
