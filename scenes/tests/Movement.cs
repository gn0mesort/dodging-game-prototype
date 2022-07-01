/** Movement control test.
 *
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

public class Movement : GridMap {
  private KinematicBody _player = null;
  private Camera _camera = null;

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _player = GetNode<KinematicBody>("Player");
    _camera = GetNode<Camera>("Camera");
    _player.Translation = MapToWorld(0, 0, 0);
    _camera.Translation = MapToWorld(0, 0, 5);
  }
}
