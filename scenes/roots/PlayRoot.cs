using Godot;
using System.Diagnostics;

public class PlayRoot : Spatial {
  private GameMain _main = null;
  private Player _player = null;
  private FollowCamera _camera = null;
  private Level _level = null;

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
    _player.Visible = true;
    _camera.Visible = true;
  }

  public override void _Ready() {
    _player = GetNode<Player>("Player");
    _camera = GetNode<FollowCamera>("FollowCamera");
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


}
