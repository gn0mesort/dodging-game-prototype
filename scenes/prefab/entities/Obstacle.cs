using Godot;
using System;

public class Obstacle : KinematicBody {
  [Export]
  public Level.LevelEntity.EntityMode Mode { get; set; } = Level.LevelEntity.EntityMode.Stationary;

  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(6f, 6f, 6f);

  private Vector3 _LEFT = new Vector3(-1f, 0f, 0f);
  private Vector3 _UP = new Vector3(0f, 1f, 0f);
  private Vector3 _RIGHT = new Vector3(1f, 0f, 0f);
  private Vector3 _DOWN = new Vector3(0f, -1f, 0f);

  private Vector3 _direction = new Vector3();

  private Vector3 _DirectionToVector(Level.LevelEntity.Direction direction) {
    switch (direction)
    {
    case Level.LevelEntity.Direction.Left:
      return _LEFT;
    case Level.LevelEntity.Direction.Right:
      return _RIGHT;
    case Level.LevelEntity.Direction.Up:
      return _UP;
    case Level.LevelEntity.Direction.Down:
      return _DOWN;
    default:
      return new Vector3();
    }
  }

  public void SetDirection(Level.LevelEntity.Direction x, Level.LevelEntity.Direction y) {
    _direction += _DirectionToVector(x);
    _direction += _DirectionToVector(y);
    if (Mode == Level.LevelEntity.EntityMode.Translating)
    {
      GD.Print($"{Name}: Initial Direction is {_direction}");
    }
  }

  public void UpdateMovementExtents(Vector3 extents) {
    var transNoZ = Translation * new Vector3(1f, 1f, 0f);
    _LEFT = transNoZ + (new Vector3(-1f, 0f, 0f) * extents);
    _UP = transNoZ + (new Vector3(0f, 1f, 0f) * extents);
    _RIGHT = transNoZ + (new Vector3(1f, 0f, 0f) * extents);
    _DOWN = transNoZ + (new Vector3(0f, -1f, 0f) * extents);
    if (Mode == Level.LevelEntity.EntityMode.Translating)
    {
      GD.Print($"{Name}: {Translation}, {_LEFT}, {_RIGHT}, {_UP}, {_DOWN}");
    }
  }

  private static bool _IsEqualApprox(Vector3 a, Vector3 b, float tolerance) {
    return Mathf.IsEqualApprox(a.x, b.x, tolerance) && Mathf.IsEqualApprox(a.y, b.y, tolerance) &&
           Mathf.IsEqualApprox(a.z, b.z, tolerance);
  }


  private void _ProcessTranslating(float delta) {
    var transNoZ = Translation * new Vector3(1f, 1f, 0f);
    if ((_direction.y == 0f && Mathf.IsEqualApprox(_direction.x, transNoZ.x, 0.5f)) ||
        (_direction.x == 0f && Mathf.IsEqualApprox(_direction.y, transNoZ.y, 0.5f)) ||
        _IsEqualApprox(_direction, transNoZ, 0.5f))
    {
      _direction *= -1f;
//    GD.Print($"{Name} = {_direction}, {Translation}");
    }
    var velocity = (_direction - transNoZ).Normalized();
    velocity *= BaseSpeed;
    var collision = MoveAndCollide(velocity * delta);
  }

  public override void _PhysicsProcess(float delta) {
    switch (Mode)
    {
    case Level.LevelEntity.EntityMode.Translating:
      _ProcessTranslating(delta);
      break;
    case Level.LevelEntity.EntityMode.Scaling:
      break;
    default:
      break;
    }
  }
}
