using Godot;

public class MenuRoot : Node {
  public enum MenuScenes {
    Primary,
    Secondary,
    Settings,
    Rebirth
  }

  private Utility _utils = null;
  private Node _child = null;
  private PackedScene _primary = null;
  private PackedScene _secondary = null;
  private PackedScene _settings = null;
  private PackedScene _rebirth = null;

  private void _OnTransitionRoot(RootScenes to) {
    _utils.TransitionRoot(to);
  }

  private void _OnTransition(MenuScenes to) {
    _child?.QueueFree();
    switch (to)
    {
    case MenuScenes.Primary:
      _child = _primary.Instance();
      break;
    case MenuScenes.Secondary:
      _child = _secondary.Instance();
      break;
    }
    _child.Connect("Transition", this, "_OnTransition");
    _child.Connect("TransitionRoot", this, "_OnTransitionRoot");
    AddChild(_child);
  }

  public override void _Ready() {
    _utils = GetNode<Utility>("/root/Util");
    // Load potential substates
    _primary = GD.Load<PackedScene>("res://scenes/menu/PrimaryTitle.tscn");
    _secondary = GD.Load<PackedScene>("res://scenes/menu/SecondaryTitle.tscn");
    // Transition to initial title screen
    _OnTransition(MenuScenes.Primary);
  }
}
