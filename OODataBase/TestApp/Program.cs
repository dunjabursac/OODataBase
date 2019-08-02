using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t1 = Task.Factory.StartNew(StartTest);
            Task t2 = Task.Factory.StartNew(StartTest);
            Task t3 = Task.Factory.StartNew(StartTest);

            Task t4 = Task.Factory.StartNew(Updating);
            Task t5 = Task.Factory.StartNew(Updating);
            Task t6 = Task.Factory.StartNew(Updating);


            t1.Wait();
            t2.Wait();
            t3.Wait();

            t4.Wait();
            t5.Wait();
            t6.Wait();

            Console.ReadLine();
        }

        public static void StartTest()
        {
            List<object> selectList;
            OODataBase_ClassLibrary.Transaction trans1 = new OODataBase_ClassLibrary.Transaction();

            trans1.Begin();
            selectList = trans1.Select("Computer", "", "", 0);
            trans1.Commit();

            trans1.Begin();
            trans1.Create(new OODataBase_ClassLibrary.Cooker() { Price = 60000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" });
            selectList = trans1.Select("Cooker", "", "", Int32.MaxValue);
            trans1.Rollback();

            trans1.Begin();
            selectList = trans1.Select("Cooker", "", "", Int32.MaxValue);
            //trans1.Create(new OODataBase_ClassLibrary.Cooker() { Price = 60000, Brand = "Gorenje", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat" });
            trans1.Commit();
        }

        public static void Updating()
        {
            OODataBase_ClassLibrary.Transaction trans2 = new OODataBase_ClassLibrary.Transaction();

            trans2.Begin();
            trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 123, PanelType = "flat", ID = 2 });
            trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 9999, PanelType = "flat", ID = 2 });
            //trans2.Update(new OODataBase_ClassLibrary.Cooker() { Price = 5500, Brand = "Beko", EnergyClass = "A+", MaxTemperature = 220, NoiseLevel = 7777, PanelType = "flat", ID = 5 });
            trans2.Commit();
        }
    }
}
