using Godot;

public interface IEntity {
  void SetMode(Level.LevelEntity.EntityMode mode);
  void SetDirection(Level.LevelEntity.Direction x, Level.LevelEntity.Direction y);
  void SetScaling(bool x, bool y);
  void UpdateMovementExtents(Vector3 extents);
}
