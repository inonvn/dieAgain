using UnityEngine;

[CreateAssetMenu(fileName = "SaveObj", menuName = "Scriptable Objects/SaveObj")]
public class SaveObj : ScriptableObject
{
    public int LV;
    public GameObject GameObject;
    public Vector3 PlayerSpawn;
}
