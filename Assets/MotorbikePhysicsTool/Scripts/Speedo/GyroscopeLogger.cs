using System.IO;
using UnityEngine;

public class GyroscopeLogger : MonoBehaviour
{
    private float logTimer = 0.0f;
    private float sampleRate = 0.1f; // Log every 0.1 seconds for 10 Hz
    private string filePath;

    void Start()
    {
        // Enable the gyroscope if it's supported
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Debug.Log("Gyroscope is supported and enabled.");
        }
        else
        {
            Debug.Log("Gyroscope is not supported on this device.");
        }


        // Define the file path for logging
        string folderPath = Path.Combine(Application.persistentDataPath, "GyroscopeLogs");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        filePath = Path.Combine(folderPath, "gyroscope_data.csv");

        // Create or overwrite the file with a header
        File.WriteAllText(filePath, "Time,AttitudeX,AttitudeY,AttitudeZ,AttitudeW,RotationRateX,RotationRateY,RotationRateZ\n");
    }

    void FixedUpdate()
    {
        if (SystemInfo.supportsGyroscope)
        {
            logTimer += Time.fixedDeltaTime;
            if (logTimer >= sampleRate)
            {
                logTimer = 0f;
                LogGyroscopeData(Time.time, Input.gyro.attitude, Input.gyro.rotationRate);
            }
        }
    }

    void LogGyroscopeData(float time, Quaternion attitude, Vector3 rotationRate)
    {
        // Create a line of data for the current time and gyroscope data
        string dataLine = $"{time},{attitude.x},{attitude.y},{attitude.z},{attitude.w},{rotationRate.x},{rotationRate.y},{rotationRate.z}\n";

        // Append the data to the CSV file
        File.AppendAllText(filePath, dataLine);
    }
}
