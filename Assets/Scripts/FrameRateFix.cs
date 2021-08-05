using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FrameRateFix : MonoBehaviour
{
    List<XRDisplaySubsystemDescriptor> displaysDescs = new List<XRDisplaySubsystemDescriptor>();
    List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
 
    bool IsActive()
    {
        displaysDescs.Clear();
        SubsystemManager.GetSubsystemDescriptors(displaysDescs);
 
        // If there are registered display descriptors that is a good indication that VR is most likely "enabled"
        return displaysDescs.Count > 0;
    }
 
    bool IsVrRunning()
    {
        bool vrIsRunning = false;
        displays.Clear();
        SubsystemManager.GetInstances(displays);
        foreach (var displaySubsystem in displays)
        {
            if (displaySubsystem.running)
            {
                vrIsRunning = true;
                break;
            }
        }
 
        return vrIsRunning;
    }


    private void Update()
    {
        if (IsVrRunning())
        {
            Time.fixedDeltaTime = (Time.timeScale / XRDevice.refreshRate);
        }
        else
        {
            Time.fixedDeltaTime = Time.timeScale / 60.0f;
        }
    }
}
