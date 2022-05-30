using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoldProject.FrighteningEvent;

public class InteractWithTraps : FrighteningEventBase //, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler
{
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PlayerManager.mapSeen)
        {
            ActivateTrap();   
        }
    }

    private void ActivateTrap()
    {
        /*RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            Debug.Log(rayHit.transform.name);*/
        Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.back, 500);
        if (hit)
        {
            if (hit.collider.CompareTag("Trap"))
            {
                Debug.Log("TOUCHED");
                StartCoroutine(DoActionCoroutine());
            }
            else
            {
                Debug.Log("NOPE");
            }
        }
    }

    protected override IEnumerator DoActionCoroutine()
    {
        
        yield return null;
        //throw new System.NotImplementedException();
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        throw new System.NotImplementedException();
    }
}

    /*public void OnPointerDown(PointerEventData eventData)
    {

        *//*Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
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
        }*//*
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        *//*Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
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
        }*//*
    }

    public void OnPointerMove(PointerEventData eventData)
    {*//*
        Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        *//*Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(mousePos, Vector3.forward * 100, Color.yellow);
        //Debug.Log(mousePos);*//*

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
        }*//*
    }*/
