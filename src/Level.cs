using Godot;

/**
 * @brief An abstract base class for all Level objects.
 *
 * This defines the minimum interface required by the Level loader.
 */
public abstract class Level : Spatial {
  /**
   * @brief The name of the level. This is used to identify the level.
   */
  [Export]
  public string LevelName { get; set; } = "";

  /**
   * @brief Retrieve's the 3-dimensional entrance position for the level.
   *
   * @return A Vector3 representing the position of the level entrance.
   */
  public abstract Vector3 Entrance();

  /**
   * @brief Retrieve's the 3-dimensional exit position for the level.
   *
   * @return A Vector3 representing the position of the level exit.
   */
  public abstract Vector3 Exit();
}
