using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class WavesEditorCollider
{
    public enum eColliderType
    {
        Item,
        Arrow,
    }

    public eColliderType ColliderType;

    public SpawnedItemDataObject SpwanItem;

    public Vector2 Center;

    public float Radius;

    public bool IsDragged;

    
}

public static class WavesEditorPreviewPanelExtension  {

    private const float Y_HIGHT_VALUE = 0.7f;

    private static Vector2 _mousePsition = Vector2.zero;

    private static List<WavesEditorCollider> _collidersList = new List<WavesEditorCollider>();

    private static WavesEditorCollider _grabbedCollider = null;

    private static Rect previewRect;

    public static void DrawPreviewPanel(this WavesEditor wavesEditor, Rect rect)
    {
        previewRect = rect;

        if (_collidersList == null)
        {
            _collidersList = new List<WavesEditorCollider>();
        }

        EditorGUI.DrawRect(rect, Color.black);

        if (_mousePsition != Vector2.zero)
        {
           // EditorGUI.DrawRect(new Rect(_mousePsition.x- 16, _mousePsition.y - 16, 32, 32), Color.yellow);
        }

        Rect yLineIndicationRect = new Rect(rect.position.x, rect.position.y + rect.height * Y_HIGHT_VALUE, rect.width, 1);
        Rect centerLineIndicationRect = new Rect(rect.width * 0.5f, 0, 1, rect.height);
        Color lineIndicatorColor = new Color(0, 1, 0, 0.3f);
        EditorGUI.DrawRect(yLineIndicationRect, lineIndicatorColor);
        EditorGUI.DrawRect(centerLineIndicationRect, lineIndicatorColor); 

        if (wavesEditor._currentWave != null && wavesEditor._currentWave.SpawnedItems != null)
        {
            foreach (SpawnedItemDataObject spwanItem in wavesEditor._currentWave.SpawnedItems)
            {
                DrawSpwanItem(spwanItem, rect, wavesEditor);

            }

            for (int i = _collidersList.Count - 1; i >= 0; i--)
            {
                bool isValid = false;
                foreach (SpawnedItemDataObject spwanItem in wavesEditor._currentWave.SpawnedItems)
                {
                    if (spwanItem == _collidersList[i].SpwanItem)
                    {
                        isValid = true;
                    }
                }

                if (isValid)
                {
                   // EditorGUI.DrawRect(new Rect(_collidersList[i].Center.x - _collidersList[i].Radius * 0.5f, _collidersList[i].Center.y - _collidersList[i].Radius * 0.5f, _collidersList[i].Radius, _collidersList[i].Radius), Color.yellow);
                }
                else
                {
                    _collidersList.Remove(_collidersList[i]);
                }
            }
        }
        else
        {
            _collidersList.Clear();
        }
       
       
    } 


    public static void MouseDown(this WavesEditor wavesEditor, Vector2 mousePosition)
    {
        _mousePsition = mousePosition;

        foreach (WavesEditorCollider collider in _collidersList)
        {
            if (Vector2.Distance(_mousePsition, collider.Center) < collider.Radius * 0.5f)
            {
                _grabbedCollider = collider;
                break;
            }
        }
    }

    public static void MouseDrag(this WavesEditor wavesEditor, Vector2 mousePosition)
    {
        _mousePsition = mousePosition;
        if (_grabbedCollider != null)
        {
            switch(_grabbedCollider.ColliderType)
            {
                case WavesEditorCollider.eColliderType.Item:
                    {
                        _grabbedCollider.SpwanItem.XPosition = _mousePsition.x - previewRect.width * 0.5f;
                        break;
                    }
                case WavesEditorCollider.eColliderType.Arrow:
                    {
                        float PivotY = previewRect.position.y + previewRect.height * Y_HIGHT_VALUE - IconBaseSize * 0.5f;
                        float pivotX = previewRect.width * 0.5f + _grabbedCollider.SpwanItem.XPosition - IconBaseSize * 0.5f;
                        Vector2 pivot = new Vector2(pivotX, PivotY);
                        _grabbedCollider.SpwanItem.ForceVector = new Vector2( _mousePsition.x - pivot.x - IconBaseSize * 0.5f, pivot.y - _mousePsition.y + IconBaseSize * 0.5f);
                        break;
                    }
            }
        }
    }

    public static void MouseUp(this WavesEditor wavesEditor, Vector2 mousePosition)
    {
        _mousePsition = Vector2.zero;
        _grabbedCollider = null;
    }


    private static void DrawSpwanItem(SpawnedItemDataObject spwanItem, Rect previewRect, WavesEditor wavesEditor)
    {
        Texture2D itemTexture = null;
        switch(spwanItem.SpwanedColor)
        {
            case eSpwanedColorType.BottomLeft:
                {
                    itemTexture = wavesEditor._leftBottomTexture;
                    break;
                }
            case eSpwanedColorType.BottomRight:
                {
                    itemTexture = wavesEditor._rightBottomTexture;
                    break;
                }
            case eSpwanedColorType.RandomCorner:
                {
                    itemTexture = wavesEditor._randomTexture;
                    break;
                }
            case eSpwanedColorType.TopLeft:
                {
                    itemTexture = wavesEditor._leftTopTexture;
                    break;
                }
            case eSpwanedColorType.TopRight:
                {
                    itemTexture = wavesEditor._rightTopTexture;
                    break;
                }  
        }

        Rect itemRect = RectForSpwanItemInPreview(previewRect, spwanItem.XPosition);

        Rect arrowRect = DrawForceArrow(itemRect.position, spwanItem.ForceVector, wavesEditor._arrowTexture);

        Graphics.DrawTexture(itemRect, itemTexture);

        WavesEditorCollider itemCollider = null;
        WavesEditorCollider arrowCollideer = null;
        foreach (WavesEditorCollider collider in _collidersList)
        {
            if (collider.ColliderType == WavesEditorCollider.eColliderType.Item && collider.SpwanItem == spwanItem)
            {
                itemCollider = collider;
            }
            if (collider.ColliderType == WavesEditorCollider.eColliderType.Arrow && collider.SpwanItem == spwanItem)
            {
                arrowCollideer = collider;
            }
        }

        if (itemCollider == null)
        {
            itemCollider = new WavesEditorCollider();
            itemCollider.SpwanItem = spwanItem;
            itemCollider.ColliderType = WavesEditorCollider.eColliderType.Item;
            _collidersList.Add(itemCollider);
        }

        if (arrowCollideer == null)
        {
            arrowCollideer = new WavesEditorCollider();
            arrowCollideer.SpwanItem = spwanItem;
            arrowCollideer.ColliderType = WavesEditorCollider.eColliderType.Arrow;
            _collidersList.Add(arrowCollideer);
        }

        if (itemCollider != null)
        {
            itemCollider.Center = itemRect.center;
            itemCollider.Radius = 64f;
        }

        if (arrowCollideer != null)
        {
            arrowCollideer.Center = new Vector2(itemRect.center.x + spwanItem.ForceVector.x, itemRect.center.y - spwanItem.ForceVector.y);
            arrowCollideer.Radius = 64f;
        }
    }

    private static Rect DrawForceArrow(Vector2 origin, Vector2 forceVector, Texture2D arrowTexture)
    {
        float height = forceVector.y;
        float width = 64f;
        float posX = -width + origin.x + IconBaseSize;
        float posY = -height + origin.y + IconBaseSize * 0.5f;

        Rect arrowRect = new Rect(posX, posY, width, height);

        Vector2 pivot = new Vector2(origin.x + IconBaseSize * 0.5f, origin.y + IconBaseSize * 0.5f);
        Matrix4x4 matrixBackup = GUI.matrix;
        float angle = Angle(forceVector);
        GUIUtility.RotateAroundPivot(angle, pivot);
        Graphics.DrawTexture(arrowRect, arrowTexture);
        GUI.matrix = matrixBackup;

        return arrowRect;
    }
     
    private static Rect RectForSpwanItemInPreview(Rect previewRect, float xPosition)
    {
        float iconSize = IconBaseSize;
        float posY = previewRect.position.y + previewRect.height * Y_HIGHT_VALUE - iconSize * 0.5f;
        float posX = previewRect.width * 0.5f + xPosition - iconSize * 0.5f;
        return new Rect(posX, posY, iconSize, iconSize) ;
    }

    private static float IconBaseSize
    {
        get
        {
            return 64f;
        }
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

}
