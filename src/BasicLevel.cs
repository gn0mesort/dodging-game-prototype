/**
 * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using Godot;

/**
 * @brief A basic implementation of the abstract Level class.
 *
 * This should be sufficient for use in the majority of cases.
 */
public class BasicLevel : Level {
  private Position3D _entrance = null;
  private Position3D _exit = null;

  /**
   * @brief A NodePath indicating a Position3D that will be used as the player's initial position.
   */
  [Export]
  public NodePath EntrancePosition { get; set; } = null;

  /**
   * @brief A NodePath indicating a Position3D that will be used as the player's final position.
   */
  [Export]
  public NodePath ExitPosition { get; set; } = null;

  /**
   * @brief Retrieve's the 3-dimensional entrance position for the level.
   *
   * @return A Vector3 representing the position of the level entrance.
   */
  public override Vector3 Entrance() {
    return _entrance.Translation;
  }

  /**
   * @brief Retrieve's the 3-dimensional exit position for the level.
   *
   * @return A Vector3 representing the position of the level exit.
   */
  public override Vector3 Exit() {
    return _exit.Translation;
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _entrance = GetNode<Position3D>(EntrancePosition);
    _exit = GetNode<Position3D>(ExitPosition);
  }
}
