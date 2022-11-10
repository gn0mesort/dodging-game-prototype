using Godot;

public class PauseScreen : Node {
  private PackedScene _pauseMenuScene = null;
  private PackedScene _settingsScene = null;
  private Node _pauseMenu = null;
  private Node _settings = null;

  [Signal]
  public delegate void RequestResume();

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

  public override void _Ready() {
    _InitializePauseMenu();
  }
}
