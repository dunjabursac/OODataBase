using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using OODataBase_ClassLibrary;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 50; i++)
            {
                threads.Add(new Thread(StartTest));
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            //Thread t1 = new Thread(StartTest);
            //Thread t2 = new Thread(StartTest);
            //Thread t3 = new Thread(StartTest);
            //Thread t4 = new Thread(StartTest);
            //Thread t5 = new Thread(StartTest);
            //Thread t6 = new Thread(StartTest);

            //t1.Start();
            //t2.Start();
            //t3.Start();
            //t4.Start();
            //t5.Start();
            //t6.Start();

            //t1.Join();
            //t2.Join();
            //t3.Join();
            //t4.Join();
            //t5.Join();
            //t6.Join();


            Console.ReadLine();
        }

        public static void StartTest()
        {
            List<object> selectList;
            OODataBase_ClassLibrary.Transaction trans1 = new OODataBase_ClassLibrary.Transaction();

            
            trans1.Begin();
            selectList = trans1.Select("Cooker", "", "", Int32.MaxValue);

            if(trans1.Create(new OODataBase_ClassLibrary.Cooker() { Price = 60000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");
            if(trans1.Create(new OODataBase_ClassLibrary.Cooker() { Price = 60000, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");
            if(trans1.Create(new OODataBase_ClassLibrary.Cooker() { Price = 60000, Brand = "Vox", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");

            if(trans1.Create(new Laptop() { Price = 61000, BatteryCapacity = 123, Brand = "Dell", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16 }))
                Console.WriteLine("Created");
            if(trans1.Create(new Laptop() { Price = 61000, BatteryCapacity = 123, Brand = "Lenovo", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16 }))
                Console.WriteLine("Created");

            if(trans1.Create(new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi", MicrophoneSensitivity = "good", OS = "Android 9 Pie", SpeakerVolume = 123, WiFiType = "5 GHz" }))
                Console.WriteLine("Created");


            trans1.Select("", "", "", Int32.MaxValue);


            if (trans1.Update(new OODataBase_ClassLibrary.Cooker() { Price = 62000, Brand = "Gorenje_1", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");
            if(trans1.Update(new OODataBase_ClassLibrary.Cooker() { Price = 54000, Brand = "Gorenje_2", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");
            if(trans1.Update(new OODataBase_ClassLibrary.Cooker() { Price = 51000, Brand = "Gorenje_3", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");

            if(trans1.Update(new Laptop() { Price = 61000, BatteryCapacity = 456, Brand = "Dell_1", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16, ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update(new Laptop() { Price = 61000, BatteryCapacity = 456, Brand = "Lenovo_1", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16, ID = 1 }))
                Console.WriteLine("Updated");

            if(trans1.Update(new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi_1", MicrophoneSensitivity = "better", OS = "Android 9 Pie", SpeakerVolume = 123, WiFiType = "5 GHz", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update(new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi_2", MicrophoneSensitivity = "the best", OS = "Android 10 Quince", SpeakerVolume = 123, WiFiType = "5 GHz", ID = 0 }))
                Console.WriteLine("Updated");


            trans1.Select("", "", "", Int32.MaxValue);


            //if(trans1.Delete("Cooker", 1, 0))
            //    Console.WriteLine("Deleted");
            //if(trans1.Delete("Laptop", 1, 0))
            //    Console.WriteLine("Deleted");


            trans1.Select("", "", "", Int32.MaxValue);



            trans1.Commit();
        }

        public static void StartTest2()
        {
            OODataBase_ClassLibrary.Transaction trans1 = new OODataBase_ClassLibrary.Transaction();

            trans1.Begin();
            trans1.Delete("Cooker", 2, 0);
            trans1.Commit();

        }

        public static void Updating()
        {
            OODataBase_ClassLibrary.Transaction trans2 = new OODataBase_ClassLibrary.Transaction();

            trans2.Begin();
            trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 2 });
            trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 9999, PanelType = "flat", ID = 2 });
            trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 7777, PanelType = "flat", ID = 1 });
            trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 7777, PanelType = "flat", ID = 1 });
            trans2.Commit();
        }
    }
}
