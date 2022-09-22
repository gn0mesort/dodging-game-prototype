/** Title screen menu node.
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
using System.Diagnostics;

public class TitleMenu : Control, IDependsOnMain {
  private Main _main = null;
  private Button _playButton = null;
  private Button _quitButton = null;

  /**
   * Set the main node.
   *
   * @param main The top level node of the scene.
   */
  public void SetMainNode(Main main) {
    _main = main;
    Debug.Assert(_main != null);
  }

  /**
   * Get the main node.
   *
   * @return The main node.
   */
  public Main GetMainNode() {
    return _main;
  }

  private void _QuitPressed() {
    _main.ExitGame(0);
  }

  private void _PlayPressed() {
    var config = new Play.Configuration("res://levels/tunnel.json");
    _main.Scenes.LoadScene("res://scenes/Play.tscn", config.Apply);
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _playButton = GetNode<Button>("Options/Play");
    _playButton.Connect("pressed", this, "_PlayPressed");
    _quitButton = GetNode<Button>("Options/Quit");
    _quitButton.Connect("pressed", this, "_QuitPressed");
  }
}
