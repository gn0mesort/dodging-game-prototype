using Godot;

public class MenuRoot : Node {
  private Node _child = null;
  private PackedScene _initial = null;
  private PackedScene _secondary = null;

  private void _OnTransition(GameState to) {
    // Exit is a weird case.
    // QueueFree will cause the interface to visibly unload before the window
    // actually closes.
    if (to == GameState.Exit)
    {
      OS.ExitCode = 0;
      GetTree().Notification(NotificationWmQuitRequest);
      return;
    }
    _child?.QueueFree();
    switch (to)
    {
    case GameState.InitialTitle:
      _child = _initial.Instance();
      break;
    case GameState.SecondaryTitle:
      _child = _secondary.Instance();
      break;
    }
    _child.Connect("Transition", this, "_OnTransition");
    AddChild(_child);
  }

  public override void _Ready() {
    // Load potential substates
    _initial = GD.Load<PackedScene>("res://scenes/prefab/menus/InitialTitle.tscn");
    _secondary = GD.Load<PackedScene>("res://scenes/prefab/menus/SecondaryTitle.tscn");
    // Transition to initial title screen
    _OnTransition(GameState.InitialTitle);
  }
}
