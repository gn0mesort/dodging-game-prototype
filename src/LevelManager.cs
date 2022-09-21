using Godot;
using System;

public class LevelManager {
  private Node _levelRoot = null;
  private Player _player = null;

  public Level Current { get; private set; } = null;
  public Vector3 ExitDepth { get; private set; } = new Vector3();

  public LevelManager(Node levelRoot, Player player) {
    _levelRoot = levelRoot;
    // _levelRoot must not be null
    _player = player;
    // _player must not be null
  }

  private Level _ReadLevel(string path) {
    var reader = new File();
    reader.Open(path, File.ModeFlags.Read);
    var json = reader.GetAsText();
    reader.Close();
    return Level.fromJson(json);
  }

  private void _AddObstacle(int id, Vector3 position) {
    if (id == 0)
    {
      return;
    }
    var spatial = Current.Scenes[Current.Entities[id - 1].ScenePath].Instance() as Spatial;
    spatial.Translation = position;
    if (spatial is IEntity)
    {
      var entity = spatial as IEntity;
      entity.SetMode(Current.Entities[id - 1].Mode);
      entity.UpdateMovementExtents(_player.MovementExtents);
      entity.SetScaling(Current.Entities[id - 1].ScaleX, Current.Entities[id - 1].ScaleY);
      entity.SetDirection(Current.Entities[id - 1].DirectionX, Current.Entities[id - 1].DirectionY);
    }
    _levelRoot.CallDeferred("add_child", spatial);
  }

  private void _InitializeLevel() {
    _player.Translation = new Vector3();
    const float step = -6f;
    var grid = _player.MovementExtents + new Vector3(0f, 0f, 1f);
    var depth = 3f * step;

    // There is probably a much more efficient way to do this!
    foreach (var list in Current.Data)
    {
      _AddObstacle(list[0], new Vector3(-1f, 1f, depth) * grid);
      _AddObstacle(list[1], new Vector3(0f, 1f, depth) * grid);
      _AddObstacle(list[2], new Vector3(1f, 1f, depth) * grid);
      _AddObstacle(list[3], new Vector3(-1f, 0f, depth) * grid);
      _AddObstacle(list[4], new Vector3(0f, 0f, depth) * grid);
      _AddObstacle(list[5], new Vector3(1f, 0f, depth) * grid);
      _AddObstacle(list[6], new Vector3(-1f, -1f, depth) * grid);
      _AddObstacle(list[7], new Vector3(0f, -1f, depth) * grid);
      _AddObstacle(list[8], new Vector3(1f, -1f, depth) * grid);
      depth += step;
    }
    ExitDepth = new Vector3(0f, 0f, depth) * grid;
    GD.Print($"Exit Depth: {ExitDepth}");
  }

  public void LoadLevel(string level) {
    Current = _ReadLevel(level);
    _InitializeLevel();
  }
}
