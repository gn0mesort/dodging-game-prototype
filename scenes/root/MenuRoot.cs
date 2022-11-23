using Godot;

/**
 * @brief Behavior script for the root menu scene.
 */
public class MenuRoot : Node {
  /**
   * @brief An enumeration of submenus.
   */
  public enum MenuScenes {
    Primary,
    Secondary,
    Settings,
    Rebirth
  }

  private Main _main = null;
  private Node _child = null;
  private PackedScene _primary = null;
  private PackedScene _secondary = null;
  private PackedScene _settings = null;
  private PackedScene _rebirth = null;

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
    case MenuScenes.Settings:
      _child = _settings.Instance();
      break;
    case MenuScenes.Rebirth:
      _child = _rebirth.Instance();
      break;
    }
    _child.Connect("Transition", this, "_OnTransition");
    _child.Connect("TransitionRoot", _main, "TransitionRoot");
    AddChild(_child);
  }

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _main = GetNode<Main>("/root/Main");
    // Load potential substates
    _primary = GD.Load<PackedScene>("res://scenes/menu/PrimaryTitle.tscn");
    _secondary = GD.Load<PackedScene>("res://scenes/menu/SecondaryTitle.tscn");
    _settings = GD.Load<PackedScene>("res://scenes/menu/Settings.tscn");
    _rebirth = GD.Load<PackedScene>("res://scenes/menu/Rebirth.tscn");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    // If we're coming from a cold start then display Title A. Otherwise, display Title B.
    if (_main.PreviousScene() == RootScenes.Exit)
    {
      _OnTransition(MenuScenes.Primary);
    }
    else
    {
      _OnTransition(MenuScenes.Secondary);
    }
  }
}
