using Godot;
using System;

public class LevelLoad : Spatial {
  [Export]
  public string TargetLevel { get; set; } = "";

  private Level _level = null;
  private readonly PackedScene[] _obstacles = new PackedScene[1];
  private Player _player = null;

  public static Level LoadLevel(string path) {
    var reader = new File();
    reader.Open(path, File.ModeFlags.Read);
    var json = reader.GetAsText();
    reader.Close();
    return Level.fromJson(json);
  }

  public void AddObstacle(int id, Vector3 position) {
    if (id == 0)
    {
      return;
    }
    var entity = _obstacles[id - 1].Instance() as Obstacle;
    entity.Translation = position;
    entity.Target = _player.GetPath();
    CallDeferred("add_child", entity);
  }

  public void InitializeLevel() {
    _player.Translation = new Vector3(0, 0, 0);
    var position = new Vector3(0, 0, -5f);
    // There is probably a much more efficient way to do this!
    for (var i = 0; i < _level.Data.TopLayer.Length; i += 3)
    {
      AddObstacle(_level.Data.TopLayer[i + 0], position + (new Vector3(-1f, 1f, 0f) * _player.MovementExtents));
      AddObstacle(_level.Data.TopLayer[i + 1], position + (new Vector3(0f, 1f, 0f) * _player.MovementExtents));
      AddObstacle(_level.Data.TopLayer[i + 2], position + (new Vector3(1f, 1f, 0f) * _player.MovementExtents));
      position += new Vector3(0, 0, -1f);
    }
    position = new Vector3(0, 0, -5f);
    for (var i = 0; i < _level.Data.MiddleLayer.Length; i += 3)
    {
      AddObstacle(_level.Data.MiddleLayer[i + 0], position + (new Vector3(-1f, 0f, 0f) * _player.MovementExtents));
      AddObstacle(_level.Data.MiddleLayer[i + 1], position + (new Vector3(0f, 0f, 0f) * _player.MovementExtents));
      AddObstacle(_level.Data.MiddleLayer[i + 2], position + (new Vector3(1f, 0f, 0f) * _player.MovementExtents));
      position += new Vector3(0, 0, -1f);
    }
    position = new Vector3(0, 0, -5f);
    for (var i = 0; i < _level.Data.BottomLayer.Length; i += 3)
    {
      AddObstacle(_level.Data.BottomLayer[i + 0], position + (new Vector3(-1f, -1f, 0f) * _player.MovementExtents));
      AddObstacle(_level.Data.BottomLayer[i + 1], position + (new Vector3(0f, -1f, 0f) * _player.MovementExtents));
      AddObstacle(_level.Data.BottomLayer[i + 2], position + (new Vector3(1f, -1f, 0f) * _player.MovementExtents));
      position += new Vector3(0, 0, -1f);
    }
  }

  public override void _Ready() {
    _obstacles[0] = GD.Load<PackedScene>("res://scenes/prefab/Obstacle.tscn");
    _player = GetNode<Player>("Player");
    _level = LoadLevel(TargetLevel);
    InitializeLevel();
  }
}
