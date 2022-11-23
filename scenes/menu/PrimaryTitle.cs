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

/**
 * @brief Behavior script for the primary title menu.
 */
public class PrimaryTitle : VBoxContainer {

  /**
   * @brief A Signal emitted to request a transition between menu scenes.
   *
   * @param to The MenuScene to transition to.
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

  private void _OnAnyPress() {
    EmitSignal("Transition", MenuScenes.Secondary);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    GetNode<Label>("PressAnyKey").Connect("Pressed", this, "_OnAnyPress");
  }
}
