using Godot;

public class SceneManager {
  private Node _viewRoot = null;
  private AnimationPlayer _fader = null;

  public string Current { get; private set; } = null;

  public SceneManager(Node viewRoot, AnimationPlayer fader) {
    _viewRoot = viewRoot;
    _fader = fader;
  }

  public void LoadScene(string scene) {
    _fader.Play("FadeOut");
    if (_viewRoot.GetChildCount() > 0)
    {
      _viewRoot.GetChild(0).QueueFree();
    }
    var packed = GD.Load<PackedScene>(scene);
    var inst = packed.Instance();
    _viewRoot.CallDeferred("add_child", inst);
    _fader.Play("FadeIn");
    Current = scene;
  }

  public void ReloadScene() {
    LoadScene(Current);
  }

}
