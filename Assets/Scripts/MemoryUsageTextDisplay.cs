using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class MemoryUsageTextDisplay : MonoBehaviour
{
    public Text memoryUsageText; // 引用Text组件

    void Start()
    {
        if (memoryUsageText == null)
        {
            Debug.LogError("MemoryUsageText is not assigned. Please assign it in the inspector.");
        }
    }

    void Update()
    {
        if (memoryUsageText != null)
        {
            // 获取当前内存信息
            long totalMemory = System.GC.GetTotalMemory(false); // 获取托管堆上的总内存
            long allocatedMemory = Profiler.GetTotalAllocatedMemoryLong(); // 获取分配的总内存
            long reservedMemory = Profiler.GetTotalReservedMemoryLong(); // 获取保留的总内存
            long monoHeapSize = Profiler.GetMonoHeapSizeLong(); // 获取Mono堆的大小
            long monoUsedSize = Profiler.GetMonoUsedSizeLong(); // 获取Mono堆已使用的大小

            // 格式化显示信息
            string memoryInfo = $"Total Memory: {FormatBytes(totalMemory)}\n" +
                                $"Allocated Memory: {FormatBytes(allocatedMemory)}\n" +
                                $"Reserved Memory: {FormatBytes(reservedMemory)}\n" +
                                $"Mono Heap Size: {FormatBytes(monoHeapSize)}\n" +
                                $"Mono Used Size: {FormatBytes(monoUsedSize)}";

            // 更新Text组件内容
            memoryUsageText.text = memoryInfo;
        }
    }

    // 辅助方法，用于格式化字节数
    private string FormatBytes(long bytes)
    {
        if (bytes >= 1073741824)
            return (bytes / 1073741824f).ToString("F2") + " GB";
        else if (bytes >= 1048576)
            return (bytes / 1048576f).ToString("F2") + " MB";
        else if (bytes >= 1024)
            return (bytes / 1024f).ToString("F2") + " KB";
        else
            return bytes + " Bytes";
    }
}
