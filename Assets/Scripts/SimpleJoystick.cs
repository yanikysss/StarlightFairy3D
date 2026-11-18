using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform handle;
    public float maxDistance = 80f;

    private Vector2 inputVector;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        // Превращаем позицию пальца в локальные координаты фона
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        // Ограничиваем движение ручки радиусом
        if (pos.magnitude > maxDistance)
            pos = pos.normalized * maxDistance;

        handle.localPosition = pos;

        // Нормализуем вектор (от -1 до 1)
        inputVector = new Vector2(pos.x / maxDistance, pos.y / maxDistance);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.localPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    // Этот метод будет читать Фея
    public Vector2 GetInput()
    {
        return inputVector;
    }
}