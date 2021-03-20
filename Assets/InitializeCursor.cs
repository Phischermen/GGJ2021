using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCursor : MonoBehaviour
{
    public Texture2D cursor;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursor, new Vector2(94, 71),CursorMode.ForceSoftware);
    }
}
