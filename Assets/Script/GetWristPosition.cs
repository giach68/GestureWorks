using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class GetWristPosition : MonoBehaviour
{
    Controller leapObject = new Controller(0);

    // Start is called before the first frame update
    void Start()
    {  
        
    }

    // Update is called once per frame
    void Update()
    {

        Frame currentFrame = leapObject.Frame(0);
        List<Hand> listHand = new List<Hand>();
        listHand = currentFrame.Hands;

        Hand rightHand = null; 

        foreach (Hand h in listHand)
        {
            if (h != null && h.IsRight)
                rightHand = h;
        }

        if (rightHand != null)
        {
            this.transform.position = rightHand.WristPosition.ToVector3();
            this.transform.rotation = rightHand.Rotation.ToQuaternion();
            //this.transform.position = rightHand.Arm.WristPosition.ToVector3();
        }
    }
}
