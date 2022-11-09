using Godot;
using System.Diagnostics;

public class PlayRoot : Spatial {
  private GameMain _main = null;
  private Player _player = null;
  private FollowCamera _camera = null;
  private Level _level = null;
  private ulong _startTime = 0;

  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  [Export]
  public string LevelOverride { get; set; } = "";

  [Export]
  public string[] Levels { get; set; } = null;

  public override void _EnterTree() {
    _main = GetNode<GameMain>("/root/Main");
    Connect("TransitionRoot", _main, "TransitionRoot");
  }

  private void _InitializeLevel(string path) {
    if (_level != null)
    {
      _level.QueueFree();
    }
    _level = GD.Load<PackedScene>(path).Instance() as Level;
    AddChild(_level);
    _player.Translation = _level.Entrance();
    _player.Initialize();
    _player.Visible = true;
    _camera.Visible = true;
  }

  private void _StartLevel() {
    if (LevelOverride != null && LevelOverride.Length > 0)
    {
      _InitializeLevel(LevelOverride);
    }
    else
    {
      Debug.Assert(Levels.Length > _main.Player.Progress);
      var levelPath = Levels[_main.Player.Progress];
      Debug.Assert(levelPath != null);
      Debug.Assert(levelPath.Length > 0);
      _InitializeLevel(levelPath);
    }
    _startTime = OS.GetTicksMsec();
  }

  private void _OnPlayerDied() {
    ++_main.Player.Deaths;
    var time = OS.GetTicksMsec() - _startTime;
    GD.Print($"Run time: {time}ms");
    _main.Player.PlayTime += time / 1000;
    GD.Print("Player died.");
    _StartLevel();
  }

  private void _OnPlayerDamaged(uint remaining) {
    ++_main.Player.Collisions;
    GD.Print("Player damaged");
  }

  public override void _Ready() {
    _player = GetNode<Player>("Player");
    _player.Connect("Died", this, "_OnPlayerDied");
    _player.Connect("HealthDamaged", this, "_OnPlayerDamaged");
    _player.Connect("ShieldDamaged", this, "_OnPlayerDamaged");
    _camera = GetNode<FollowCamera>("FollowCamera");
    _StartLevel();
  }

}
