using Godot;
using System;

public class Play : Spatial, IDependsOnMain, IRequiresConfiguration {
  public class Configuration {
    public string LevelPath { get; private set; } = "";

    public Configuration(string levelPath) {
      // TODO: verify that the input path is a level path.
      LevelPath = levelPath;
    }

    public void Apply(Node target) {
      var actualTarget = target as Play;
      if (actualTarget == null)
      {
        return;
      }
      actualTarget.Levels.LoadLevel(LevelPath);
    }
  }

  private Main _main = null;
  private SceneManager.SceneConfigurationMethod _deferredConfig = null;
  private Player _player = null;

  public LevelManager Levels { get; private set; } = null;

  public void SetMainNode(Main main) {
    _main = main;
  }

  public Main GetMainNode() {
    return _main;
  }

  public void Configure(SceneManager.SceneConfigurationMethod config) {
    _deferredConfig = config;
  }

  public override void _Ready() {
    _player = GetNode<Player>("Player");
    Levels = new LevelManager(this, _player);
    _deferredConfig(this);
    _player.SetDeferred("axis_lock_motion_z", false);
  }
}
