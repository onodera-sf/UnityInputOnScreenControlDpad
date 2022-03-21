using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenDpad
	: OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler, IInitializePotentialDragHandler
{
	[InputControl(layout = "Vector2")]
	[SerializeField]
	private string _controlPath;
	/// <summary><see cref="OnScreenControl"/> �Œ�`���ꂽ�l�B</summary>
	protected override string controlPathInternal { get => _controlPath; set => _controlPath = value; }

	/// <summary>�I�u�W�F�N�g�̈ʒu�B</summary>
	private Vector2 _objectPosition;

	/// <summary>�I�u�W�F�N�g�̃T�C�Y�̔��� (�X�P�[�����܂�)�B</summary>
	private Vector2 _objectSizeHalf;


	/// <summary>
	/// �I�u�W�F�N�g�����삷��ŏ��̃^�C�~���O�łP�񂾂��Ă΂�܂��B
	/// </summary>
	private void Start()
	{
		var rectTransform = (RectTransform)base.transform;

		// �I�u�W�F�N�g�̈ʒu���擾
		_objectPosition = rectTransform.anchoredPosition;

		// �I�u�W�F�N�g�̃T�C�Y�̔������擾 (�X�P�[���T�C�Y���l��)
		_objectSizeHalf = rectTransform.sizeDelta * rectTransform.localScale / 2f;
	}

	/// <summary>�h���b�O�̏����������Ƃ��ČĂ΂�܂��B</summary>
	/// <param name="eventData">�^�b�`���B</param>
	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		// �^�b�`�̃X���C�h����𑦍��ɔ������������̂Ńh���b�O�J�n�܂ł�臒l�𖳌��ɂ��܂�
		eventData.useDragThreshold = false;
	}

	/// <summary>�^�b�`�����^�C�~���O�ŌĂ΂�܂��B</summary>
	/// <param name="eventData">�^�b�`���B</param>
	public void OnPointerDown(PointerEventData eventData)
	{
		Operate(eventData);
	}

	/// <summary>�^�b�`������h���b�O���邽�тɌĂ΂�܂��B</summary>
	/// <param name="eventData">�^�b�`���B</param>
	public void OnDrag(PointerEventData eventData)
	{
		Operate(eventData);
	}

	/// <summary>�^�b�`�𗣂����Ƃ��ɌĂ΂�܂��B</summary>
	/// <param name="eventData">�^�b�`���B</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		// ���͂���߂������ɂ������̂� zero ��n���܂�
		SendValueToControl(Vector2.zero);
	}

	/// <summary>
	/// �����p�b�h�̓��͏������s���܂��B
	/// </summary>
	/// <param name="eventData">�^�b�`���B</param>
	private void Operate(PointerEventData eventData)
	{
		// �^�b�`�ʒu�� Canvas ��̈ʒu�ɕϊ����܂�
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			transform.parent.GetComponentInParent<RectTransform>(),
			eventData.position,
			eventData.pressEventCamera,
			out Vector2 localPoint);

		// Dpad �̒��S�����_�Ƃ����^�b�`�ʒu
		Vector2 positionInDpad = localPoint - _objectPosition;

		// �^�b�`�ʒu���I�u�W�F�N�g�T�C�Y�̔����Ŋ��� 0�`1 �͈̔͂Ɏ��߂܂�
		Vector2 positionRate = Vector2.ClampMagnitude(positionInDpad / _objectSizeHalf, 1);

		// ���͒l�� OnScreenControl �ɓn���Ă��̌�̏�����C���܂��B
		SendValueToControl(positionRate);
	}
}
