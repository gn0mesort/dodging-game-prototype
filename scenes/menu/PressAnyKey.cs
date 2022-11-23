using Godot;

/**
 * @brief A Control that emits a signal when any key, mouse button, or gamepad button is pressed.
 */
public class PressAnyKey : Label {
  /**
   * @brief Whether or not the text of the control should blink.
   */
  [Export]
  public bool Blinking { get; set; } = true;

  /**
   * @brief A Signal emitted when any key or button is pressed.
   */
  [Signal]
  public delegate void Pressed();

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var animations = GetNode<AnimationPlayer>("Animations");
    if (Blinking)
    {
      animations.Play("Blink");
    }
  }

  /**
   * @brief Handling for otherwise unhandled InputEvents.
   *
   * @param ev The event that triggered handling.
   */
  public override void _UnhandledInput(InputEvent ev) {
    if ((ev is InputEventKey || ev is InputEventMouseButton || ev is InputEventJoypadButton) && ev.IsPressed()
        && !ev.IsEcho())
    {
      GetTree().SetInputAsHandled();
      EmitSignal("Pressed");
    }
  }
}
