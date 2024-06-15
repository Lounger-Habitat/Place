// Copyright (c) Bytedance. All rights reserved.
// Description:

using System.Collections.Generic;
using ByteDance.LiveOpenSdk.Tea;
using Douyin.LiveOpenSDK;
using UnityEngine;

namespace ByteDance.LiveOpenSdk.Runtime
{
    /// <summary>
    /// 直播开放 SDK 的 API 入口点。
    /// </summary>
    public static class LiveOpenSdk
    {
        public static ILiveOpenSdk Instance { get; } = new LiveOpenSdkImpl();

        public static ILiveCloudGameAPI CloudGameApi => LiveOpenSdkRuntime.CloudGameAPI;

        static LiveOpenSdk()
        {
            InitTeaCommonProps();
        }

        private static void InitTeaCommonProps()
        {
#if ENABLE_MONO
            const string scriptingBackend = "mono";
#elif ENABLE_IL2CPP
            const string scriptingBackend = "il2cpp";
#else
            const string scriptingBackend = "unknown";
#endif
            var props = LiveOpenSdkImpl.InternalEnv.TeaCommonEventProperties
                        ?? new Dictionary<string, object>();
            props[EventProperties.ScriptingBackend] = scriptingBackend;
            props[EventProperties.UnityVersion] = Application.unityVersion;
            props["game_identifier"] = Application.identifier;
            props["game_version"] = Application.version;
            LiveOpenSdkImpl.InternalEnv.TeaCommonEventProperties = props;
        }

        internal static LiveOpenSdkInternalEnv InternalEnv => LiveOpenSdkImpl.InternalEnv;
    }
}