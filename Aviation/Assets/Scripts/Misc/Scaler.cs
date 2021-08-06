using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private GameObject rightGui;
    [SerializeField] private GameObject canvas;
    public GameObject RightGui { get { return rightGui; } set { rightGui = value; } }
    public GameObject Canvas { get { return canvas; } set { canvas = value; } }
    private float borderSizeRight;
    private float initialScaling;
    private float neededScaling;
    public float NeededScaling { get { return neededScaling; } }
    public float BorderSizeRight { get { return borderSizeRight; } set { borderSizeRight = value; } }

    private void Start()
    {
        initialScaling = 16 / 9f;
        neededScaling = Camera.main.aspect / initialScaling;
        Vector2 canvasScale = new Vector2(canvas.transform.localScale.x, canvas.transform.localScale.y);

        float xSize = 0;
        if (gameObject.GetComponent<Collider>() != null)
        {
            xSize = gameObject.GetComponent<Collider>().bounds.size.x / 2 * neededScaling;
        }
        borderSizeRight = rightGui.GetComponent<RectTransform>().rect.size.x *canvasScale.x + xSize;

        gameObject.transform.localScale = gameObject.transform.localScale / neededScaling;
    }
}

