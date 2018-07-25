using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace MNISTExtractor
{
    class Program
    {
        static readonly string DefaultOutputDirectory = "output\\";
        static void Main(string[] args)
        {
            string imagesFile = Input("Enter images filename: ");
            string labelsFile = Input("Enter labels filename: ");
            string outputDir = Input("Enter output directory (optional): ");
            if (string.IsNullOrWhiteSpace(outputDir)) outputDir = DefaultOutputDirectory;
            if (!File.Exists(imagesFile ) || !File.Exists(labelsFile))
            {
                Print("File(s) missing.");
                Pause();
                return;
            }
            BinaryReader imageReader = new BinaryReader(new FileStream(imagesFile, FileMode.Open));
            BinaryReader labelReader = new BinaryReader(new FileStream(labelsFile, FileMode.Open));
            if (ReadInt(imageReader) != 2051 || ReadInt(labelReader) != 2049)
            {
                Print("Invalid magic number.");
                Pause();
                imageReader.Dispose();
                labelReader.Dispose();
                return;
            }
            int count1 = ReadInt(imageReader);
            int count2 = ReadInt(labelReader);
            if (count1 != count2)
            {
                Print("Images and Labels count mismatched.");
                Pause();
                imageReader.Dispose();
                labelReader.Dispose();
                return;
            }
            Directory.CreateDirectory(outputDir);
            for (int i = 0; i <= 9; i++)
                Directory.CreateDirectory(Path.Combine(outputDir, i.ToString()));
            Print("Starting conversion...");
            int count = count1 = count2;
            int rows = ReadInt(imageReader);
            int columns = ReadInt(imageReader);
            for(int i = 0; i < count; i++)
            {
                byte label = labelReader.ReadByte();
                Bitmap image = new Bitmap(columns, rows);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                System.Drawing.Imaging.BitmapData data =
                    image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                IntPtr ptr = data.Scan0;
                int bytes = Math.Abs(data.Stride) * image.Height;
                byte[] rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
                for (int j = 0; j < rows * columns; j++)
                    rgbValues[j * 3] = rgbValues[j * 3 + 1] = rgbValues[j * 3 + 2] = imageReader.ReadByte();
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                image.UnlockBits(data);
                string name = Path.Combine(outputDir, label.ToString(), i.ToString() + ".png");
                image.Save(name);
                image.Dispose();
                Print(name);
            }
            Print("Conversion completed!");
            Pause();
        }
        static int ReadInt(BinaryReader reader)
        {
            byte[] value = reader.ReadBytes(4);
            if (BitConverter.IsLittleEndian) Array.Reverse(value);
            return BitConverter.ToInt32(value, 0);
        }
        static void Print(string message) => Console.WriteLine(message);
        static string Input(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
        static void Pause() => Console.ReadKey();
    }
}
