using System.Runtime.InteropServices;

public class Platform
{
    [DllImport("__Internal")]
    private static extern bool IsMobile();
    
    public static bool isMobileBrowser()
    {
        #if UNITY_EDITOR
            return false; // value to return in Play Mode (in the editor)
        #elif UNITY_WEBGL
            return Platform.IsMobile(); // value based on the current browser
        #else
            return false; // value for builds other than WebGL
        #endif
    }
}
