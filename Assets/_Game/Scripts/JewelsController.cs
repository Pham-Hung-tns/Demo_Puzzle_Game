using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JewelsController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Vector3 shapeSelectScale;
    private Vector3 shapeStartScale;
    private Canvas _canvas;
    public Vector3 startPosition;
    public gridColor gridColor;

    private void Start()
    {
        shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _canvas = this.GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        startPosition = transform.parent.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //_transform.anchorMin = Vector2.zero;
        //_transform.anchorMax = Vector2.zero;
        //_transform.pivot = Vector2.zero;
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas.transform as RectTransform,
            eventData.position, _canvas.worldCamera, out pos);
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeStartScale;
        EventsInGame.PlaceGrid(this.GetComponent<Image>().sprite, this, gridColor.ToString());
        if (!Grid.isOnMatrix)
        {
            transform.position = startPosition;
        }
    }
}