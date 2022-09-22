/** Game scene management object.
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
using System.Diagnostics;
using Godot;

public class SceneManager : IDependsOnMain {
  /**
   * The delegate signature for scene configurations.
   */
  public delegate void SceneConfigurationMethod(Node target);

  private Main _main = null;
  private Node _viewRoot = null;
  private AnimationPlayer _fader = null;

  /**
   * The path of the current scene.
   */
  public string Current { get; private set; } = null;

  /**
   * The configuration delegate used for the current scene.
   */
  public SceneConfigurationMethod CurrentConfigMethod { get; private set; } = null;

  /**
   * Constructs a new SceneManager.
   *
   * @param main The main node of the game.
   * @param viewRoot The node under which to load scenes.
   * @param fader An AnimationPlayer that controls the visibility of the game viewport during loading operations.
   */
  public SceneManager(Main main, Node viewRoot, AnimationPlayer fader) {
    SetMainNode(main);
    _viewRoot = viewRoot;
    Debug.Assert(_viewRoot != null);
    _fader = fader;
    Debug.Assert(_fader != null);
  }

  /**
   * Set the main node.
   *
   * @param main The top level node of the scene.
   */
  public void SetMainNode(Main main) {
    Debug.Assert(main != null);
    _main = main;
  }


  /**
   * Get the main node.
   *
   * @return The main node.
   */
  public Main GetMainNode() {
    return _main;
  }

  /**
   * Load a scene under the specified root node.
   *
   * @param scene The path to the scene to load.
   * @param config The configuration delegate for the scene to load.
   */
  public void LoadScene(string scene, SceneConfigurationMethod config) {
    Debug.Assert(scene != null);
    Debug.Assert(!scene.Empty());
    GD.Print($"Loading scene: \"{scene}\"");
    _fader.Play("FadeOut");
    for(int i = 0; i < _viewRoot.GetChildCount(); ++i)
    {
      _viewRoot.GetChild(i).QueueFree();
    }
    var packed = GD.Load<PackedScene>(scene);
    var inst = packed.Instance();
    if (inst as IDependsOnMain != null)
    {
      var dep = inst as IDependsOnMain;
      dep.SetMainNode(_main);
    }
    if (inst as IRequiresConfiguration != null && config != null)
    {
      var conf = inst as IRequiresConfiguration;
      conf.Configure(config);
    }
    _viewRoot.CallDeferred("add_child", inst);
    _fader.Play("FadeIn");
    Current = scene;
    CurrentConfigMethod = config;
  }

  /**
   * Load a scene under the specified root node.
   *
   * @param scene The path to the scene to load.
   */
  public void LoadScene(string scene) {
    LoadScene(scene, null);
  }

  /**
   * Reload the current scene with the current configuration.
   */
  public void ReloadScene() {
    GD.Print("Reload Scene");
    LoadScene(Current, CurrentConfigMethod);
  }

}
