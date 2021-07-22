using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    [SerializeField] GameObject pg1;
    [SerializeField] GameObject pg2;
    [SerializeField] GameObject pg3;

    public void OnOnePressed()
    {
        pg1.SetActive(true);
        pg2.SetActive(false);
        pg3.SetActive(false);
    }

    public void OnTwoPressed()
    {
        pg1.SetActive(false);
        pg2.SetActive(true);
        pg3.SetActive(false);
    }

    public void OnThreePressed()
    {
        pg1.SetActive(false);
        pg2.SetActive(false);
        pg3.SetActive(true);
    }
}
