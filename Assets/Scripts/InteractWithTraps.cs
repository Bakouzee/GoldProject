using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractWithTraps : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler
{
    public Camera playerCamera;
    private RectTransform _screenRectTransform;

    private void Awake()
    {
        _screenRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            Debug.Log(rayHit.transform.name);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, Vector3.forward * 100, Color.yellow);
        //Debug.Log(mousePos);
        
        if (Physics.Raycast(ray.origin, Vector3.forward * 100, out RaycastHit hit))
        {
            Debug.Log("Oui le raycast est true");
            if(hit.collider != null)
            {
                Debug.Log("Youhou");
            } else
            {
                Debug.Log("Dommage le man");
            }
        }

       /* RectTransformUtility.ScreenPointToLocalPointInRectangle(_screenRectTransform, eventData.position, null, out Vector2 localClick);
        localClick.y = (_screenRectTransform.rect.yMin * -1) - (localClick.y * -1);
        Debug.Log(localClick);

        Vector2 viewportClick = new Vector2(localClick.x / _screenRectTransform.rect.xMax, localClick.y / (_screenRectTransform.rect.yMin * -1));
        Debug.Log(viewportClick);

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(viewportClick.x, viewportClick.y, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Test"))
            {
                Debug.Log("Touched");
            }
            else
            {
                Debug.Log("Nope");
            }
        }*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, Vector3.forward * 100, Color.yellow);
        //Debug.Log(mousePos);

        if (Physics.Raycast(ray.origin, Vector3.forward * 100, out RaycastHit hit))
        {
            Debug.Log("Oui le raycast est true");
            if (hit.collider != null)
            {
                Debug.Log("Youhou");
            }
            else
            {
                Debug.Log("Dommage le man");
            }
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mousePos, Vector3.forward * 100, Color.yellow);
        //Debug.Log(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward * 100);

        if (hit)
        {
            Debug.Log("Oui le raycast est true");
            if (hit.collider != null)
            {
                Debug.Log("Youhou");
            }
            else
            {
                Debug.Log("Dommage le man");
            }
        }
    }
}
