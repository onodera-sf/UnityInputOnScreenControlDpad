using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionScript : MonoBehaviour
{
  /// <summary>����\��������e�L�X�g�I�u�W�F�N�g�B</summary>
  [SerializeField] private Text TextObject;

  /// <summary>�A�N�V�����}�b�v���玩���������ꂽ�N���X�B</summary>
  private InputActionSample _actionMap;

  private void Awake()
  {
    // �e������s�����Ƃ��ɌĂ΂��C�x���g��ݒ肷��
    _actionMap = new InputActionSample();
    _actionMap.Action2D.Move.performed += context => OnMove(context);
    _actionMap.Action2D.Attack.performed += context => OnAttack(context);
    _actionMap.Action2D.Move.canceled += context => OnMove(context);
    _actionMap.Action2D.Attack.canceled += context => OnAttack(context);
  }

  private void OnEnable()
  {
    // ���̃I�u�W�F�N�g���L���ɂȂ����Ƃ��ɃA�N�V�����}�b�v��L���ɂ���
    _actionMap.Enable();
  }

  private void OnDisable()
  {
    // ���̃I�u�W�F�N�g�������ɂȂ����Ƃ��ɃA�N�V�����}�b�v���]�v�ȓ�����N�����Ȃ��悤�ɖ����ɂ���
    _actionMap.Disable();
  }

  /// <summary>
  /// Move ������������ɌĂ΂�郁�\�b�h�ł��B
  /// </summary>
  /// <param name="context">�R�[���o�b�N�p�����[�^�B</param>
  public void OnMove(InputAction.CallbackContext context)
  {
    // Move �̓��͗ʂ��擾
    var vec = context.ReadValue<Vector2>();
    TextObject.text = $"Move:({vec.x:f2}, {vec.y:f2})\n{TextObject.text}";
  }

  /// <summary>
  /// Attack ������������ɌĂ΂�郁�\�b�h�ł��B
  /// </summary>
  /// <param name="context">�R�[���o�b�N�p�����[�^�B</param>
  public void OnAttack(InputAction.CallbackContext context)
  {
    // Attack �{�^���̏�Ԃ��擾
    var value = context.ReadValueAsButton();
    TextObject.text = $"Attack:{value}\n{TextObject.text}";
  }
}
