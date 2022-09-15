using Godot;
using System;

public class Play : Spatial, IDependsOnMain, IRequiresConfiguration {
  public class Configuration {
    public string LevelPath { get; private set; } = "";

    public Configuration(string levelPath) {
      // TODO: verify that the input path is a level path.
      LevelPath = levelPath;
    }

    public void Apply(Node target) {
      var actualTarget = target as Play;
      if (actualTarget == null)
      {
        return;
      }
      actualTarget.Levels.LoadLevel(LevelPath);
    }
  }

  private Main _main = null;
  private SceneManager.SceneConfigurationMethod _deferredConfig = null;
  private Player _player = null;
  private Timer _timer = null;
  private uint _timerTicks = 0;
  private bool _timerLeadIn = true;
  private PackedScene _pauseMenuScene = GD.Load<PackedScene>("res://scenes/prefab/Pause.tscn");

  public LevelManager Levels { get; private set; } = null;

  public void SetMainNode(Main main) {
    _main = main;
  }

  public Main GetMainNode() {
    return _main;
  }

  public void Configure(SceneManager.SceneConfigurationMethod config) {
    _deferredConfig = config;
  }

  private void _OnTimerTimeout() {
    if (_timerLeadIn && _timerTicks < 3)
    {
      ++_timerTicks;
    }
    else if (_timerLeadIn && _timerTicks >= 3)
    {
      _timer.Stop();
      _timerLeadIn = false;
      _timerTicks = 0;
      GD.Print("Start game");
      _player.SetDeferred("axis_lock_motion_z", false);
    }
    else if (!_timerLeadIn && _timerTicks < 3)
    {
      ++_timerTicks;
    }
    else if (!_timerLeadIn && _timerTicks >= 3)
    {
      _timer.Stop();
      _timerLeadIn = true;
      _timerTicks = 0;
      GD.Print("Stop game");
      _player.SetDeferred("axis_lock_motion_z", true);
      // In the future do something more intelligent.
      _main.Scenes.LoadScene("res://scenes/TitleMenu.tscn");
    }
  }

  private void _OnPlayerDied() {
    _main.Scenes.LoadScene("res://scenes/GameOver.tscn");
  }

  public override void _Ready() {

    _player = GetNode<Player>("Player");
    _timer = GetNode<Timer>("Timer");
    Levels = new LevelManager(this, _player);
    _deferredConfig(this);
    _player.Connect("Died", this, "_OnPlayerDied");
    _timer.Connect("timeout", this, "_OnTimerTimeout");
    _timer.CallDeferred("start");
  }

  public override void _PhysicsProcess(float delta) {
    if (!_timerLeadIn && (_player.Translation.z < Levels.ExitDepth.z) && _timer.IsStopped())
    {
      GD.Print($"Player Translation @ Exit: {_player.Translation}");
      _timer.CallDeferred("start");
    }
  }

  public override void _Input(InputEvent ev) {
    if (ev.IsActionPressed("pause"))
    {
      _main.Paused = true;
      var pauseMenu = _pauseMenuScene.Instance() as Pause;
      pauseMenu.SetMainNode(_main);
      CallDeferred("add_child", pauseMenu);
    }
  }
}
