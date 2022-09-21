using Godot;
using System;

public class Field : Area, IEntity {
 [Export]
  public Level.LevelEntity.EntityMode Mode { get; set; } = Level.LevelEntity.EntityMode.Stationary;

  [Export]
  public Vector3 MaxScale { get; set; } = new Vector3(2f, 2f, 1f);

  private bool _scaleX = false;
  private bool _scaleY = false;

  private Tween _tween = null;

  public void SetMode(Level.LevelEntity.EntityMode mode) {
    Mode = mode;
  }

  public void SetDirection(Level.LevelEntity.Direction x, Level.LevelEntity.Direction y) {
    return;
  }

  public void SetScaling(bool x, bool y) {
    _scaleX = x;
    _scaleY = y;
  }

  public void UpdateMovementExtents(Vector3 extents) {
    return;
  }

  private void _OnBodyEntered(Node body) {
    var player = body as Player;
    if (player != null)
    {
      player.SpeedMultiplier = 0.5f;
    }
  }

  private void _OnBodyExited(Node body) {
    var player = body as Player;
    if (player != null)
    {
      player.SpeedMultiplier = 1f;
    }
  }

  public override void _Ready() {
    _tween = GetNode<Tween>("Tween");
    Connect("body_entered", this, "_OnBodyEntered");
    Connect("body_exited", this, "_OnBodyExited");
    if (Mode == Level.LevelEntity.EntityMode.Translating)
    {
      Mode = Level.LevelEntity.EntityMode.Stationary;
    }
  }

    private void _ProcessScaling(float delta) {
    if (_tween.IsActive())
    {
      return;
    }
    var nextScale = new Vector3(1f, 1f, 1f);
    if (_scaleX && !Mathf.IsEqualApprox(Scale.x, MaxScale.x))
    {
      nextScale.x = MaxScale.y;
    }
    if (_scaleY && !Mathf.IsEqualApprox(Scale.y, MaxScale.y))
    {
      nextScale.y = MaxScale.y;
    }
    _tween.InterpolateProperty(this, "scale", Scale, nextScale, 3f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
    _tween.Start();
  }

  public override void _PhysicsProcess(float delta) {
    if (Mode == Level.LevelEntity.EntityMode.Scaling)
    {
      _ProcessScaling(delta);
    }
  }
}
