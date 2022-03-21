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
	/// <summary><see cref="OnScreenControl"/> で定義された値。</summary>
	protected override string controlPathInternal { get => _controlPath; set => _controlPath = value; }

	/// <summary>オブジェクトの位置。</summary>
	private Vector2 _objectPosition;

	/// <summary>オブジェクトのサイズの半分 (スケールも含む)。</summary>
	private Vector2 _objectSizeHalf;


	/// <summary>
	/// オブジェクトが動作する最初のタイミングで１回だけ呼ばれます。
	/// </summary>
	private void Start()
	{
		var rectTransform = (RectTransform)base.transform;

		// オブジェクトの位置を取得
		_objectPosition = rectTransform.anchoredPosition;

		// オブジェクトのサイズの半分を取得 (スケールサイズも考慮)
		_objectSizeHalf = rectTransform.sizeDelta * rectTransform.localScale / 2f;
	}

	/// <summary>ドラッグの初期化処理として呼ばれます。</summary>
	/// <param name="eventData">タッチ情報。</param>
	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		// タッチのスライド操作を即座に発生させたいのでドラッグ開始までの閾値を無効にします
		eventData.useDragThreshold = false;
	}

	/// <summary>タッチしたタイミングで呼ばれます。</summary>
	/// <param name="eventData">タッチ情報。</param>
	public void OnPointerDown(PointerEventData eventData)
	{
		Operate(eventData);
	}

	/// <summary>タッチした後ドラッグするたびに呼ばれます。</summary>
	/// <param name="eventData">タッチ情報。</param>
	public void OnDrag(PointerEventData eventData)
	{
		Operate(eventData);
	}

	/// <summary>タッチを離したときに呼ばれます。</summary>
	/// <param name="eventData">タッチ情報。</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		// 入力をやめた扱いにしたいので zero を渡します
		SendValueToControl(Vector2.zero);
	}

	/// <summary>
	/// 方向パッドの入力処理を行います。
	/// </summary>
	/// <param name="eventData">タッチ情報。</param>
	private void Operate(PointerEventData eventData)
	{
		// タッチ位置を Canvas 上の位置に変換します
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			transform.parent.GetComponentInParent<RectTransform>(),
			eventData.position,
			eventData.pressEventCamera,
			out Vector2 localPoint);

		// Dpad の中心を原点としたタッチ位置
		Vector2 positionInDpad = localPoint - _objectPosition;

		// タッチ位置をオブジェクトサイズの半分で割り 0～1 の範囲に収めます
		Vector2 positionRate = Vector2.ClampMagnitude(positionInDpad / _objectSizeHalf, 1);

		// 入力値を OnScreenControl に渡してその後の処理を任せます。
		SendValueToControl(positionRate);
	}
}
