using Godot;
using System;

public class Movement : GridMap {
  private KinematicBody _player = null;
  private Camera _camera = null;

  public override void _Ready() {
    _player = GetNode<KinematicBody>("Player");
    _camera = GetNode<Camera>("Camera");
    _player.Translation = MapToWorld(0, 0, 0);
    _camera.Translation = MapToWorld(0, 0, 5);
  }
}
