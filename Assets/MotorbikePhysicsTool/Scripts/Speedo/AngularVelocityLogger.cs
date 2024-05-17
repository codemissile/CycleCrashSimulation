using System.IO;
using UnityEngine;

public class AngularVelocityLogger : MonoBehaviour
{
    private Rigidbody rb;
    private float logTimer = 0.0f;
    private float sampleRate = 0.1f; // Log every 0.1 seconds for 10 Hz
    private string filePath;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Setup the logging file path.
        string folderPath = Path.Combine(Application.persistentDataPath, "AngularVelocityLogs");
        Directory.CreateDirectory(folderPath); // Create the directory if it doesn't exist.
        filePath = Path.Combine(folderPath, "angular_velocity_data.csv");

        // Overwrite the file with a header for this session's data.
        File.WriteAllText(filePath, "Time,AngularVelocityX,AngularVelocityY,AngularVelocityZ,Magnitude\n");
    }

    void FixedUpdate()
    {
        logTimer += Time.fixedDeltaTime;
        if (logTimer >= sampleRate)
        {
            logTimer = 0f;
            Vector3 angularVelocity = rb.angularVelocity;
            float magnitude = angularVelocity.magnitude;

            LogAngularVelocityData(Time.time, angularVelocity, magnitude);
        }
    }

    void LogAngularVelocityData(float time, Vector3 angularVelocity, float magnitude)
    {
        // Log the current time, angular velocity components, and magnitude.
        string dataLine = $"{time},{angularVelocity.x},{angularVelocity.y},{angularVelocity.z},{magnitude}\n";
        File.AppendAllText(filePath, dataLine);
    }
}
