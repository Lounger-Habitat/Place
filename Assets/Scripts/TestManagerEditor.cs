using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestManager))]
public class TestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 绘制默认的检视面板布局
        DrawDefaultInspector();
        
        
        GUILayout.BeginVertical();

        // 在按钮上方添加一个标题
        GUILayout.Label("点赞", EditorStyles.boldLabel);
        // 如果按钮被点击
        if (GUILayout.Button("Like!"))
        {
           // 调用MyScript中的DoSomething方法
           ((TestManager)target).DoLike();
        }

        // 结束水平布局组
        GUILayout.EndVertical();

        GUILayout.BeginVertical();

        // 在按钮上方添加一个标题
        GUILayout.Label("礼物", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        // 如果按钮被点击
        if (GUILayout.Button("¥0.1"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        if (GUILayout.Button("¥1"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        if (GUILayout.Button("¥5.2"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // 如果按钮被点击
        if (GUILayout.Button("¥9.9"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        if (GUILayout.Button("¥52"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        if (GUILayout.Button("¥88.8"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // 如果按钮被点击
        if (GUILayout.Button("¥120"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        if (GUILayout.Button("¥188.8"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        if (GUILayout.Button("¥300"))
        {
           // 调用MyScript中的DoSomething方法
           //
        }
        GUILayout.EndHorizontal();
        // 结束水平布局组
        GUILayout.EndVertical();


    }
}