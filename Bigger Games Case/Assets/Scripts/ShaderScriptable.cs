using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Shaders")]
    public class ShaderScriptable : ScriptableObject
    {
        public List<Material> Materials;
    }
}
