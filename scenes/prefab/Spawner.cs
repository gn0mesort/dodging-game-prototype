using Godot;
using System;

public class Spawner : Node {
  [Export]
  public PackedScene SpawnedObject { get; set; } = null;

  [Export]
  public NodePath Target { get; set; } = "";
  private Player _target = null;
  private Vector3[] _positions = new Vector3[9];

  private RandomNumberGenerator _rand = new RandomNumberGenerator();

  private ulong _spawns = 0;

  private void _OnTimeout() {
    var position = (_target.Translation * new Vector3(0f, 0f, 1f));
    position += new Vector3(0f, 0f, 10 * _target.BaseSpeed.z);
    var count = _rand.RandiRange(0, 8);
    for (int i = 0; i < count; ++i, ++_spawns);
    {
      var selected = _rand.RandiRange(0, 8);
      var obj = SpawnedObject.Instance() as Obstacle;
      obj.Target = Target;
      obj.Translation = _positions[selected] + position;
      GetParent<Node>().CallDeferred("add_child", obj);
      // GD.Print($"Spawn count = {_spawns}, Node count = {GetParent().GetChildCount()}");
    }
  }

  public override void _Ready() {
    if (Target == "" || SpawnedObject == null)
    {
      return;
    }

    _rand.Randomize();
    _target = GetNode<Player>(Target);
    _positions[0] = new Vector3(-1f, 1f, 0f) * _target.MovementExtents;
    _positions[1] = new Vector3(0f, 1f, 0f) * _target.MovementExtents;
    _positions[2] = new Vector3(1f, 1f, 0f) * _target.MovementExtents;
    _positions[3] = new Vector3(-1f, 0f, 0f) * _target.MovementExtents;
    _positions[4] = new Vector3(0f, 0f, 0f);
    _positions[5] = new Vector3(1f, 0f, 0f) * _target.MovementExtents;
    _positions[6] = new Vector3(-1f, -1f, 0f) * _target.MovementExtents;
    _positions[7] = new Vector3(0f, -1f, 0f) * _target.MovementExtents;
    _positions[8] = new Vector3(1f, -1f, 0f) * _target.MovementExtents;
    var timer = GetNode<Timer>("Timer");
    timer.Connect("timeout", this, "_OnTimeout");
  }
}
