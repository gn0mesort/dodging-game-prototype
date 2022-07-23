/** Obstacle spawning entity.
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

public class Spawner : Node {
  private Player _target = null;
  private Vector3[] _positions = new Vector3[9];
  private RandomNumberGenerator _rand = new RandomNumberGenerator();

  /**
   * A PackedScene containing the entity that will be spawned.
   * This must be an Obstacle.
   */
  [Export]
  public PackedScene SpawnedEntity { get; set; } = null;

  /**
   * A NodePath indicating the target of the spawned entities.
   * This must be a Player.
   */
  [Export]
  public NodePath Target { get; set; } = "";


  private void _OnTimeout() {
    var position = (_target.Translation * new Vector3(0f, 0f, 1f));
    position += new Vector3(0f, 0f, 10 * _target.BaseSpeed.z);
    var selected = _rand.RandiRange(0, 8);
    var obj = SpawnedEntity.Instance() as Obstacle;
    obj.Target = Target;
    obj.Translation = _positions[selected] + position;
    GetParent<Node>().CallDeferred("add_child", obj);
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    if (Target == "" || SpawnedEntity == null)
    {
      return;
    }
    _rand.Randomize();
    _target = GetNode<Player>(Target);
    // Initialize positions based on Player.
    // This should form 9 grid cells.
    _positions[0] = new Vector3(-1f, 1f, 0f) * _target.MovementExtents;
    _positions[1] = new Vector3(0f, 1f, 0f) * _target.MovementExtents;
    _positions[2] = new Vector3(1f, 1f, 0f) * _target.MovementExtents;
    _positions[3] = new Vector3(-1f, 0f, 0f) * _target.MovementExtents;
    _positions[4] = new Vector3(0f, 0f, 0f);
    _positions[5] = new Vector3(1f, 0f, 0f) * _target.MovementExtents;
    _positions[6] = new Vector3(-1f, -1f, 0f) * _target.MovementExtents;
    _positions[7] = new Vector3(0f, -1f, 0f) * _target.MovementExtents;
    _positions[8] = new Vector3(1f, -1f, 0f) * _target.MovementExtents;
    var timer = GetNode<Timer>("Timer");
    timer.Connect("timeout", this, "_OnTimeout");
  }
}
