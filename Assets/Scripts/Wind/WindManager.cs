using UnityEngine;

public class WindManager : MonoBehaviour
{
    WindTierManager wtm;

    void Start()
    {
        wtm = GameObject.FindObjectOfType<WindTierManager>();
    }

    /// <summary>
    /// Get the wind with each component representing the speed in m/s. Remember to use Time.deltaTime/
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 GetWind(Vector3 pos)
    {
        return wtm.GetWind(pos);
    }
}