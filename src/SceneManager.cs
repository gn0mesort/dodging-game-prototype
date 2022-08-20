using Godot;

public class SceneManager : IDependsOnMain {
  public delegate void SceneConfigurationMethod(Node target);

  private Main _main = null;
  private Node _viewRoot = null;
  private AnimationPlayer _fader = null;

  public string Current { get; private set; } = null;

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
    _fader.Play("FadeOut");
    if (_viewRoot.GetChildCount() > 0)
    {
      _viewRoot.GetChild(0).QueueFree();
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
  }

  public void LoadScene(string scene) {
    LoadScene(scene, null);
  }

  public void ReloadScene() {
    LoadScene(Current);
  }

}
