using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Gesture
{
    public string gestureId;
    public string gestureNameInSequence;
    public string gestureDisplayName;
    public string gestureDescription;
    public int timerDuration;
    public string videoFileName;

    public override string ToString() => $"gestureId={gestureId}, " +
            $"gestureNameInSequence={gestureNameInSequence}, " +
            $"gestureDisplayName={gestureDisplayName}, " +
            $"gestureDescription={gestureDescription}, " +
            $"timerDuration={timerDuration}, " +
            $"videoFileName={videoFileName}";
}