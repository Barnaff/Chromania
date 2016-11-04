//
//  BuildPostProcessor.cs
//
//  Lunar Unity Mobile Console
//  https://github.com/SpaceMadness/lunar-unity-console
//
//  Copyright 2016 Alex Lementuev, SpaceMadness.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor.LunarConsole;

using System.Collections;
using System.IO;

using LunarConsolePlugin;

namespace LunarConsolePluginInternal
{
    static class BuildPostProcessor
    {
        // Vuforia plugin might cause issues if it runs first.
        // See: http://forum.unity3d.com/threads/lunar-mobile-console-high-performance-unity-ios-android-logger-built-with-native-platform-ui.347650/page-4#post-2771845
        [PostProcessBuild(1000)]
        static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (LunarConsoleSettings.consoleEnabled)
            {
                if (target.ToString() == "iOS" || target.ToString() == "iPhone")
                {
                    OnPostprocessIOS(pathToBuiltProject);
                }
            }
        }

        static void OnPostprocessIOS(string pathToBuiltProject)
        {
            XCProject project = new XCProject(pathToBuiltProject);

            string[] files = Directory.GetFiles(EditorConstants.EditorPathIOS, "*.projmods", System.IO.SearchOption.AllDirectories);
            foreach (string file in files)
            {
                project.ApplyMod(file);
            }

            project.Save();
        }
    }
}
