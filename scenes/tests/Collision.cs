/** Collision test.
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

public class Collision : GridMap {
  private KinematicBody _player = null;
  private Camera _camera = null;
  private Obstacle[] _obstacles = new Obstacle[3];

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _player = GetNode<KinematicBody>("Player");
    _camera = GetNode<Camera>("Camera");
    for (int i = 0; i < 3; ++i)
    {
      _obstacles[i] = GetNode<Obstacle>($"Obstacle{i}");
      _obstacles[i].SetMap(this);
    }
    _player.Translation = MapToWorld(0, 0, 0);
    _obstacles[0].Translation = MapToWorld(0, 1, 0);
    _obstacles[1].Translation = MapToWorld(0, -1, 0);
    _obstacles[2].Translation = MapToWorld(-1, -1, 0);
    _camera.Translation = MapToWorld(0, 1, 2);
    _camera.Transform = _camera.Transform.LookingAt(_player.Translation, new Vector3(0, 1, 0));
  }
}

