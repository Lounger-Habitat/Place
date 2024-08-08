// Copyright (c) Bytedance. All rights reserved.
// Description:

using System.IO;
using ByteDance.LiveOpenSdk.Runtime;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Douyin.LiveOpenSDK.Editor
{
    public class SdkBuildAutoProcess : IPostprocessBuildWithReport
    {
        public int callbackOrder => -1;

        private string SdkVersion => LiveOpenSdk.Instance.Version;

        public void OnPostprocessBuild(BuildReport report)
        {
            var outputPath = report.summary.outputPath;
            var outputFolderPath = Path.GetDirectoryName(outputPath);
            Debug.Log($"{nameof(SdkBuildAutoProcess)} OnPostprocessBuild" +
                      $" sdk {SdkVersion} app {Application.version} {report.summary.platform}" +
                      $", outputPath: {outputPath}");

            if (SdkEditorTool.IsBuildForMobile())
            {
                Debug.Log($"{nameof(SdkBuildAutoProcess)} skip for mobile, {SdkEditorTool.ActiveBuildTarget}");
                return;
            }

            CopyTools(outputFolderPath);

            Debug.Log($"{nameof(SdkBuildAutoProcess)} finish" +
                      $" sdk {SdkVersion} app {Application.version}");
        }

        private void CopyTools(string outputFolderPath)
        {
            var pkgPath = SdkEditorTool.SdkPackagePath;
            SdkEditorTool.CopyFile($"{pkgPath}/dycloudsdk/parfait_crash_handler.exe", outputFolderPath);
        }
    }
}