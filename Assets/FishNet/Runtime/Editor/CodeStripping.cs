﻿
using FishNet.Configuring;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;

#if UNITY_EDITOR
using UnityEditor.Compilation;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.Build;
#endif

namespace FishNet.Configuring
{


    public class CodeStripping
    //PROSTART
#if UNITY_EDITOR
    : IPreprocessBuildWithReport, IPostprocessBuildWithReport
#endif
    //PROEND
    {

        /// <summary>
        /// True if making a release build for client.
        /// </summary>
        public static bool ReleasingForClient => (Configuration.ConfigurationData.IsBuilding && !Configuration.ConfigurationData.IsHeadless && !Configuration.ConfigurationData.IsDevelopment);
        /// <summary>
        /// True if making a release build for server.
        /// </summary>
        public static bool ReleasingForServer => (Configuration.ConfigurationData.IsBuilding && Configuration.ConfigurationData.IsHeadless && !Configuration.ConfigurationData.IsDevelopment);
        /// <summary>
        /// Returns if to remove server logic.
        /// </summary>
        /// <returns></returns>
        public static bool RemoveServerLogic
        {
            get
            {
                //PROSTART
                if (!StripBuild)
                    return false;
                //Cannot remove server code if headless.
                if (Configuration.ConfigurationData.IsHeadless)
                    return false;

                return true;
                //PROSTART

                /* This is to protect non pro users from enabling this
                 * without the extra logic code.  */
#pragma warning disable CS0162 // Unreachable code detected
                return false;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
        /// <summary>
        /// Returns if to remove server logic.
        /// </summary>
        /// <returns></returns>
        public static bool RemoveClientLogic
        {
            get
            {
                //PROSTART
                if (!StripBuild)
                    return false;
                //Cannot remove server code if headless.
                if (!Configuration.ConfigurationData.IsHeadless)
                    return false;

                return true;
                //PROEND

                /* This is to protect non pro users from enabling this
                 * without the extra logic code.  */
#pragma warning disable CS0162 // Unreachable code detected
                return false;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
        /// <summary>
        /// True if building and stripping is enabled.
        /// </summary>
        public static bool StripBuild
        {
            get
            {
                //PROSTART
                if (!Configuration.ConfigurationData.IsBuilding || Configuration.ConfigurationData.IsDevelopment)
                    return false;
                //Stripping isn't enabled.
                if (!Configuration.ConfigurationData.StripReleaseBuilds)
                    return false;

                //Fall through.
                return true;
                //PROEND

                /* This is to protect non pro users from enabling this
                 * without the extra logic code.  */
#pragma warning disable CS0162 // Unreachable code detected
                return false;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        //PROSTART
        #region Pro stuff.
        private static object _compilationContext;
        public int callbackOrder => 0;

#if UNITY_EDITOR

        public void OnPreprocessBuild(BuildReport report)
        {
            CompilationPipeline.compilationStarted += CompilationPipelineOnCompilationStarted;
            CompilationPipeline.compilationFinished += CompilationPipelineOnCompilationFinished;

            //Set building values.
            Configuration.ConfigurationData.IsBuilding = true;

            BuildOptions options = report.summary.options;
            Configuration.ConfigurationData.IsHeadless = options.HasFlag(BuildOptions.EnableHeadlessMode);
            Configuration.ConfigurationData.IsDevelopment = options.HasFlag(BuildOptions.Development);

            //Write to file.
            Configuration.ConfigurationData.Write(false);
        }
        /* Solution for builds ending with errors and not triggering OnPostprocessBuild.
        * Link: https://gamedev.stackexchange.com/questions/181611/custom-build-failure-callback
        */
        private void CompilationPipelineOnCompilationStarted(object compilationContext)
        {
            _compilationContext = compilationContext;
        }

        private void CompilationPipelineOnCompilationFinished(object compilationContext)
        {
            if (compilationContext != _compilationContext)
                return;

            _compilationContext = null;

            CompilationPipeline.compilationStarted -= CompilationPipelineOnCompilationStarted;
            CompilationPipeline.compilationFinished -= CompilationPipelineOnCompilationFinished;

            UnsetIsBuilding();
        }

        private void UnsetIsBuilding()
        {
            //Set building values.
            Configuration.ConfigurationData.IsBuilding = false;
            Configuration.ConfigurationData.IsHeadless = false;
            Configuration.ConfigurationData.IsDevelopment = false;
            //Write to file.
            Configuration.ConfigurationData.Write(false);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (Configuration.ConfigurationData.IsBuilding)
                UnsetIsBuilding();
        }
#endif
        #endregion
        //PROEND
    }

}
