using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    [SerializeField] GameObject pg1;
    [SerializeField] GameObject pg2;
    [SerializeField] GameObject pg3;
    [SerializeField] GameObject pg4;
    [SerializeField] GameObject pg5;

    public void OnOnePressed()
    {
        pg1.SetActive(true);
        pg2.SetActive(false);
        pg3.SetActive(false);
        pg4.SetActive(false);
        pg5.SetActive(false);
    }

    public void OnTwoPressed()
    {
        pg1.SetActive(false);
        pg2.SetActive(true);
        pg3.SetActive(false);
        pg4.SetActive(false);
        pg5.SetActive(false);
    }

    public void OnThreePressed()
    {
        pg1.SetActive(false);
        pg2.SetActive(false);
        pg3.SetActive(true);
        pg4.SetActive(false);
        pg5.SetActive(false);
    }

    public void OnFourPressed()
    {
        pg1.SetActive(false);
        pg2.SetActive(false);
        pg3.SetActive(false);
        pg4.SetActive(true);
        pg5.SetActive(false);
    }

        public void OnFivePressed()
    {
        pg1.SetActive(false);
        pg2.SetActive(false);
        pg3.SetActive(false);
        pg4.SetActive(false);
        pg5.SetActive(true);
    }
}
