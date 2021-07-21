using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    HotAirBalloon Balloon;

	private void Start()
	{
        Balloon = FindObjectOfType<HotAirBalloon>();
	}

	public void AttemptBurnStart()
    {
        Balloon.isBurnerOn = true;
    }

    public void BurnStop()
    {
        Balloon.isBurnerOn = false;
    }
}
