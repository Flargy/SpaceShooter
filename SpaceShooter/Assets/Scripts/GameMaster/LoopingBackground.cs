using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [SerializeField] private Transform loopObject1 = default;
    [SerializeField] private Transform loopObject2 = default;

    [SerializeField] private Transform startLoopPos = default;
    [SerializeField] private Transform endLoopPos = default;

    [SerializeField] private float loopTime = 10;

    private float loopTimerOne = 0.5f;
    private float loopTimerTwo = 0f;


    private void Start()
    {
        loopObject1.position = Vector3.Lerp(startLoopPos.position, endLoopPos.position, loopTimerOne);
        loopObject2.position = Vector3.Lerp(startLoopPos.position, endLoopPos.position, loopTimerTwo);
    }

    private void Update()
    {
        loopTimerOne += GameVariables.GameTime / loopTime;
        loopTimerTwo += GameVariables.GameTime / loopTime;

        LoopTransforms();
    }


    private void LoopTransforms()
    {
        loopObject1.position = Vector3.Lerp(startLoopPos.position, endLoopPos.position, loopTimerOne);
        loopObject2.position = Vector3.Lerp(startLoopPos.position, endLoopPos.position, loopTimerTwo);

        loopTimerOne = loopTimerOne >= 1 ? 0 : loopTimerOne;
        loopTimerTwo = loopTimerTwo >= 1 ? 0 : loopTimerTwo;
    }


}
