///** Pause menu node.
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
//public class Pause : ColorRect, IDependsOnMain {
//  private Main _main = null;
//  private Button _resume = null;
//  private Button _settings = null;
//  private Button _quitToMenu = null;
//  private Button _quitToDesktop = null;
//
//  /**
//   * Set the main node.
//   *
//   * @param main The top level node of the scene.
//   */
//  public void SetMainNode(Main main) {
//    _main = main;
//    Debug.Assert(_main != null);
//  }
//
//  /**
//   * Get the main node.
//   *
//   * @return The main node.
//   */
//  public Main GetMainNode() {
//    return _main;
//  }
//
//  private void _OnResumePressed() {
//    Visible = false;
//    _main.Paused = false;
//    QueueFree();
//  }
//
//  private void _OnQuitToMenuPressed() {
//    _main.Paused = false;
//    _main.Scenes.LoadScene("res://scenes/TitleMenu.tscn");
//  }
//
//  private void _OnQuitToDesktopPressed() {
//    _main.ExitGame(0);
//  }
//
//  /**
//   * Post-_EnterTree initialization.
//   */
//  public override void _Ready() {
//    _resume = GetNode<Button>("Menu/Resume");
//    _resume.Connect("pressed", this, "_OnResumePressed");
//    _settings = GetNode<Button>("Menu/Settings");
//    _quitToMenu = GetNode<Button>("Menu/QuitToMenu");
//    _quitToMenu.Connect("pressed", this, "_OnQuitToMenuPressed");
//    _quitToDesktop = GetNode<Button>("Menu/QuitToDesktop");
//    _quitToDesktop.Connect("pressed", this, "_OnQuitToDesktopPressed");
//  }
//
//  /**
//   * Unhandled input event processing.
//   *
//   * @param ev The InputEvent to process.
//   */
//  public override void _UnhandledInput(InputEvent ev) {
//    if (ev.IsActionPressed("pause"))
//    {
//      _OnResumePressed();
//    }
//  }
//
//}
