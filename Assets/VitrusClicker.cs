using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VitrusClicker : MonoBehaviour, IPointerDownHandler
{
    Vitrus_Genenator_2 vitrGen;

    public void OnPointerDown(PointerEventData eventData)
    {
       

        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor))
            return;
        Debug.Log("LocalCursor:" + localCursor);

        // if pivot 0.0
        vitrGen.SetAPointInMatrix(
            (int)(localCursor.y / (GetComponent<RectTransform>().rect.height / vitrGen.matrixSize)),
            (int)(localCursor.x / (GetComponent<RectTransform>().rect.width / vitrGen.matrixSize))
            );
    }

    // Start is called before the first frame update
    void Start()
    {
        vitrGen = GetComponent<Vitrus_Genenator_2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
