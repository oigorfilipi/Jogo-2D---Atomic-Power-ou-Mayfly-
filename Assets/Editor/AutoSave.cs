using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoSave
{
    // O construtor é chamado automaticamente quando a Unity compila o código
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state)
    {
        // Se a Unity estiver prestes a sair do modo de edição para entrar no Play
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto-salvando Cena e Projeto antes de testar...");
            EditorSceneManager.SaveOpenScenes(); // Salva a Hierarquia (Cena)
            AssetDatabase.SaveAssets();          // Salva os Prefabs e Materiais (Projeto)
        }
    }
}