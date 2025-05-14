using Game.Grid;
using Sound;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Shape
{
    public class ShapeDrag : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        [SerializeField] private Vector2 offset = new(0, 500f);

        private Canvas _canvas;

        private Vector2 _shapeSelectedScale;
        private Vector3 _shapeStartScale;
        private RectTransform _shapeTransform;
        private Vector3 _startPosition;
        
        private void Awake()
        {
            _shapeStartScale = GetComponent<RectTransform>().localScale;
            _shapeTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _startPosition = _shapeTransform.localPosition;
        }

        private void Start()
        {
            var squareScale = FindAnyObjectByType<GridCreator>().SquareScale;
            _shapeSelectedScale = new Vector2(squareScale, squareScale);
        }

        private void OnEnable()
        {
            GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        }

        private void OnDisable()
        {
            GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            _shapeTransform.anchorMin = Vector2.zero;
            _shapeTransform.anchorMax = Vector2.zero;
            _shapeTransform.pivot = Vector2.one / 2f;

            UpdatePosition(eventData);
            
            GameEvents.OnDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        private void UpdatePosition(PointerEventData eventData)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                eventData.position,
                Camera.main, out pos);
            _shapeTransform.localPosition = pos + offset;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GetComponent<RectTransform>().localScale = _shapeSelectedScale;
            SoundManager.Instance.PlaySfx(Config.AudioName.PickUp);
            UpdatePosition(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GetComponent<RectTransform>().localScale = _shapeStartScale;
            GameEvents.CheckIfShapeCanBePlaced();
        }

        public bool IsOnStartPosition()
        {
            return _shapeTransform.localPosition == _startPosition;
        }

        public void MoveShapeToStartPosition()
        {
            _shapeTransform.localPosition = _startPosition;
        }
    }
}