using Godot;
using System;

public class Obstacle : KinematicBody, ICollidable {
  [Export]
  public Level.LevelEntity.EntityMode Mode { get; set; } = Level.LevelEntity.EntityMode.Stationary;

  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(6f, 6f, 6f);

  private Vector3 _LEFT = new Vector3(-1f, 0f, 0f);
  private Vector3 _UP = new Vector3(0f, 1f, 0f);
  private Vector3 _RIGHT = new Vector3(1f, 0f, 0f);
  private Vector3 _DOWN = new Vector3(0f, -1f, 0f);

  private Vector3 _direction = new Vector3();
  private Level.LevelEntity.Direction _directionX = Level.LevelEntity.Direction.None;
  private Level.LevelEntity.Direction _directionY = Level.LevelEntity.Direction.None;

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
    _directionX = x;
    _directionY = y;
    if (x != Level.LevelEntity.Direction.None)
    {
      _direction += _DirectionToVector(x);
    }
    if (y != Level.LevelEntity.Direction.None)
    {
      _direction += _DirectionToVector(y);
    }
  }

  public void UpdateMovementExtents(Vector3 extents) {
    var transNoZ = Translation * new Vector3(1f, 1f, 0f);
    _LEFT = transNoZ + (new Vector3(-1f, 0f, 0f) * extents);
    _UP = transNoZ + (new Vector3(0f, 1f, 0f) * extents);
    _RIGHT = transNoZ + (new Vector3(1f, 0f, 0f) * extents);
    _DOWN = transNoZ + (new Vector3(0f, -1f, 0f) * extents);
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
      const Level.LevelEntity.Direction noDir = Level.LevelEntity.Direction.None;
      var x = _directionX == noDir ? 1f : -1f;
      var y = _directionY == noDir ? 1f : -1f;
      var flip = new Vector3(x, y, 0f);
      _direction *= flip;
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

  public void HandleCollision(KinematicCollision collision) {
    if (collision != null)
    {
      if (collision.Collider is Player)
      {
        var player = (collision.Collider as Player);
        player.TakeDamage();
        QueueFree();
      }
    }
  }
}
