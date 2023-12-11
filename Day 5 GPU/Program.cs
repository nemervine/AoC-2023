using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ILGPU;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

/// <summary>
/// Example of using Interop.WriteLine within a kernel to display output.
/// </summary>
static void WriteLineKernel(Index1D index, ArrayView<int> dataView)
{
    // NB: String interpolation, alignment, spacing, format and precision
    // specifiers are not currently supported. Use standard {x} placeholders.
    Interop.WriteLine("{0} = {1}", index, dataView[index]);
}

static void MyKernel(
            Index1D index,             // The global thread index (1D in this case)
            ArrayView<int> dataView,   // A view to a chunk of memory (1D in this case)
            int constant)              // A sample uniform constant
{
    dataView[index] = index + constant;
}

static void PrintAcceleratorInfo(Accelerator accelerator)
{
    Console.WriteLine($"Name: {accelerator.Name}");
    Console.WriteLine($"MemorySize: {accelerator.MemorySize}");
    Console.WriteLine($"MaxThreadsPerGroup: {accelerator.MaxNumThreadsPerGroup}");
    Console.WriteLine($"MaxSharedMemoryPerGroup: {accelerator.MaxSharedMemoryPerGroup}");
    Console.WriteLine($"MaxGridSize: {accelerator.MaxGridSize}");
    Console.WriteLine($"MaxConstantMemory: {accelerator.MaxConstantMemory}");
    Console.WriteLine($"WarpSize: {accelerator.WarpSize}");
    Console.WriteLine($"NumMultiprocessors: {accelerator.NumMultiprocessors}");
}

using (var context = Context.CreateDefault())
{
    // For each available device...
    foreach (var device in context)
    {
        // Create accelerator for the given device.
        // Note that all accelerators have to be disposed before the global context is disposed
        using var accelerator = device.CreateAccelerator(context);
        Console.WriteLine($"Accelerator: {device.AcceleratorType}, {accelerator.Name}");
        PrintAcceleratorInfo(accelerator);
        Console.WriteLine();
    }
}

// CPU accelerators can also be created manually with custom settings.
// The following code snippet creates a CPU accelerator with 4 threads
// and highest thread priority.
using (var context = Context.Create(builder => builder.CPU(new CPUDevice(4, 1, 1))))
{
    using var accelerator = context.CreateCPUAccelerator(0, CPUAcceleratorMode.Auto, ThreadPriority.Highest);
    PrintAcceleratorInfo(accelerator);
}

//var debug = false;
//debug = true;
//string path = debug ? "Test.txt" : "Input.txt";
//var input = System.IO.File.ReadAllText(path);
//var mappings = new List<string>
//            {
//                "seed-to-soil map:",
//                "soil-to-fertilizer map:",
//                "fertilizer-to-water map:",
//                "water-to-light map:",
//                "light-to-temperature map:",
//                "temperature-to-humidity map:",
//                "humidity-to-location map:"
//            };

//var seeds = Regex.Matches(Regex.Match(input, "seeds\\:\\s(\\d+\\W*)*").Value, "\\d+\\s\\d+\\s").Select(x => Regex.Matches(x.Value, "\\d+").Select(y => float.Parse(y.Value)).ToArray()).OrderBy(x => x[0]).ToList();
//var seedsToCheck = new List<float> { };

//for (int i = 0; i < seeds.Count; i++)
//    seedsToCheck.AddRange(Enumerable.Range(0, Convert.ToInt32(seeds[i][1])).Select(y => y + seeds[i][0]));

//var maps = new List<List<AlmanacRange>>(7);
//for (var j = 0; j < mappings.Count(); j++)
//{
//    maps.Add(new List<AlmanacRange> { });
//    foreach (var s in Regex.Matches(Regex.Match(input, "(" + mappings[j] + "\\s)(((\\d*\\s*){3})+)").Value, "(\\s\\d+){3}").Select(x => Regex.Matches(x.Value, "\\d+").Select(y => float.Parse(y.Value)).ToArray()).ToList())
//    {
//        maps[j].Add(new AlmanacRange(s[1], s[0], s[2]));
//    }
//    maps[j] = maps[j].OrderBy(x => x.Source).ToList();
//}


//public struct AlmanacRange
//{
//    public float Source { get; set; }
//    public float Destination { get; set; }
//    public float Length { get; set; }

//    public AlmanacRange(float source, float destination, float length)
//    {
//        Source = source;
//        Destination = destination;
//        Length = length;
//    }
//}