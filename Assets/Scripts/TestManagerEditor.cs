using UnityEditor;
#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
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
           ((TestManager)target).SendGift(1f);
        }
        if (GUILayout.Button("¥1"))
        {
           // 调用MyScript中的DoSomething方法
           ((TestManager)target).SendGift(10f);
        }
        if (GUILayout.Button("¥2"))
        {
           ((TestManager)target).SendGift(20f);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // 如果按钮被点击
        if (GUILayout.Button("¥5.2"))
        {
           ((TestManager)target).SendGift(52);
        }
        if (GUILayout.Button("¥9.9"))
        {
            ((TestManager)target).SendGift(99f);
        }
        if (GUILayout.Button("¥19.9"))
        {
           ((TestManager)target).SendGift(199f);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        // 如果按钮被点击
        if (GUILayout.Button("¥29.9"))
        {
           ((TestManager)target).SendGift(299f);
        }
        GUILayout.EndHorizontal();
        // 结束水平布局组
        GUILayout.EndVertical();


    }
}
#endif