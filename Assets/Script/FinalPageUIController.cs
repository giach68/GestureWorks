using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalPageUIController : MonoBehaviour
{
    public GameObject FinalPagePanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeMessage(string message)
    {
        FinalPagePanel.transform.Find("AcquisitionCompletedText").GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }
}