using UnityEngine;
using System.Collections;

public interface IDraggable  {

    bool IsGrabbed { get; set; }

    void BeginDrag();

    void EndDrag();
}
