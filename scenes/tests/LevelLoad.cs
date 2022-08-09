using Godot;
using System;

public class LevelLoad : Spatial {
  [Export]
  public string TargetLevel { get; set; } = "";

  private Level _level = null;
  private readonly PackedScene[] _obstacles = new PackedScene[100];
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
    // TODO: make an Entity class for these.
    var entity = _level.Entities[id - 1].Scene.Instance() as Spatial;
    entity.Translation = position;
    GD.Print(entity.Translation);
    CallDeferred("add_child", entity);
  }

  public void InitializeLevel() {
    _player.Translation = new Vector3();
    var step = _player.BaseSpeed.z;
    GD.Print(step);
    var grid = _player.MovementExtents + new Vector3(0f, 0f, 1f);
    GD.Print(grid);
    var depth = 3f * step;

    // There is probably a much more efficient way to do this!
    foreach (var list in _level.Data)
    {
      GD.Print(depth);
      AddObstacle(list[0], new Vector3(-1f, 1f, depth) * grid);
      AddObstacle(list[1], new Vector3(0f, 1f, depth) * grid);
      AddObstacle(list[2], new Vector3(1f, 1f, depth) * grid);
      AddObstacle(list[3], new Vector3(-1f, 0f, depth) * grid);
      AddObstacle(list[4], new Vector3(0f, 0f, depth) * grid);
      AddObstacle(list[5], new Vector3(1f, 0f, depth) * grid);
      AddObstacle(list[6], new Vector3(-1f, -1f, depth) * grid);
      AddObstacle(list[7], new Vector3(0f, -1f, depth) * grid);
      AddObstacle(list[8], new Vector3(1f, -1f, depth) * grid);
      depth += step;
    }
  }

  public override void _Ready() {
    _player = GetNode<Player>("Player");
    _level = LoadLevel(TargetLevel);
    GD.Print(_level);
    InitializeLevel();
  }
}
