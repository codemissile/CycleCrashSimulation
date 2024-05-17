using System.IO;
using UnityEngine;

public class QuaternionLogger : MonoBehaviour
{
    private float logTimer = 0.0f;
    private float sampleRate = 0.1f; // Log every 0.1 seconds for 10 Hz
    private string filePath;

    void Start()
    {
        // Setup the logging file path.
        string folderPath = Path.Combine(Application.persistentDataPath, "QuaternionLogs");
        Directory.CreateDirectory(folderPath); // Create the directory if it doesn't exist.
        filePath = Path.Combine(folderPath, "quaternion_data.csv");

        // Overwrite the file with a header for this session's data.
        File.WriteAllText(filePath, "Time,RotationX,RotationY,RotationZ,RotationW,Magnitude\n");
    }

    void Update()
    {
        // Simulate orientation change or handle user input here.
        // Example: Rotate the object about the Y-axis at 1 degree per second.
        transform.Rotate(new Vector3(0, Time.deltaTime * 1, 0), Space.World);
    }

    void FixedUpdate()
    {
        logTimer += Time.fixedDeltaTime;
        if (logTimer >= sampleRate)
        {
            logTimer = 0f;
            Quaternion currentRotation = transform.rotation;
            float magnitude = Mathf.Sqrt(currentRotation.x * currentRotation.x + currentRotation.y * currentRotation.y + currentRotation.z * currentRotation.z + currentRotation.w * currentRotation.w);

            LogQuaternionData(Time.time, currentRotation, magnitude);
        }
    }

    void LogQuaternionData(float time, Quaternion rotation, float magnitude)
    {
        // Log the current time, rotation quaternion components, and magnitude.
        string dataLine = $"{time},{rotation.x},{rotation.y},{rotation.z},{rotation.w},{magnitude}\n";
        File.AppendAllText(filePath, dataLine);

        // Optionally, log to console for immediate feedback during development.
        Debug.Log($"Logged Quaternion Data: {dataLine}");
    }
}
