using Godot;

public class SceneManager : IDependsOnMain {
  public delegate void SceneConfigurationMethod(Node target);

  private Main _main = null;
  private Node _viewRoot = null;
  private AnimationPlayer _fader = null;

  public string Current { get; private set; } = null;
  public SceneConfigurationMethod CurrentConfigMethod { get; private set; } = null;

  public SceneManager(Main main, Node viewRoot, AnimationPlayer fader) {
    SetMainNode(main);
    _viewRoot = viewRoot;
    _fader = fader;
  }

  public void SetMainNode(Main main) {
    _main = main;
  }

  public Main GetMainNode() {
    return _main;
  }

  public void LoadScene(string scene, SceneConfigurationMethod config) {
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

  public void LoadScene(string scene) {
    LoadScene(scene, null);
  }

  public void ReloadScene() {
    GD.Print("Reload Scene");
    LoadScene(Current, CurrentConfigMethod);
  }

}
