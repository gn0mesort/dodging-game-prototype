using Godot;

public class PressAnyKey : Label {
  [Export]
  public bool Blinking { get; set; } = true;

  [Signal]
  public delegate void Pressed();

  public override void _Ready() {
    var animations = GetNode<AnimationPlayer>("Animations");
    if (Blinking)
    {
      animations.Play("Blink");
    }
  }

  public override void _UnhandledInput(InputEvent ev) {
    if ((ev is InputEventKey || ev is InputEventMouseButton || ev is InputEventJoypadButton) && ev.IsPressed()
        && !ev.IsEcho())
    {
      GetTree().SetInputAsHandled();
      EmitSignal("Pressed");
    }
  }
}
