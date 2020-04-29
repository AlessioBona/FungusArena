using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VitrusClicker : MonoBehaviour, IPointerDownHandler
{
    BattleManager battleManager;

    public void OnPointerDown(PointerEventData eventData)
    {
       

        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor))
            return;
        Debug.Log("LocalCursor:" + localCursor);

        // if pivot 0.0
        battleManager.SetAPointInMatrix(
            (int)(localCursor.y / (GetComponent<RectTransform>().rect.height / battleManager.matrixSize)),
            (int)(localCursor.x / (GetComponent<RectTransform>().rect.width / battleManager.matrixSize))
            );
    }

    // Start is called before the first frame update
    void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
