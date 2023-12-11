using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// The BackgroundColor of Camera should be set to Black with 0 alpha in order to make this transparent effect works
public class MyTransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
    
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    private struct MARGINS{
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cybottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_EXSTYLE = -20;

    const int WS_EX_LAYERED = 0x00080000;
    const int WS_EX_TRANSPARENT = 0x00000020;
    const uint LWA_COLORKEY = 0x00000001;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    static readonly IntPtr HWND_TOP = new IntPtr(0);
    private IntPtr hWnd;


    void Start()
    {
        // MessageBox(new IntPtr(0), "Hello World!", "Hello Dialog", 0);

        #if !UNITY_EDITOR
            hWnd = GetActiveWindow();

            MARGINS margins = new MARGINS { cxLeftWidth = -1};
            DwmExtendFrameIntoClientArea(hWnd, ref margins);

            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            // SetLayeredWindowAttributes(hWnd, 0, 0,LWA_COLORKEY);
            
            SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, 0); 
        #endif

        Application.runInBackground = true;
    }

    private void Update()
    {
        //Clickthrough是用偵測Collider的方式，對象要有裝collider才會算被點到
        SetClickthrough(Physics2D.OverlapPoint(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition()) == null);    
    }

    private void SetClickthrough(bool clickthrough)
    {
        if(clickthrough)
        {
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        }
        else
        {
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
        }
    }
}
