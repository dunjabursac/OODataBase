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

            for (int i = 0; i < 15; i++)
            {
                threads.Add(new Thread(StartTest1));
            }
            for (int i = 0; i < 15; i++)
            {
                threads.Add(new Thread(StartTest2));
            }
            for (int i = 0; i < 15; i++)
            {
                threads.Add(new Thread(StartTest3));
            }
            for (int i = 0; i < 15; i++)
            {
                threads.Add(new Thread(StartTest4));
            }
            for (int i = 0; i < 15; i++)
            {
                threads.Add(new Thread(StartTest5));
            }


            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }


            Console.ReadLine();
        }


        // Tests

        public static void StartTest1()
        {
            List<object> selectList;
            Transaction trans1 = new Transaction();

            
            trans1.Begin();
            selectList = trans1.Select("Cooker", "", "", Int32.MaxValue);

            if(trans1.Create(new Cooker() { Price = 64199, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");
            if(trans1.Create(new Cooker() { Price = 55783, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");
            if(trans1.Create(new Cooker() { Price = 47255, Brand = "Vox", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");

            if(trans1.Create(new Laptop() { Price = 61990, BatteryCapacity = 123, Brand = "Dell", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16 }))
                Console.WriteLine("Created");
            if(trans1.Create(new Laptop() { Price = 58481, BatteryCapacity = 123, Brand = "Lenovo", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16 }))
                Console.WriteLine("Created");

            if(trans1.Create(new Smart() { Price = 22372, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi", MicrophoneSensitivity = "good", OS = "Android 9 Pie", SpeakerVolume = 123, WiFiType = "5 GHz" }))
                Console.WriteLine("Created");


            trans1.Select("", "", "", Int32.MaxValue);


            if (trans1.Update(new Cooker() { Price = 62000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");
            if(trans1.Update(new Cooker() { Price = 54000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");
            if(trans1.Update(new Cooker() { Price = 51000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");

            if(trans1.Update(new Laptop() { Price = 61000, BatteryCapacity = 456, Brand = "Dell", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16, ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update(new Laptop() { Price = 61000, BatteryCapacity = 456, Brand = "Lenovo", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16, ID = 1 }))
                Console.WriteLine("Updated");

            if(trans1.Update(new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi_1", MicrophoneSensitivity = "better", OS = "Android 9 Pie", SpeakerVolume = 123, WiFiType = "5 GHz", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update(new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi_2", MicrophoneSensitivity = "the best", OS = "Android 10 Quince", SpeakerVolume = 123, WiFiType = "5 GHz", ID = 0 }))
                Console.WriteLine("Updated");


            trans1.Select("", "", "", Int32.MaxValue);

            trans1.Commit();
        }

        public static void StartTest2()
        {
            List<object> selectList;
            Transaction trans1 = new Transaction();


            trans1.Begin();
            selectList = trans1.Select("AirConditioner", "Price", "56000", Int32.MaxValue);

            if (trans1.Create(new AirConditioner() { Price = 56000, Brand = "LG", EnergyClass = "A++", CoolingCapacity = 4, NoiseLevel = 41, MinCoolingTemperature = 16 })) 
                Console.WriteLine("Created");
            if (trans1.Create(new AirConditioner() { Price = 56000, Brand = "Samsung", EnergyClass = "A+", CoolingCapacity = 4, NoiseLevel = 56, MinCoolingTemperature = 15 }))
                Console.WriteLine("Created");

            if (trans1.Create(new Desktop() { Price = 265990, Brand = "Altos", PowerSupply = 700, Processor = "i7-8700K", RAM = 32, ROM = 2, Type = "Professional" }))
                Console.WriteLine("Created");
            if (trans1.Create(new Desktop() { Price = 44990, Brand = "A-comp", PowerSupply = 500, Processor = "AMD Ryzen 3", RAM = 8, ROM = 1, Type = "Gaming" }))
                Console.WriteLine("Created");

            if (trans1.Create(new Freezer() { Price = 56390, Brand = "Gorenje", EnergyClass = "A+", MinCoolingTemperature = -9, NoiseLevel = 40, Volume = 277}))
                Console.WriteLine("Created");


            trans1.Select("", "", "", Int32.MaxValue);


            if (trans1.Update(new AirConditioner() { Price = 49000, Brand = "LG", EnergyClass = "A++", CoolingCapacity = 4, NoiseLevel = 41, MinCoolingTemperature = 16, ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update(new AirConditioner() { Price = 47900, Brand = "LG", EnergyClass = "A++", CoolingCapacity = 4, NoiseLevel = 41, MinCoolingTemperature = 16, ID = 0 }))
                Console.WriteLine("Updated");

            if (trans1.Update(new Desktop() { Price = 225990, Brand = "Altos", PowerSupply = 700, Processor = "i7-8700K", RAM = 32, ROM = 2, Type = "Professional", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update(new Desktop() { Price = 42990, Brand = "A-comp", PowerSupply = 500, Processor = "AMD Ryzen 3", RAM = 8, ROM = 1, Type = "Gaming", ID = 1 }))
                Console.WriteLine("Updated");
            

            trans1.Select("", "", "", Int32.MaxValue);


            trans1.Commit();
        }

        public static void StartTest3()
        {
            Transaction trans2 = new Transaction();

            trans2.Begin();
            trans2.Update(new Cooker() { Price = 56890, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 2 });
            trans2.Update(new Cooker() { Price = 58122, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 9999, PanelType = "flat", ID = 2 });
            trans2.Update(new Cooker() { Price = 51260, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 7777, PanelType = "flat", ID = 1 });
            trans2.Update(new Cooker() { Price = 50970, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 7777, PanelType = "flat", ID = 1 });
            trans2.Commit();
        }

        public static void StartTest4()
        {
           Transaction trans1 = new Transaction();
            
            trans1.Begin();

            if (trans1.Create(new Fridge() { Price = 27490, Brand = "Elin", EnergyClass = "A+", Volume = 175, Type = "Combined", MinCoolingTemperature = 5, NoiseLevel = 44 }))
                Console.WriteLine("Created");

            trans1.Commit();


            trans1.Begin();

            if (trans1.Create(new Wireless() { Price = 9990, Brand = "Panasonic", BatteryCapacity = 500, MicrophoneSensitivity = "satisfying", Range = 50, SpeakerVolume = 19 }))
                Console.WriteLine("Created");

            trans1.Commit();


            trans1.Begin();

            if (trans1.Create(new Microwave() { Price = 24990, Brand = "BOSCH", EnergyClass = "A++", NoiseLevel = 27, MaxTemperature = 100, Volume = 25, Managing = "Digital" }))
                Console.WriteLine("Created");
            
            trans1.Commit();
        }
        
        public static void StartTest5()
        {
            // Thread.Sleep(5000);


            Transaction trans1 = new Transaction();

            trans1.Begin();

            if (trans1.Delete("Desktop", 4, 2))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Laptop", 1, 1))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Tablet", 1, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Cooker", 1, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Oven", 3, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Microwave", 3, 1))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Fridge", 2, 5))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Freezer", 0, 2))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("AirConditioner", 1, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("WashingMachine", 1, 4))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("DryingMachine", 1, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("AirConditioner", 1, 1))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Dishwasher", 3, 2))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Wire", 2, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Smart", 1, 0))
                Console.WriteLine("Deleted");

            trans1.Commit();

            trans1.Begin();

            if (trans1.Delete("Regular", 1, 3))
                Console.WriteLine("Deleted");

            trans1.Commit();
        }
    }
}
