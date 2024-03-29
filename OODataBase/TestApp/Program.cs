﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using OODataBase_ClassLibrary;
using System.ComponentModel;
using System.IO;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Multiple users test");
            Console.WriteLine("2. Speed test");
            Console.WriteLine("0. Exit");

            int answer = Convert.ToInt32(Console.ReadLine());
            
            if(answer == 1)
            {
                List<Thread> threads = new List<Thread>();

                for (int i = 0; i < 25; i++)
                {
                    threads.Add(new Thread(StartTest1));
                }
                for (int i = 0; i < 25; i++)
                {
                    threads.Add(new Thread(StartTest2));
                }
                for (int i = 0; i < 25; i++)
                {
                    threads.Add(new Thread(StartTest3));
                }
                for (int i = 0; i < 25; i++)
                {
                    threads.Add(new Thread(StartTest4));
                }


                foreach (var t in threads)
                {
                    t.Start();
                }

                foreach (var t in threads)
                {
                    t.Join();
                }

                Console.WriteLine("\nPress any key to exit...");
            }
            else if(answer == 2)
            {
                TestOperationsSpeed();

                //aaa a = new aaa();
                //a.a = 10;
                //a.aa = "aa";

                //bbb b = new bbb();
                //b.b = a;
                //b.bb = 20;

                //Transaction t = new Transaction();

                //t.Begin();

                //t.Create("bbb", b);
                //t.Update("bbb", new bbb() { b = new aaa(), bb = 25, ID = 0});
                //t.Commit();
            }
            
            Console.ReadLine();
        }


        // Tests

        public static void StartTest1()
        {
            Transaction trans1 = new Transaction();

            List<object> selectList;
            string selectItem;
            string selectProperty;
            string selectValue;
            int selectVersion;

            trans1.Begin();

            selectItem = "Cooker";
            selectProperty = "";
            selectValue = "";
            selectVersion = 0;

            selectList = trans1.Select(selectItem, selectProperty, selectValue, selectVersion);
            ShowSelectedItems(selectList, selectItem, selectProperty, selectValue, selectVersion);

            if (trans1.Create("Cooker", new Cooker() { Price = 64199, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");
            if (trans1.Create("Cooker", new Cooker() { Price = 55783, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");
            if (trans1.Create("Cooker", new Cooker() { Price = 47255, Brand = "Vox", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" }))
                Console.WriteLine("Created");

            if (trans1.Create("Laptop", new Laptop() { Price = 61990, BatteryCapacity = 123, Brand = "Dell", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16 }))
                Console.WriteLine("Created");
            if (trans1.Create("Laptop", new Laptop() { Price = 58481, BatteryCapacity = 123, Brand = "Lenovo", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16 }))
                Console.WriteLine("Created");

            if (trans1.Create("Smart", new Smart() { Price = 22372, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi", MicrophoneSensitivity = "good", OS = "Android 9 Pie", SpeakerVolume = 123, WiFiType = "5 GHz" }))
                Console.WriteLine("Created");


            selectItem = "";
            selectProperty = "";
            selectValue = "";
            selectVersion = Int32.MaxValue;

            selectList = trans1.Select(selectItem, selectProperty, selectValue, selectVersion);
            ShowSelectedItems(selectList, selectItem, selectProperty, selectValue, selectVersion);


            if (trans1.Update("Cooker", new Cooker() { Price = 62000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update("Cooker", new Cooker() { Price = 54000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update("Cooker", new Cooker() { Price = 51000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 0 }))
                Console.WriteLine("Updated");

            if (trans1.Update("Laptop", new Laptop() { Price = 61000, BatteryCapacity = 456, Brand = "Dell", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16, ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update("Laptop", new Laptop() { Price = 61000, BatteryCapacity = 456, Brand = "Lenovo", KeyboardType = "fulsize", Processor = "i5", RAM = 8, Resolution = "1920x1080", ROM = 256, ScreenSize = 16, ID = 1 }))
                Console.WriteLine("Updated");

            if (trans1.Update("Smart", new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi", MicrophoneSensitivity = "better", OS = "Android 9 Pie", SpeakerVolume = 123, WiFiType = "5 GHz", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update("Smart", new Smart() { Price = 21000, ScreenSize = 5, RAM = 6, ROM = 64, Resolution = "1920x1080", Brand = "Xiaomi", MicrophoneSensitivity = "the best", OS = "Android 10 Quince", SpeakerVolume = 123, WiFiType = "5 GHz", ID = 0 }))
                Console.WriteLine("Updated");


            selectItem = "Cooker";
            selectProperty = "";
            selectValue = "";
            selectVersion = 2;

            selectList = trans1.Select(selectItem, selectProperty, selectValue, selectVersion);
            ShowSelectedItems(selectList, selectItem, selectProperty, selectValue, selectVersion);


            trans1.Commit();
        }

        public static void StartTest2()
        {
            Transaction trans1 = new Transaction();

            List<object> selectList;
            string selectItem;
            string selectProperty;
            string selectValue;
            int selectVersion;


            trans1.Begin();

            selectItem = "AirConditioner";
            selectProperty = "Price";
            selectValue = "56000";
            selectVersion = Int32.MaxValue;

            selectList = trans1.Select(selectItem, selectProperty, selectValue, selectVersion);
            ShowSelectedItems(selectList, selectItem, selectProperty, selectValue, selectVersion);

            if (trans1.Create("AirConditioner", new AirConditioner() { Price = 56000, Brand = "LG", EnergyClass = "A++", CoolingCapacity = 4, NoiseLevel = 41, MinCoolingTemperature = 16 }))
                Console.WriteLine("Created");
            if (trans1.Create("AirConditioner", new AirConditioner() { Price = 56000, Brand = "Samsung", EnergyClass = "A+", CoolingCapacity = 4, NoiseLevel = 56, MinCoolingTemperature = 15 }))
                Console.WriteLine("Created");

            if (trans1.Create("Desktop", new Desktop() { Price = 265990, Brand = "Altos", PowerSupply = 700, Processor = "i7-8700K", RAM = 32, ROM = 2, Type = "Professional" }))
                Console.WriteLine("Created");
            if (trans1.Create("Desktop", new Desktop() { Price = 44990, Brand = "A-comp", PowerSupply = 500, Processor = "AMD Ryzen 3", RAM = 8, ROM = 1, Type = "Gaming" }))
                Console.WriteLine("Created");

            if (trans1.Create("Freezer", new Freezer() { Price = 56390, Brand = "Gorenje", EnergyClass = "A+", MinCoolingTemperature = -9, NoiseLevel = 40, Volume = 277 }))
                Console.WriteLine("Created");


            selectItem = "";
            selectProperty = "";
            selectValue = "";
            selectVersion = Int32.MaxValue;

            selectList = trans1.Select(selectItem, selectProperty, selectValue, selectVersion);
            ShowSelectedItems(selectList, selectItem, selectProperty, selectValue, selectVersion);


            if (trans1.Update("AirConditioner", new AirConditioner() { Price = 49000, Brand = "LG", EnergyClass = "A++", CoolingCapacity = 4, NoiseLevel = 41, MinCoolingTemperature = 16, ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update("AirConditioner", new AirConditioner() { Price = 47900, Brand = "LG", EnergyClass = "A++", CoolingCapacity = 4, NoiseLevel = 41, MinCoolingTemperature = 16, ID = 0 }))
                Console.WriteLine("Updated");

            if (trans1.Update("Desktop", new Desktop() { Price = 225990, Brand = "Altos", PowerSupply = 700, Processor = "i7-8700K", RAM = 32, ROM = 2, Type = "Professional", ID = 0 }))
                Console.WriteLine("Updated");
            if (trans1.Update("Desktop", new Desktop() { Price = 42990, Brand = "A-comp", PowerSupply = 500, Processor = "AMD Ryzen 3", RAM = 8, ROM = 1, Type = "Gaming", ID = 1 }))
                Console.WriteLine("Updated");


            selectItem = "";
            selectProperty = "";
            selectValue = "";
            selectVersion = Int32.MaxValue;

            selectList = trans1.Select(selectItem, selectProperty, selectValue, selectVersion);
            ShowSelectedItems(selectList, selectItem, selectProperty, selectValue, selectVersion);


            trans1.Commit();
        }

        public static void StartTest3()
        {
            Transaction trans1 = new Transaction();

            trans1.Begin();

            if (trans1.Create("Fridge", new Fridge() { Price = 27490, Brand = "Elin", EnergyClass = "A+", Volume = 175, Type = "Combined", MinCoolingTemperature = 5, NoiseLevel = 44 }))
                Console.WriteLine("Created");

            trans1.Commit();


            trans1.Begin();

            if (trans1.Create("Wireless", new Wireless() { Price = 9990, Brand = "Panasonic", BatteryCapacity = 500, MicrophoneSensitivity = "satisfying", Range = 50, SpeakerVolume = 19 }))
                Console.WriteLine("Created");

            trans1.Commit();


            trans1.Begin();

            if (trans1.Create("Microwave", new Microwave() { Price = 24990, Brand = "BOSCH", EnergyClass = "A++", NoiseLevel = 27, MaxTemperature = 100, Volume = 25, Managing = "Digital" }))
                Console.WriteLine("Created");

            trans1.Commit();
        }

        public static void StartTest4()
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


        public static void TestOperationsSpeed()
        {
            string className = "Oven";

            // empty Oven.xml for more precise test results
            StreamWriter streamWriter = new StreamWriter(className + ".xml");
            streamWriter.Close();
            File.Delete("Version.txt");

            TestCreateOperationSpeed();
            TestReadOperationSpeed();
            TestUpdateOperationSpeed();
            TestSelectOperationSpeed();
            TestDeleteOperationSpeed();

            Console.WriteLine("\nPress any key to exit...");
        }

        public static void TestCreateOperationSpeed()
        {
            Transaction transCRUD = new Transaction();
            var myStopwatch = System.Diagnostics.Stopwatch.StartNew();
            float sum = 0;
            long currentElapsedTime;
            float currentMinTime = 1000;
            float currentMaxTime = 0;

            Console.WriteLine("Testing time for creating an item...");

            for (int i = 0; i < 1000; i++)
            {
                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transCRUD.Begin();
                transCRUD.Create("Oven", new Oven() { Brand = "Samsung", Price = 176000, EnergyClass = "A++", MaxTemperature = 280, NoiseLevel = 123, Volume = 34 });
                transCRUD.Commit();
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                sum += currentElapsedTime;

                if(currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if(currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }
            }

            Console.WriteLine("Minimum time for creating an item is: " + currentMinTime + " milliseconds.");
            Console.WriteLine("Maximum time for creating an item is: " + currentMaxTime + " milliseconds.\n");
            Console.WriteLine("Average time for creating an item is: " + (sum / 1000).ToString() + " milliseconds.\n");
            Console.WriteLine("---------------------------------------------------\n");
        }

        public static void TestReadOperationSpeed()
        {
            Transaction transCRUD = new Transaction();
            var myStopwatch = System.Diagnostics.Stopwatch.StartNew();
            float sum = 0;
            long currentElapsedTime;
            float currentMinTime = 1000;
            float currentMaxTime = 0;

            Console.WriteLine("Testing time for reading an item...");

            for (int i = 0; i < 1000; i++)
            {
                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transCRUD.Begin();
                transCRUD.Read("Oven", 0, 0);
                transCRUD.Commit();
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }
            }

            Console.WriteLine("Minimum time for reading an item is: " + currentMinTime + " milliseconds.");
            Console.WriteLine("Maximum time for reading an item is: " + currentMaxTime + " milliseconds.\n");
            Console.WriteLine("Average time for reading an item is: " + (sum / 1000).ToString() + " milliseconds.\n");
            Console.WriteLine("---------------------------------------------------\n");
        }

        public static void TestUpdateOperationSpeed()
        {
            Transaction transCRUD = new Transaction();
            var myStopwatch = System.Diagnostics.Stopwatch.StartNew();
            float sum = 0;
            long currentElapsedTime;
            float currentMinTime = 1000;
            float currentMaxTime = 0;

            Console.WriteLine("Testing time for updating an item...");

            for (int i = 0; i < 1000; i++)
            {
                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transCRUD.Begin();
                transCRUD.Update("Oven", new Oven() { ID = 0, Brand = "Samsung", Price = 176000, EnergyClass = "A++", MaxTemperature = 280, NoiseLevel = 123, Volume = 34 });
                transCRUD.Commit();
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Updating an item need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if(currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }
            }

            Console.WriteLine("Minimum time for updating an item is: " + currentMinTime + " milliseconds.");
            Console.WriteLine("Maximum time for updating an item is: " + currentMaxTime + " milliseconds.\n");
            Console.WriteLine("Average time for updating an item is: " + (sum / 1000).ToString() + " milliseconds.\n");
            Console.WriteLine("---------------------------------------------------\n");
        }

        public static void TestDeleteOperationSpeed()
        {
            Transaction transCRUD = new Transaction();
            var myStopwatch = System.Diagnostics.Stopwatch.StartNew();
            float sum = 0;
            long currentElapsedTime;
            float currentMinTime = 1000;
            float currentMaxTime = 0;

            Console.WriteLine("Testing time for deleting an item...");

            for (int i = 0; i < 1000; i++)
            {
                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transCRUD.Begin();
                transCRUD.Delete("Oven", i, 0);
                transCRUD.Commit();
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Deleting an item need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }
            }

            Console.WriteLine("Minimum time for deleting an item is: " + currentMinTime + " milliseconds.");
            Console.WriteLine("Maximum time for deleting an item is: " + currentMaxTime + " milliseconds.\n");
            Console.WriteLine("Average time for deleting an item is: " + (sum / 1000).ToString() + " milliseconds.\n");
            Console.WriteLine("---------------------------------------------------\n");
        }

        public static void TestSelectOperationSpeed()
        {
            Transaction transSelect = new Transaction();
            var myStopwatch = System.Diagnostics.Stopwatch.StartNew();
            float sum = 0;
            long currentElapsedTime;
            float currentMinTime = 1000;
            float currentMaxTime = 0;

            Console.WriteLine("Testing time for selecting items...");

            for (int i = 0; i < 200; i++)
            {
                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transSelect.Begin();

                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transSelect.Select("", "", "", Int32.MaxValue);
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Selecting items need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }
                
            
                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transSelect.Select("Oven", "", "", Int32.MaxValue);
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Selecting items need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }


                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transSelect.Select("", "Price", "123", Int32.MaxValue);
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Selecting items need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }


                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transSelect.Select("", "Price", "176000", 0);
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Selecting items need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }


                myStopwatch = System.Diagnostics.Stopwatch.StartNew();
                transSelect.Select("Oven", "Price", "176000", 0);
                currentElapsedTime = myStopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Selecting items need " + currentElapsedTime.ToString() + " milliseconds to execute.");
                sum += currentElapsedTime;

                if (currentElapsedTime < currentMinTime)
                {
                    currentMinTime = currentElapsedTime;
                }
                if (currentElapsedTime > currentMaxTime)
                {
                    currentMaxTime = currentElapsedTime;
                }


                transSelect.Commit();
            }

            Console.WriteLine("Minimum time for selecting an item is: " + currentMinTime + " milliseconds.");
            Console.WriteLine("Maximum time for selecting an item is: " + currentMaxTime + " milliseconds.\n");
            Console.WriteLine("Average time for selecting items is: " + (sum / 1000).ToString() + " milliseconds.\n");
            Console.WriteLine("---------------------------------------------------\n");
        }

        public static void ShowSelectedItems(List<object> selectedItems, string sItem, string sProperty, string sValue, int sVersion)
        {
            string showSelected = "\n***************** SELECTED ITEMS *****************\n\n";

            if (sVersion == Int32.MaxValue)
            {
                showSelected += "***** " + sItem + " | " + sProperty + " | " + sValue + " | " + "MaxValue" + " *****" + "\n\n";
            }
            else
            {
                showSelected += "***** " + sItem + " | " + sProperty + " | " + sValue + " | " + sVersion + " *****" + "\n\n";
            }


            foreach (var item in selectedItems)
            {
                showSelected += "--- ";
                showSelected += item.GetType().ToString().Split('.').Last() + " ---\n";

                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                {
                    showSelected += descriptor.Name + " : " + descriptor.GetValue(item) + "\n";
                }

                showSelected += "\n\n";
            }

            Console.WriteLine(showSelected);
        }
    }
}
