using System.IO;
using UnityEngine;

public class AccelerationLogger : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 previousVelocity;
    private float logTimer = 0.0f;
    private float sampleRate = 0.1f; // Log every 0.1 seconds for 10 Hz
    private string filePath;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousVelocity = rb.velocity;

        // Define the file path for the folder where you want to save the CSV
        // This ensures the file is in a known location, easy to find
        string folderPath = Path.Combine(Application.persistentDataPath, "AccelerationLogs");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        filePath = Path.Combine(folderPath, "acceleration_data.csv");

        // Create or overwrite the file with a header at the start of each application run
        File.WriteAllText(filePath, "Time,X,Y,Z,Magnitude\n");
    }

    void FixedUpdate()
    {
        logTimer += Time.fixedDeltaTime;
        if (logTimer >= sampleRate)
        {
            logTimer = 0f;
            Vector3 currentVelocity = rb.velocity;
            Vector3 acceleration = (currentVelocity - previousVelocity) / Time.fixedDeltaTime;
            LogAcceleration(Time.time, acceleration);
            previousVelocity = currentVelocity;
        }
    }

    void LogAcceleration(float time, Vector3 acceleration)
    {
        // Calculate the magnitude of the acceleration vector
        float magnitude = Mathf.Sqrt(acceleration.x * acceleration.x + acceleration.y * acceleration.y + acceleration.z * acceleration.z);

        // Create a line of data for the current time and acceleration
        string dataLine = $"{time},{acceleration.x},{acceleration.y},{acceleration.z},{magnitude}\n";

        // Append the data to the CSV file
        File.AppendAllText(filePath, dataLine);
    }
}
