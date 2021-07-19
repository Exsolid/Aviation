using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private GameObject leftGui;
    [SerializeField] private GameObject rightGui;
    [SerializeField] private GameObject canvas;
    public GameObject LeftGui { get { return leftGui; } set { leftGui = value; } }
    public GameObject RightGui { get { return rightGui; } set { rightGui = value; } }
    public GameObject Canvas { get { return canvas; } set { canvas = value; } }
    private float borderSizeLeft;
    private float borderSizeRight;
    private float initialScaling;
    private float neededScaling;
    public float NeededScaling { get { return neededScaling; } }
    public float BorderSizeLeft { get { return borderSizeLeft; } set { borderSizeLeft = value; } }
    public float BorderSizeRight { get { return borderSizeRight; } set { borderSizeRight = value; } }

    private void Start()
    {
        initialScaling = 16 / 9f;
        neededScaling = Camera.main.aspect / initialScaling;
        Vector2 canvasScale = new Vector2(canvas.transform.localScale.x, canvas.transform.localScale.y);
        borderSizeLeft = leftGui.GetComponent<RectTransform>().rect.size.x * canvasScale.x + 50 * neededScaling;
        borderSizeRight = rightGui.GetComponent<RectTransform>().rect.size.x *canvasScale.x + 50 * neededScaling;

        gameObject.transform.localScale = gameObject.transform.localScale / neededScaling;
    }
}

