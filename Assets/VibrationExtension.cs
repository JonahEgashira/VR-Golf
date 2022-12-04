using System.Collections;
using UnityEngine;

/// <summary>
/// 振動処理拡張クラス
/// </summary>
public class VibrationExtension : MonoBehaviour
{
    // Singleton化
    private static VibrationExtension _instance;
    public static VibrationExtension Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void VibrateController(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        StartCoroutine(VibrateForSeconds(duration, frequency, amplitude, controller));
    }

    IEnumerator VibrateForSeconds(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        // 振動開始
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        // 振動間隔分待つ
        yield return new WaitForSeconds(duration);

        // 振動終了
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
