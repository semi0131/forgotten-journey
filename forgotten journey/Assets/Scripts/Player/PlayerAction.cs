using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public TalkManager manager;
    GameObject scanObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
            manager.Action(scanObject);
    }
}
