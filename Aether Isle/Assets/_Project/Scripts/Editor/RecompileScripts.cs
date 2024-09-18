using UnityEditor;
using UnityEditor.Compilation;

namespace Game.Editor
{
    public class RecompileScripts : EditorWindow
    {
        [MenuItem("GameTools/Recompile")]
        private static void ShowWindow()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}
