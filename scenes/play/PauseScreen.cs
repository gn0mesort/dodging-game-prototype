using Godot;

/**
 * @brief Behavior script for the pause screen.
 */
public class PauseScreen : Node {
  private PackedScene _pauseMenuScene = null;
  private PackedScene _settingsScene = null;
  private Node _pauseMenu = null;
  private Node _settings = null;

  /**
   * @brief A Signal emitted to request the game resume.
   */
  [Signal]
  public delegate void RequestResume();

  /**
   * @brief A Signal emitted to request the level restart.
   */
  [Signal]
  public delegate void RequestRestartLevel();

  private void _OnRequestResume() {
    EmitSignal("RequestResume");
  }

  private void _OnRequestRestartLevel() {
    EmitSignal("RequestRestartLevel");
  }

  private void _OnRequestSettings() {
    _InitializeSettings();
  }

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _pauseMenuScene = GD.Load<PackedScene>("res://scenes/menu/PauseMenu.tscn");
    _settingsScene = GD.Load<PackedScene>("res://scenes/menu/Settings.tscn");
  }

  private void _OnRequestBack() {
    _InitializePauseMenu();
  }

  private void _InitializeSettings() {
    _pauseMenu?.QueueFree();
    _pauseMenu = null;
    _settings = _settingsScene.Instance();
    AddChild(_settings);
    _settings.Connect("RequestBack", this, "_OnRequestBack");

  }

  private void _InitializePauseMenu() {
    _settings?.QueueFree();
    _settings = null;
    _pauseMenu = _pauseMenuScene.Instance();
    AddChild(_pauseMenu);
    _pauseMenu.Connect("RequestResume", this, "_OnRequestResume");
    _pauseMenu.Connect("RequestRestartLevel", this, "_OnRequestRestartLevel");
    _pauseMenu.Connect("RequestSettings", this, "_OnRequestSettings");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _InitializePauseMenu();
  }
}
