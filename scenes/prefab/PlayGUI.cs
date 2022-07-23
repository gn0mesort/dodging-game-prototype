/** Game GUI processing.
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

public class PlayGUI : Control {
  private Player _target = null;
  private Label _score = null;
  private Label _health = null;

  /**
   * A NodePath pointing to the tracked Node.
   * Must be a Player.
   */
  [Export]
  public NodePath Target { get; set; } = "";

  private void _OnTargetStatusChanged() {
    _UpdateLabels();
  }

  private void _UpdateLabels() {
    _score.Text = $"Score: {_target.Score,9:D09}";
    _health.Text = $"Health: {_target.Health,3:D03}";
   }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    if (Target == "")
    {
      return;
    }
    _score = GetNode<Label>("Score");
    _health = GetNode<Label>("Health");
    _target = GetNode<Player>(Target);
    _target.Connect("StatusChanged", this, "_OnTargetStatusChanged");
    _UpdateLabels();
  }
}
