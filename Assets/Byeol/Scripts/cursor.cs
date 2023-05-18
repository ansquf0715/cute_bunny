using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public bool hotSpotIsCenter = false;
    public Vector2 adjustHotSpot = Vector2.zero;
    Vector2 hotSpot;
    public float cursorSizeMultiplier = 0.7f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MyCursor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MyCursor()
    {
        yield return new WaitForEndOfFrame();

        Texture2D resizedTexture = 
            ResizeTexture(cursorTexture, cursorTexture.width * cursorSizeMultiplier, 
            cursorTexture.height * cursorSizeMultiplier);

        if(hotSpotIsCenter)
        {
            hotSpot.x = cursorTexture.width / 3;
            hotSpot.y = cursorTexture.height / 3;
        }
        else
        {
            hotSpot = adjustHotSpot;
        }

        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    Texture2D ResizeTexture(Texture2D sourceTexture, float targetWidth, float targetHeight)
    {
        RenderTexture rt = new RenderTexture((int)targetWidth, (int)targetHeight, 0);
        RenderTexture.active = rt;
        Graphics.Blit(sourceTexture, rt);
        Texture2D resizedTexture = new Texture2D((int)targetWidth, (int)targetHeight);
        resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = null;
        return resizedTexture;
    }

}
