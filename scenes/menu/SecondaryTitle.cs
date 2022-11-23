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

public class SecondaryTitle : VBoxContainer {

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

  private Main _main = null;

  private void _OnExitPressed() {
    EmitSignal("TransitionRoot", RootScenes.Exit);
  }

  private void _OnPlayPressed() {
    EmitSignal("TransitionRoot", RootScenes.Play);
  }

  private void _OnSettingsPressed() {
    EmitSignal("Transition", MenuScenes.Settings);
  }

  private void _OnRebirthPressed() {
    EmitSignal("Transition", MenuScenes.Rebirth);
  }

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _main = GetNode<Main>("/root/Main");
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    var playButton = GetNode<Button>("Play");
    var rebirthButton = GetNode<Button>("Rebirth");
    if (_main.Player.Progress == 0)
    {
      playButton.Text = "New Game";
    }
    else
    {
      playButton.Text = "Continue";
    }
    if (_main.Player.IsInitialized())
    {
      rebirthButton.Disabled = true;
    }
    else
    {
      rebirthButton.Connect("pressed", this, "_OnRebirthPressed");
    }
    playButton.Connect("pressed", this, "_OnPlayPressed");
    GetNode<Button>("Settings").Connect("pressed", this, "_OnSettingsPressed");
    GetNode<Button>("Exit").Connect("pressed", this, "_OnExitPressed");
    playButton.GrabFocus();
  }

}
