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
 * @brief Behavior script for the Rebirth menu.
 */
public class Rebirth : CenterContainer {

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

  private void _OnConfirmPressed() {
    var main = GetNode<Main>("/root/Main");
    main.InitializePlayerData();
    main.StorePlayerData();
    _OnBackPressed();
  }

  private void _OnBackPressed() {
    EmitSignal("Transition", MenuScenes.Secondary);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var confirm = GetNode<Button>("VBoxContainer/VBoxContainer2/Confirm");
    confirm.Connect("pressed", this, "_OnConfirmPressed");
    GetNode<Button>("VBoxContainer/VBoxContainer2/Back").Connect("pressed", this, "_OnBackPressed");
    confirm.GrabFocus();
  }
}
