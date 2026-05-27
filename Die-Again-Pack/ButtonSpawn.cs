using UnityEngine;

public class ButtonSpawn : MonoBehaviour
{
    public int LV;
    public CanvasGroup Lock;
    public void OnPress()
    {
        if (GameManager.instance.LvNow >= LV)
        {
            RandomInon.ButtonSound(GameManager.instance.audioSource, GameManager.instance.buttonSound);
            GameManager.instance.LoadLV(LV);
        }
    }
}
