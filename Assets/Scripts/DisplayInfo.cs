using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

public class DisplayInfo : MonoBehaviour
{
    public TextMeshProUGUI MemoryText;

    // Update is called once per frame
    void Update()
    {
        MemoryText.text = ("Total Memory: " + FormatSize(GetTotalPhys()) + "\n" + "Memory Used: " + FormatSize(GetUsedPhys()) + "\n" + "Memory Available: " + FormatSize(GetAvailPhys()));
    }

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength; //Current structure size
        public uint dwMemoryLoad; //Current memory utilization
        public ulong ullTotalPhys; //Total physical memory size
        public ulong ullAvailPhys; //Available physical memory size
        public ulong ullTotalPageFile; //Total Exchange File Size
        public ulong ullAvailPageFile; //Total Exchange File Size
        public ulong ullTotalVirtual; //Total virtual memory size
        public ulong ullAvailVirtual; //Available virtual memory size
        public ulong ullAvailExtendedVirtual; //Keep this value always zero
    }

    private static string FormatSize(double size)
    {
        double d = (double)size;
        int i = 0;
        while ((d > 1024) && (i < 5))
        {
            d /= 1024;
            i++;
        }
        string[] unit = { "B", "KB", "MB", "GB", "TB" };
        return (string.Format("{0} {1}", Math.Round(d, 2), unit[i]));
    }

    public static MEMORY_INFO GetMemoryStatus()
    {
        MEMORY_INFO mi = new MEMORY_INFO();
        mi.dwLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(mi);
        GlobalMemoryStatusEx(ref mi);
        return mi;
    }

    public static ulong GetAvailPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return mi.ullAvailPhys;
    }

    public static ulong GetUsedPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return (mi.ullTotalPhys - mi.ullAvailPhys);
    }

    public static ulong GetTotalPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return mi.ullTotalPhys;
    }
}