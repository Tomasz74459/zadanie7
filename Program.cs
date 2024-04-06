using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string sourceFilePath = "sourceFile.txt";
        GenerateLargeFile(sourceFilePath, 300);

        TestCopyMethod("Metoda FileStream", sourceFilePath, "fileStreamCopy.txt", CopyUsingFileStream);
        TestCopyMethod("Metoda File.Copy", sourceFilePath, "fileCopy.txt", CopyUsingFileCopy);
        TestCopyMethod("Metoda File.ReadAllBytes i File.WriteAllBytes", sourceFilePath, "readWriteBytesCopy.txt", CopyUsingReadAllBytesWriteAllBytes);

        File.Delete(sourceFilePath);

        Console.ReadLine();
    }

    static void GenerateLargeFile(string filePath, int sizeInMB)
    {
        byte[] buffer = new byte[1024 * 1024];
        Random rand = new Random();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            for (int i = 0; i < sizeInMB; i++)
            {
                rand.NextBytes(buffer);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }

    static void TestCopyMethod(string methodName, string sourceFilePath, string targetFilePath, Action<string, string> copyMethod)
    {
        Console.WriteLine($"Testowanie: {methodName}");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        copyMethod(sourceFilePath, targetFilePath);

        stopwatch.Stop();
        Console.WriteLine($"Czas wykonania: {stopwatch.ElapsedMilliseconds} ms");
    }

    static void CopyUsingFileStream(string sourceFilePath, string targetFilePath)
    {
        using (FileStream sourceStream = File.OpenRead(sourceFilePath))
        {
            using (FileStream targetStream = File.Create(targetFilePath))
            {
                sourceStream.CopyTo(targetStream);
            }
        }
    }

    static void CopyUsingFileCopy(string sourceFilePath, string targetFilePath)
    {
        File.Copy(sourceFilePath, targetFilePath);
    }

    static void CopyUsingReadAllBytesWriteAllBytes(string sourceFilePath, string targetFilePath)
    {
        byte[] data = File.ReadAllBytes(sourceFilePath);
        File.WriteAllBytes(targetFilePath, data);
    }
}