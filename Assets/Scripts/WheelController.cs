using UnityEngine;

public class WheelController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;
    public SimpleCarController carController;
    public float rotationSpeedMultiplier = 1f;

    private void Update()
    {
        float steer = carController.CurrentSteer();
        float targetTorque = carController.CurrentMotorTorque();

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider wheelCollider = wheelColliders[i];
            Transform wheelMesh = wheelMeshes[i];

            // Apply steering to the front wheels
            if (i < 2)
            {
                wheelCollider.steerAngle = carController.maxSteerAngle * steer;
            }

            // Apply motor torque
            ApplyDriveForce(wheelCollider, targetTorque);

            // Rotate the wheel meshes based on the wheel collider's RPM
            float wheelRPM = wheelCollider.rpm;
            float rotationSpeed = wheelRPM / 60 * 360 * Time.deltaTime * rotationSpeedMultiplier;
            wheelMesh.Rotate(Vector3.right, rotationSpeed, Space.Self);

            // Update wheel mesh position and rotation
            wheelCollider.GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);
            wheelMesh.SetPositionAndRotation(wheelPosition, wheelRotation);
        }
    }

    private void ApplyDriveForce(WheelCollider wheelCollider, float targetTorque)
    {
        wheelCollider.motorTorque = targetTorque;
    }
}
