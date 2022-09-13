using Godot;
using System;

public interface ICollidable {
  void HandleCollision(KinematicCollision collision);
}
