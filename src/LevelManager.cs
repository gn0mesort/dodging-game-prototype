///** Object for managing and loading levels.
// *
// * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
// *
// * This program is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Affero General Public License as published
// * by the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Affero General Public License for more details.
// *
// * You should have received a copy of the GNU Affero General Public License
// * along with this program.  If not, see <https://www.gnu.org/licenses/>.
// */
//using Godot;
//using System.Diagnostics;
//
//public class LevelManager {
//  private Node _levelRoot = null;
//  private Player _player = null;
//
//  /**
//   * The description of the current level.
//   */
//  public Level Current { get; private set; } = null;
//
//  /**
//   * The depth of the level exit as a 3D point.
//   */
//  public Vector3 ExitDepth { get; private set; } = new Vector3();
//
//  /**
//   * Constructs a new level manager.
//   *
//   * @param levelRoot The node to append level entities under.
//   * @param player The player object. Used in calculating the geometry of the level grid during a load.
//   */
//  public LevelManager(Node levelRoot, Player player) {
//    _levelRoot = levelRoot;
//    Debug.Assert(_levelRoot != null);
//    _player = player;
//    Debug.Assert(_player != null);
//  }
//
//  private Level _ReadLevel(string path) {
//    var reader = new File();
//    reader.Open(path, File.ModeFlags.Read);
//    var json = reader.GetAsText();
//    reader.Close();
//    return Level.fromJson(json);
//  }
//
//  private void _AddObstacle(int id, Vector3 position) {
//    if (id == 0)
//    {
//      return;
//    }
//    var spatial = Current.Scenes[Current.Entities[id - 1].ScenePath].Instance() as Spatial;
//    spatial.Translation = position;
//    if (spatial is IEntity)
//    {
//      var entity = spatial as IEntity;
//      entity.SetMode(Current.Entities[id - 1].Mode);
//      entity.UpdateMovementExtents(_player.MovementExtents);
//      entity.SetScaling(Current.Entities[id - 1].ScaleX, Current.Entities[id - 1].ScaleY);
//      entity.SetDirection(Current.Entities[id - 1].DirectionX, Current.Entities[id - 1].DirectionY);
//    }
//    _levelRoot.CallDeferred("add_child", spatial);
//  }
//
//  private void _InitializeLevel() {
//    _player.Translation = new Vector3();
//    const float step = -6f;
//    var grid = _player.MovementExtents + new Vector3(0f, 0f, 1f);
//    var depth = 3f * step;
//
//    // There is probably a much more efficient way to do this!
//    foreach (var list in Current.Data)
//    {
//      _AddObstacle(list[0], new Vector3(-1f, 1f, depth) * grid);
//      _AddObstacle(list[1], new Vector3(0f, 1f, depth) * grid);
//      _AddObstacle(list[2], new Vector3(1f, 1f, depth) * grid);
//      _AddObstacle(list[3], new Vector3(-1f, 0f, depth) * grid);
//      _AddObstacle(list[4], new Vector3(0f, 0f, depth) * grid);
//      _AddObstacle(list[5], new Vector3(1f, 0f, depth) * grid);
//      _AddObstacle(list[6], new Vector3(-1f, -1f, depth) * grid);
//      _AddObstacle(list[7], new Vector3(0f, -1f, depth) * grid);
//      _AddObstacle(list[8], new Vector3(1f, -1f, depth) * grid);
//      depth += step;
//    }
//    ExitDepth = new Vector3(0f, 0f, depth) * grid;
//    GD.Print($"Exit Depth: {ExitDepth}");
//  }
//
//  /**
//   * Load a level.
//   *
//   * @param level The path to the level to load (e.g., res://levels/level.json).
//   */
//  public void LoadLevel(string level) {
//    Debug.Assert(level != null);
//    Debug.Assert(!level.Empty());
//    Current = _ReadLevel(level);
//    _InitializeLevel();
//  }
//}
