using UnityEngine;
using UnityEngine.UI;
public class TalkManager : MonoBehaviour
{
    public Text talkText;
    public GameObject scanObject;

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        talkText.text = "이것의 이름은" + scanObject.name;
    }
}
