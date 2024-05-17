using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gadd420
{
    public class Speedometer : MonoBehaviour
    {
        public RB_Controller controller;
        public Text mph;
        public Text gear;

        // Logging Variables
        private float logTimer = 0.0f;
        private float sampleRate = 0.1f; // Log every 0.1 seconds for 10 Hz
        private string filePath;

        void Start()
        {
            // Initialize CSV file for logging
            filePath = Path.Combine(Application.persistentDataPath, "acceleration_data.csv");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "Time,X,Y,Z\n");
            }
        }

        void Update()
        {
            if (controller)
            {
                DisplaySpeedAndGear();
                LogAccelerationData();

            }
        }

        void DisplaySpeedAndGear()
        {
            //Gets and displays speed in Mph
            float speed = controller.currentSpeed * controller.msToMph;
            speed = Mathf.Round(speed);
            if (controller.useKmph)
            {
                mph.text = (speed + " Kmph");
            }
            else
            {
                mph.text = (speed + " Mph");
            }


            //Gets and displays the gears
            float currentGear = controller.currentGear + 1;
            gear.text = ("Gear " + currentGear);
        }

        void LogAccelerationData()
        {
            logTimer += Time.deltaTime;
            if (logTimer >= sampleRate)
            {
                logTimer = 0f; // Reset timer for next sample

                // Assume controller.GetAcceleration() is a method you implement to get the acceleration vector
                Vector3 acceleration = controller.GetAcceleration(); // This needs to be implemented based on your RB_Controller

                string dataLine = string.Format("{0},{1},{2},{3}\n", Time.time, acceleration.x, acceleration.y, acceleration.z);
                File.AppendAllText(filePath, dataLine);
            }
        }
    }
}
