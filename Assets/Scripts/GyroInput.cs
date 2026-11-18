using UnityEngine;
using UnityEngine.InputSystem;

public class GyroInput : MonoBehaviour
{
    // Указываем полное имя, чтобы Unity не путалась между старым и новым Gyroscope
    private UnityEngine.InputSystem.Gyroscope gyro;
    private bool gyroEnabled = false;

    void Start()
    {
        // Проверяем наличие гироскопа через новый Input System
        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            gyro = UnityEngine.InputSystem.Gyroscope.current;
            gyroEnabled = true;
            Debug.Log("Гироскоп включен");
        }
        else
        {
            Debug.Log("Гироскоп не найден (это нормально для ПК)");
        }
    }

    void Update()
    {
        if (!gyroEnabled || gyro == null) return;

        // Получаем угловую скорость вращения
        Vector3 angularVelocity = gyro.angularVelocity.ReadValue();

        // Поворачиваем камеру по оси Y
        float rotationAmount = angularVelocity.y * 50f * Time.deltaTime;

        if (Camera.main != null)
        {
            Camera.main.transform.Rotate(0, rotationAmount, 0);
        }
    }

    void OnDestroy()
    {
        if (gyroEnabled && UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.DisableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }
    }
}