using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : MonoBehaviour
{
    HotAirBalloon Balloon;
    private void Start()
    {
        Balloon = FindObjectOfType<HotAirBalloon>();
    }

    public void ValveStart()
    {
        Balloon.isReleaseOn = true;
    }

    public void ValveStop()
    {
        Balloon.isReleaseOn = false;
    }
}
