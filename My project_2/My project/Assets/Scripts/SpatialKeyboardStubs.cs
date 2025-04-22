// Place this file under Assets/Scripts to satisfy missing sample types
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard
{
    /// <summary>
    /// Stub for XRKeyboardKey from the Spatial Keyboard sample.
    /// Add any properties that KeyboardOptimizer.cs references.
    /// </summary>
    public class XRKeyboardKey : MonoBehaviour
    {
        // The graphic component that KeyboardOptimizer moves
        public Graphic targetGraphic;
        public Text textComponent;
        public Graphic iconComponent;
        public Graphic highlightComponent;
    }
}
