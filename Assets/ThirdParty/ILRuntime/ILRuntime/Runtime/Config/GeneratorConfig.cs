//-----------------------------------------------------------------------
// <filename>GeneratorConfig</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #代码自动生成的一些规则# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 21:57:28# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class GeneratorConfig
{

    /// <summary>
    /// 适配器存放的路径
    /// </summary>
    public const string GenAdapterPath = "Assets/ThirdParty/ILRuntime/ILRuntime/Adapter/";
    public static string GenCLRBindingTrunkPath = "Assets/ThirdParty/ILRuntime/ILRuntime/Binding/trunk2hotfix";
    public static string GenCLRBindingHotfixPath = "Assets/ThirdParty/ILRuntime/ILRuntime/Binding/hotfix2trunk";

    #region 白名单
    /// <summary>
    /// 白名单命名空间
    /// </summary>
    public static List<string> whiteNameSpaceList = new List<string>()
    {
        "GameFramework.Test",
    };

    /// <summary>
    /// 官方白名单程序集
    /// </summary>
    public static List<string> whiteAssemblyList = new List<string>()
    {
        "UnityEngine",
        "UnityEngine.UI",
        "UnityEngine.CoreModule",
        "UnityEngine.UIModule"
    };

    /// <summary>
    /// 用户白名单程序集
    /// </summary>
    public static List<string> whiteUserAssemblyList = new List<string>()
        {
            "Assembly-CSharp",
        };

    /// <summary>
    /// 需要写适配器的类型(需要是全额限定名)
    /// </summary>
    public static List<string> GenAdaterCLRType = new List<string>()
        {
            "SubMonoBehavior",
        };
    #endregion

    #region 黑名单
    /// <summary>
    /// 官方黑名单程序集
    /// </summary>
    public static List<string> blackNamespaceList = new List<string>()
        {
            "UnityEngineInternal",
            "UnityEngine.VR",
            "UnityEngine.WSA",
            "UnityEngine.Windows",
            "UnityEngine.Apple",
            "UnityEngine.Collections",
            "UnityEngine.Tizen",
            "UnityEngine.iOS",
            "UnityEngine.Experimental",
            "UnityEngine.Networking",
            "UnityEngine.AI",
            "UnityEngine.Rendering",
            "UnityEngine.Internal.VR",
            "Unity.Collections.LowLevel.Unsafe"
        };

    /// <summary>
    /// 黑名单:记录一些特定类型的成员
    /// </summary>
    public static List<List<string>> SpecialBlackTypeList = new List<List<string>>()
    {
        new List<string>() { "LCL.PrefabLightmapData", "SaveLightmap" },
        new List<string>() { "Input", "IsJoystickPreconfigured" },
        new List<string>() { "UnityEngine.MonoBehaviour", "runInEditMode" },
        new List<string>() { "UnityEngine.AudioSettings", "GetSpatializerPluginName" },
        new List<string>() { "UnityEngine.AudioSettings", "GetSpatializerPluginNames" },
        new List<string>() { "UnityEngine.AudioSettings", "SetSpatializerPluginName" },
        new List<string>(){"UnityEngine.UI.Graphic", "OnRebuildRequested"},
        new List<string>(){"UnityEngine.UI.Text", "OnRebuildRequested"},
        new List<string>(){"UnityEngine.WWW", "movie"},
        new List<string>(){ "UnityEngine.Texture", "imageContentsHash"},
#if UNITY_WEBGL
        new List<string>(){"UnityEngine.WWW", "threadPriority"},
#endif
        new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
        new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
        new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
        new List<string>(){"UnityEngine.Light", "areaSize"},
        new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
        new List<string>(){ "UnityEngine.WWWAudioExtensions", "GetMovieTexture"},
        new List<string>(){ "UnityEngine.Terrain", "bakeLightProbesForTrees"},
        new List<string>(){ "UnityEngine.Terrain", "bakeLightProbesForTrees"},
        new List<string>(){ "UnityEngine.AnimatorControllerParameter", "name"},
#if !UNITY_WEBPLAYER
        new List<string>(){"UnityEngine.Application", "ExternalEval"},
#endif
        new List<string>(){"UnityEngine.GameObject", "networkView"},
        new List<string>(){"UnityEngine.Component", "networkView"},
        new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"}
    };

    /// <summary>
    /// 黑名单类型(因平台差异，各有不同)
    /// </summary>
    public static List<Type> blackTypeList = new List<Type>()
    {

#if UNITY_EDITOR
            typeof(NavMeshTriangulation),
            typeof(UnityEngine.ClusterInput),
            typeof(UnityEngine.ClusterInputType),
            typeof(TextureCompressionQuality),


            typeof(UnityEngine.iOS.ActivityIndicatorStyle),
            typeof(Physics),
            typeof(Physics2D),
            typeof(PhysicsUpdateBehaviour2D),
            typeof(PhysicMaterialCombine),
            typeof(PhysicMaterial),
            typeof(PhysicsMaterial2D),
            typeof(ParticlePhysicsExtensions),

            typeof(GUI),
            typeof(GUIContent),
            typeof(GUIElement),
            typeof(GUILayer),
            typeof(GUILayoutOption),
            typeof(GUILayoutUtility),
            typeof(GUISettings),
            typeof(GUISkin),
            typeof(GUIStyle),
            typeof(GUIStyleState),
            typeof(GUITargetAttribute),
            typeof(GUIText),
            typeof(GUITexture),
            typeof(GUIUtility),

            typeof(Graphics),

            typeof(Animator),

            typeof(Rigidbody),
            typeof(Rigidbody2D),
            typeof(RigidbodyConstraints),
            typeof(RigidbodyConstraints2D),
            typeof(RigidbodyInterpolation),
            typeof(RigidbodyInterpolation2D),
            typeof(RigidbodySleepMode2D),
            typeof(RigidbodyType2D),

            typeof(Terrain),
            typeof(TerrainChangedFlags),
            typeof(TerrainCollider),
            typeof(TerrainData),
            typeof(TerrainExtensions),
            typeof(TerrainRenderFlags),
#if UNITY_STANDALONE
            typeof(UnityEngine.FullScreenMovieControlMode),
            typeof(UnityEngine.FullScreenMovieScalingMode),
            typeof(UnityEngine.AndroidActivityIndicatorStyle),
            typeof(UnityEngine.AndroidInput),
            typeof(UnityEngine.AndroidJavaClass),
            typeof(UnityEngine.AndroidJavaException),
            typeof(UnityEngine.AndroidJavaObject),
            typeof(UnityEngine.AndroidJavaProxy),
            typeof(UnityEngine.AndroidJavaRunnable),
            typeof(UnityEngine.AndroidJNI),
            typeof(UnityEngine.AndroidJNIHelper),
            typeof(UnityEngine.TouchScreenKeyboard),
            typeof(UnityEngine.TouchScreenKeyboardType),
            typeof(iPhoneSettings),
#elif UNITY_ANDROID || UNITY_IOS
            typeof(UnityEngine.FullScreenMovieControlMode),
            typeof(UnityEngine.FullScreenMovieScalingMode),
            typeof(UnityEngine.AndroidActivityIndicatorStyle),
            typeof(UnityEngine.AndroidInput),
            typeof(UnityEngine.AndroidJavaClass),
            typeof(UnityEngine.AndroidJavaException),
            typeof(UnityEngine.AndroidJavaObject),
            typeof(UnityEngine.AndroidJavaProxy),
            typeof(UnityEngine.AndroidJavaRunnable),
            typeof(UnityEngine.AndroidJNI),
            typeof(UnityEngine.AndroidJNIHelper),
            typeof(UnityEngine.TouchScreenKeyboard),
            typeof(UnityEngine.TouchScreenKeyboardType),
            typeof(iPhoneSettings),
            typeof(UnityEngine.MovieTexture),
            typeof(TizenActivityIndicatorStyle),

#endif
            typeof(UnityEngine.EventProvider),
            typeof(UnityEngine.ClusterNetwork),
            typeof(UnityEngine.MovieTexture),

#if UNITY_2017 || UNITY_2018
            typeof(UnityEngine.Playables.PlayableBehaviour),

#else
            typeof(UnityEngine.SamsungTV),                        
            typeof(ConstructorSafeAttribute), 

            typeof(ThreadSafeAttribute),
#endif
            typeof(UnityEngine.UI.GraphicRebuildTracker),

            typeof(UnityEngine.TerrainData),
            typeof(SphericalHarmonicsL2),


            typeof(UnityEngine.GUIStyleState),
            typeof(UnityEngine.Handheld),
            typeof(UnityEngine.Caching),
            typeof(UnityEngine.iPhoneUtils),


            
#endif

    };
    #endregion
}
