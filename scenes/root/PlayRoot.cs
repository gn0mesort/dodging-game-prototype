using Godot;
using System.Diagnostics;

public class PlayRoot : Spatial {
  private GameMain _main = null;
  private Player _player = null;
  private FollowCamera _camera = null;
  private Level _level = null;
  private Timer _leadIn = null;
  private Timer _leadOut = null;
  private ulong _startTime = 0;
  private bool _exited = false;

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
    _exited = false;
    _level = GD.Load<PackedScene>(path).Instance() as Level;
    AddChild(_level);
    _player.Translation = _level.Entrance();
    // Lock motion during lead
    _player.LockMotion();
    _player.Visible = true;
    _camera.Visible = true;
    GD.Print($"Level {_main.Player.Progress} Start");
    _leadIn.Start();
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
  }

  private void _OnPlayerDied() {
    ++_main.Player.Deaths;
    _UpdatePlayTime();
    GD.Print("Player died.");
    _player.Initialize();
    _StartLevel();
  }

  private void _OnPlayerDamaged(uint remaining) {
    ++_main.Player.Collisions;
    GD.Print("Player damaged");
  }

  private void _OnLeadInTimeout() {
    _startTime = OS.GetTicksMsec();
    _player.UnlockMotion();
  }

  private void _UpdatePlayTime() {
    var time = OS.GetTicksMsec() - _startTime;
    GD.Print($"Run time: {time}ms");
    _main.Player.PlayTime += time / 1000;
  }

  private void _OnLeadOutTimeout() {
    // We're not going to have 2^16 levels hopefully
    var oldProgress = _main.Player.Progress;
    _main.Player.Progress = (ushort) Utility.Clamp(_main.Player.Progress + 1, 0, Levels.Length - 1);
    _main.StorePlayerData();
    if (LevelOverride != null && LevelOverride.Length > 0)
    {
      EmitSignal("TransitionRoot", RootScenes.Menu);
      return;
    }
    else if (oldProgress == _main.Player.Progress)
    {
      EmitSignal("TransitionRoot", RootScenes.GameComplete);
      return;
    }
    _StartLevel();
  }

  public override void _Ready() {
    _player = GetNode<Player>("Player");
    _player.Connect("Died", this, "_OnPlayerDied");
    _player.Connect("HealthDamaged", this, "_OnPlayerDamaged");
    _player.Connect("ShieldDamaged", this, "_OnPlayerDamaged");
    _camera = GetNode<FollowCamera>("FollowCamera");
    _leadIn = GetNode<Timer>("LeadIn");
    _leadIn.Connect("timeout", this, "_OnLeadInTimeout");
    _leadOut = GetNode<Timer>("LeadOut");
    _leadOut.Connect("timeout", this, "_OnLeadOutTimeout");
    _StartLevel();
  }

  private void _EndLevel() {
    _exited = true;
    _player.LockMotion();
    GD.Print($"Level {_main.Player.Progress} End");
    _UpdatePlayTime();
    _leadOut.Start();
  }

  public override void _PhysicsProcess(float delta) {
    // Less than is farther forward in this context
    if (_level != null && !_exited && _player.Translation.z < _level.Exit().z)
    {
      _EndLevel();
    }
  }
}
