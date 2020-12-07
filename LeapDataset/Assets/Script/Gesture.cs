using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Gesture
{
    public string gestureId { get; set; }
    public string gestureNameInSequence { get; set; }
    public string gestureDisplayName { get; set; }
    public string gestureDescription { get; set; }
    public int timerDuration { get; set; }
    public string videoFileName { get; set; }

    public override string ToString() => $"gestureId={gestureId}, " +
            $"gestureNameInSequence={gestureNameInSequence}, " +
            $"gestureDisplayName={gestureDisplayName}, " +
            $"gestureDescription={gestureDescription}, " +
            $"timerDuration={timerDuration}, " +
            $"videoFileName={videoFileName}";
}
