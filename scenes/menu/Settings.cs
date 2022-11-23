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

using MenuScenes = MenuRoot.MenuScenes;

public class Settings : GridContainer {
  /**
   * @brief A Signal emitted to request a transition between root scenes.
   *
   * @param to The RootScene to transition to.
   */
  [Signal]
  public delegate void Transition(MenuScenes to);

  /**
   * @brief A Signal emitted to request a transition between root scenes.
   *
   * @param to The RootScene to transition to.
   */
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  /**
   * @brief A Signal emitted to request a return to the previous menu.
   */
  [Signal]
  public delegate void RequestBack();

  private void _OnBackPressed() {
    EmitSignal("Transition", MenuScenes.Secondary);
    EmitSignal("RequestBack");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var back = GetNode<Button>("Back");
    back.Connect("pressed", this, "_OnBackPressed");
    back.GrabFocus();
  }
}
