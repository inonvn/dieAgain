using UnityEngine;

public class ButtonSpawn : MonoBehaviour
{
    public int LV;
    public void OnPress()
    {
        GameManager.instance.LoadLV(LV);
    }
}
