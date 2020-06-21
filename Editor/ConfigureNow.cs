using UnityEngine;

namespace Skibitsky.Unity.Editor
{
    /// <summary>
    ///     Configuration Scriptable Object. Must be located at Assets/unity-now/ConfigureNow
    /// </summary>
    // Uncomment the line bellow if you lost the Scriptable Object and need to create a new one.
    // [CreateAssetMenu(fileName = "ConfigureNow", menuName = "unity-now/ConfigureNow", order = 0)]
    public class ConfigureNow : ScriptableObject
    {
        [SerializeField] public string BaseUrl = "https://api.zeit.co/v9/now";

        [SerializeField] public bool CopyUrl = true;
        [SerializeField] public string Token;
    }
}