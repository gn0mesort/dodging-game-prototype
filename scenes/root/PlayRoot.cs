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
    _player.Initialize();
    // Lock motion during lead
    _player.LockMotion();
    _player.Visible = true;
    _camera.Visible = true;
    GD.Print("Level Start");
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

  private void _OnLeadInTimeout() {
    _player.UnlockMotion();
  }

  private void _OnLeadOutTimeout() {
    if (LevelOverride != null && LevelOverride.Length > 0)
    {
      EmitSignal("TransitionRoot", RootScenes.Menu);
    }
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
    GD.Print("Level End");
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
