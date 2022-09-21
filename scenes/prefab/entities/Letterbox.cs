using Godot;
using System;

public class Letterbox : KinematicBody, IEntity {
  private AnimationPlayer _animations = null;
  private bool _isClosed = false;

  [Export]
  public Level.LevelEntity.EntityMode Mode { get; set; } = Level.LevelEntity.EntityMode.Stationary;

  public void SetMode(Level.LevelEntity.EntityMode mode) {
    Mode = mode;
  }

  public void SetDirection(Level.LevelEntity.Direction x, Level.LevelEntity.Direction y) {
    return;
  }

  public void SetScaling(bool x, bool y) {
    return;
  }

  public void UpdateMovementExtents(Vector3 extents) {
    return;
  }

  private void _OnAnimationFinished(string animation) {
    _isClosed = !_isClosed;
    if (_isClosed)
    {
      _animations.PlayBackwards(animation);
    }
    else
    {
      _animations.Play(animation);
    }
  }

  public override void _Ready() {
    _animations = GetNode<AnimationPlayer>("Animations");
    if (Mode == Level.LevelEntity.EntityMode.Translating)
    {
      Mode = Level.LevelEntity.EntityMode.Stationary;
    }
    _animations.Connect("animation_finished", this, "_OnAnimationFinished");
    _animations.Play("Close");
  }

}
