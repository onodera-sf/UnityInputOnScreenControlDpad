using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionScript : MonoBehaviour
{
  /// <summary>情報を表示させるテキストオブジェクト。</summary>
  [SerializeField] private Text TextObject;

  /// <summary>アクションマップから自動生成されたクラス。</summary>
  private InputActionSample _actionMap;

  private void Awake()
  {
    // 各操作を行ったときに呼ばれるイベントを設定する
    _actionMap = new InputActionSample();
    _actionMap.Action2D.Move.performed += context => OnMove(context);
    _actionMap.Action2D.Attack.performed += context => OnAttack(context);
    _actionMap.Action2D.Move.canceled += context => OnMove(context);
    _actionMap.Action2D.Attack.canceled += context => OnAttack(context);
  }

  private void OnEnable()
  {
    // このオブジェクトが有効になったときにアクションマップを有効にする
    _actionMap.Enable();
  }

  private void OnDisable()
  {
    // このオブジェクトが無効になったときにアクションマップが余計な動作を起こさないように無効にする
    _actionMap.Disable();
  }

  /// <summary>
  /// Move 操作をした時に呼ばれるメソッドです。
  /// </summary>
  /// <param name="context">コールバックパラメータ。</param>
  public void OnMove(InputAction.CallbackContext context)
  {
    // Move の入力量を取得
    var vec = context.ReadValue<Vector2>();
    TextObject.text = $"Move:({vec.x:f2}, {vec.y:f2})\n{TextObject.text}";
  }

  /// <summary>
  /// Attack 操作をした時に呼ばれるメソッドです。
  /// </summary>
  /// <param name="context">コールバックパラメータ。</param>
  public void OnAttack(InputAction.CallbackContext context)
  {
    // Attack ボタンの状態を取得
    var value = context.ReadValueAsButton();
    TextObject.text = $"Attack:{value}\n{TextObject.text}";
  }
}
