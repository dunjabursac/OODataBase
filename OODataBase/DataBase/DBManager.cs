using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DataBase
{
    public class DBManager
    {
        private static Dictionary<string, Dictionary<int, object>> Tables;
        private static Dictionary<string, Dictionary<int, List<object>>> TablesList;
        private static Dictionary<string, List<string>> ParentChildren;
        private static int Version = 1;


        // Singleton *****************************************************************************

        private static DBManager DBM_instance = null;

        private DBManager()
        {
            CreateTables();
        }

        public static DBManager DBM_Instance
        {
            get
            {
                if(DBM_instance == null)
                {
                    DBM_instance = new DBManager();
                }

                return DBM_instance;
            }
        }


        //private static readonly DBManager DBM_instance = new DBManager();

        //public static DBManager GetDBM_instance()
        //{
        //    return DBM_instance;
        //}

        // ***************************************************************************************

        private void CreateTables()
        {
            try
            {
                using (StreamReader stream = new StreamReader("Version.txt"))
                {
                    Version = Int32.Parse(stream.ReadLine());
                }
            }
            catch
            {
                WriteVersion(Version);
            }
            Tables = new Dictionary<string, Dictionary<int, object>>();
            TablesList = new Dictionary<string, Dictionary<int, List<object>>>();

            ParentChildren = new Dictionary<string, List<string>>();
            ParentChildren.Add("Item", new List<string>());

            XmlSchema myschema;

            try
            {
                XmlTextReader reader = new XmlTextReader("Model.xsd");
                myschema = XmlSchema.Read(reader, ValidationCallback);
                myschema.Write(Console.Out);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                myschema = null;
            }


            for (int i = 0; i < myschema.Items.Count; i++)
            {
                try
                {
                    if (((XmlSchemaElement)myschema.Items[i]).Name.Contains("Element"))
                    {
                        string className = ((XmlSchemaElement)myschema.Items[i]).SchemaTypeName.Name;

                        Tables.Add(className, new Dictionary<int, object>());
                        TablesList.Add(className, new Dictionary<int, List<object>>());
                        if (!File.Exists(className + ".xml"))
                        {
                            StreamWriter streamWriter = new StreamWriter(className + ".xml");
                            streamWriter.Close();
                        }
                    }
                }
                catch (Exception e1)
                {
                    // ucitavanje klasa iz XSD-a koje nisu listovi

                    try
                    {
                        string currwntClassName = ((XmlSchemaComplexType)myschema.Items[i]).Name;

                        Type type = Type.GetType($"DataBase.{currwntClassName}");
                        if (type != null && type.Name != "Item")
                        {
                            var currentClassInstance = Activator.CreateInstance(type);
                            var currentClassParentInstance = currentClassInstance.GetType().BaseType;//.GetGenericArguments()[0];
                            string currentClassParentName = currentClassParentInstance.Name;

                            if (ParentChildren.ContainsKey(currentClassParentName))
                            {
                                ParentChildren[currentClassParentName].Add(currwntClassName);
                            }
                            else
                            {
                                ParentChildren.Add(currentClassParentName, new List<string>());
                                ParentChildren[currentClassParentName].Add(currwntClassName);
                            }
                        }


                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine("Error:" + e2.Message);
                        continue;
                    }


                    Console.WriteLine("Error:" + e1.Message);
                    continue;
                }
            }

            TableFill();
        }

        void TableFill()
        {
            foreach (var item in TablesList)
            {
                using (var xml = new XmlTextReader(item.Key + ".xml"))
                {
                    Type t2 = Type.GetType("DataBase." + item.Key);
                    XmlSerializer xd = new XmlSerializer(t2);
                    try
                    {
                        xml.ReadStartElement("wrapper"); // skip the wrapper
                    }
                    catch
                    {
                        continue;
                    }
                    object obj;
                    int numOfTry = 0;

                    while (true)
                    {
                        obj = (object)xd.Deserialize(xml);
                        if (obj == null)
                        {
                            if (numOfTry == 1)
                            {
                                numOfTry = 0;
                                break;
                            }
                            //xml.Read();
                            numOfTry++;
                            continue;

                        }
                        //xml.Read();
                        if (obj != null)
                        {
                            numOfTry = 0;
                            if (!TablesList[item.Key].ContainsKey(((Item)obj).ID))
                            {
                                TablesList[item.Key].Add(((Item)obj).ID, new List<object>());
                            }

                            //Tables[item.Key].Add(((Item)obj).ID, obj);        //Prosla verzija
                            TablesList[item.Key][((Item)obj).ID].Insert(0, obj);
                        }
                    }
                }
            }

            SortTableList();
        }

        void SortTableList()
        {
            foreach (var item in TablesList)
            {
                Dictionary<int, List<object>> tmp = new Dictionary<int, List<object>>(item.Value);
                foreach (var value in tmp)
                {
                    TablesList[item.Key][value.Key] = value.Value.OrderByDescending(i => ((Item)i).Version).ToList();
                }
            }
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }

        public List<string> GetAllClassNames()
        {
            List<string> allClassNames = new List<string>();

            foreach(var cn in ParentChildren)
            {
                if(!allClassNames.Contains(cn.Key))
                {
                    allClassNames.Add(cn.Key);
                }

                foreach(var currentChild in cn.Value)
                {
                    if(!allClassNames.Contains(currentChild))
                    {
                        allClassNames.Add(currentChild);
                    }
                }
            }

            return allClassNames;
        }

        public Dictionary<string, List<string>> GetParentChildren()
        {
            return ParentChildren;
        }

        public List<string> GetLeavesName()
        {
            return new List<string>(Tables.Keys);
        }

        public bool Create(string name, object item, bool delOrUpd = false)
        {
            // Stara verzija
            //if (Tables[name].Count != 0)
            //{
            //    ((Item)item).ID = ((Item)Tables[name].Last().Value).ID + 1;
            //}
            //else
            //{
            //    ((Item)item).ID = 0;
            //}
            //Tables[name].Add(((Item)item).ID, item);

            if (TablesList[name].Count != 0)
            {
                ((Item)item).ID = ((Item)TablesList[name].Last().Value.FirstOrDefault()).ID + 1;
            }
            else
            {
                ((Item)item).ID = 0;
            }

            TablesList[name].Add(((Item)item).ID, new List<object>());
            TablesList[name][((Item)item).ID].Add(item);

            bool ret = true;
            string filename = name + ".xml";
            const string wrapperTagName = "wrapper";
            string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
            string wrapperEndTag = string.Format("</{0}>", wrapperTagName);

            using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                XmlWriter xml = null;
                Type t = Type.GetType("DataBase." + name);
                var serializer = new XmlSerializer(t);
                if (stream.Length == 0)
                {
                    xml = XmlWriter.Create(stream);
                    xml.WriteStartDocument();
                    xml.WriteRaw(wrapperStartTag);
                }
                else
                {
                    xml = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true });
                    var bufferLength = Encoding.UTF8.GetByteCount(wrapperEndTag);
                    var buffer = new byte[bufferLength];
                    stream.Position = stream.Length - bufferLength;
                    stream.Read(buffer, 0, bufferLength);
                    if (!Encoding.UTF8.GetString(buffer).StartsWith(wrapperEndTag))
                    {
                        ret = false;
                    }
                    else
                    {
                        stream.SetLength(stream.Length - bufferLength);
                    }
                }

                serializer.Serialize(xml, item);
                xml.WriteRaw(wrapperEndTag);
                xml.Close();
            }

            return ret;
        }

        public bool Delete(string name, int id, int version)
        {
            bool ret = true;

            //if (!Tables.ContainsKey(name))
            //{
            //    ret = false;
            //}
            //else if (!Tables[name].ContainsKey(id))
            //{
            //    ret = false;
            //}
            //else
            //{
            //    if (!Tables[name].Remove(id))
            //    {
            //        ret = false;
            //    }
            //}

            if (!TablesList.ContainsKey(name))
            {
                ret = false;
            }
            else if (!TablesList[name].ContainsKey(id))
            {
                ret = false;
            }
            else
            {
                object obj = TablesList[name][id].Find(i => ((Item)i).Version == version);
                if (!TablesList[name][id].Remove(obj))
                {
                    ret = false;
                }
            }

            StreamWriter streamWriter = new StreamWriter(name + ".xml");
            streamWriter.Close();
            //Tables[name] = new Dictionary<int, object>();

            //foreach (var item in Tables[name])
            //{
            //    Create(name, item.Value, true);
            //}

            RefreshXml(name);

            return ret;
        }

        public object Read(string name, int id, int version)
        {
            object ret = new object();

            //if (!Tables.ContainsKey(name))
            //{
            //    ret = null;
            //}
            //else if (!Tables[name].ContainsKey(id))
            //{
            //    ret = null;
            //}
            //else
            //{
            //    ret = (Tables[name])[id];
            //}

            if (!TablesList.ContainsKey(name))
            {
                ret = null;
            }
            else if (!TablesList[name].ContainsKey(id))
            {
                ret = null;
            }
            else
            {
                ret = TablesList[name][id].Find(i => ((Item)i).Version <= version);
            }

            return ret;
        }

        public bool Update(string name, int id, object obj)
        {
            bool ret = true;

            //if (!Tables.ContainsKey(name))
            //{
            //    ret = false;
            //}
            //else if (!Tables[name].ContainsKey(id))
            //{
            //    ret = false;
            //}
            //else
            //{
            //    ((Item)obj).ID = id;
            //    Tables[name][id] = obj;
            //}

            if (!TablesList.ContainsKey(name))
            {
                ret = false;
            }
            else if (!TablesList[name].ContainsKey(id))
            {
                ret = false;
            }
            else
            {
                ((Item)obj).ID = id;
                ((Item)obj).Version = Version;
                TablesList[name][id].Insert(0, obj);
                Version++;
                WriteVersion(Version);
            }

            StreamWriter streamWriter = new StreamWriter(name + ".xml");
            streamWriter.Close();

            //foreach (var item in Tables[name])
            //{
            //    Create(name, item.Value, true);
            //}

            RefreshXml(name);

            return ret;
        }

        public bool UpdateMultiple(List<string> names, List<int> ids, List<object> objects)
        {
            bool ret = true;

            if (names.Count == 0)
                ret = false;

            for (int i = 0; i < names.Count; i++)
            {
                if (!TablesList.ContainsKey(names[i]))
                {
                    ret = false;
                    break;
                }
                else if (!TablesList[names[i]].ContainsKey(ids[i]))
                {
                    ret = false;
                    break;
                }
            }

            if (ret)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    ((Item)objects[i]).ID = ids[i];
                    ((Item)objects[i]).Version = Version;
                    TablesList[names[i]][ids[i]].Insert(0, objects[i]);
                }
                Version++;
                WriteVersion(Version);
            }

            foreach (string name in names)
            {
                StreamWriter streamWriter = new StreamWriter(name + ".xml");
                streamWriter.Close();

                RefreshXml(name);
            }

            return ret;
        }
        
        public List<object> GetAllItems()
        {
            List<object> allItems = new List<object>();

            foreach(var kvp1 in TablesList)
            {
                foreach (var kvp2 in kvp1.Value)
                {
                    foreach(var listItem in kvp2.Value)
                    {
                        allItems.Add(listItem);
                    }
                }
            }

            return allItems;
        }

        void RefreshXml(string name)
        {
            //if (Tables[name].Count != 0)
            if (TablesList[name].Count != 0)
            {

                string filename = name + ".xml";
                const string wrapperTagName = "wrapper";
                string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
                string wrapperEndTag = string.Format("</{0}>", wrapperTagName);

                using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    XmlWriter xml = null;
                    Type t = Type.GetType("DataBase." + name);
                    var serializer = new XmlSerializer(t);
                    xml = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, CloseOutput = false });
                    //xml.WriteStartDocument();
                    xml.WriteRaw(wrapperStartTag);

                    foreach (var item in TablesList[name])
                    {
                        foreach (var value in item.Value)
                        {
                            serializer.Serialize(xml, value);
                            xml.Flush();
                        }
                    }

                    xml.WriteRaw(wrapperEndTag);
                    xml.Close();
                }
            }
        }

        void WriteVersion(int version)
        {
            using (StreamWriter stream = new StreamWriter("Version.txt", false))
            {
                stream.Write(version);
            }
        }
        
        public List<object> GetItemsByType(string choosenType)
        {
            List<object> allItems = GetAllItems();
            List<object> selectedItems = new List<object>();

            switch(choosenType)
            {
                case "Computer":
                    Computer currentComputer;
                    foreach(var item in allItems)
                    {
                        try
                        {
                            currentComputer = (Computer)item;
                            selectedItems.Add(currentComputer);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Appliances":
                    Appliances currentAppliance;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentAppliance = (Appliances)item;
                            selectedItems.Add(currentAppliance);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Phone":
                    Phone currentPhone;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentPhone = (Phone)item;
                            selectedItems.Add(currentPhone);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                // *********************************************************************************************************************************************

                case "Laptop":
                    Laptop currentLaptop;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentLaptop = (Laptop)item;
                            selectedItems.Add(currentLaptop);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Desktop":
                    Desktop currentDesktop;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentDesktop = (Desktop)item;
                            selectedItems.Add(currentDesktop);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Tablet":
                    Tablet currentTablet;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentTablet = (Tablet)item;
                            selectedItems.Add(currentTablet);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                case "CoolingDevice":
                    CoolingDevice currentCoolingDevice;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentCoolingDevice = (CoolingDevice)item;
                            selectedItems.Add(currentCoolingDevice);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Machine":
                    Machine currentMachine;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentMachine = (Machine)item;
                            selectedItems.Add(currentMachine);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Cooking":
                    Cooking currentCooking;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentCooking = (Cooking)item;
                            selectedItems.Add(currentCooking);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                case "Landline":
                    Landline currentLandline;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentLandline = (Landline)item;
                            selectedItems.Add(currentLandline);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Mobile":
                    Mobile currentMobile;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentMobile = (Mobile)item;
                            selectedItems.Add(currentMobile);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                // *********************************************************************************************************************************************

                case "AirConditioner":
                    AirConditioner currentAirConditioner;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentAirConditioner = (AirConditioner)item;
                            selectedItems.Add(currentAirConditioner);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Fridge":
                    Fridge currentFridge;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentFridge = (Fridge)item;
                            selectedItems.Add(currentFridge);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Freezer":
                    Freezer currentFreezer;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentFreezer = (Freezer)item;
                            selectedItems.Add(currentFreezer);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                case "WashingMachine":
                    WashingMachine currentWashingMachine;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentWashingMachine = (WashingMachine)item;
                            selectedItems.Add(currentWashingMachine);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "DryingMachine":
                    DryingMachine currentDryingMachine;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentDryingMachine = (DryingMachine)item;
                            selectedItems.Add(currentDryingMachine);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Dishwasher":
                    Dishwasher currentDishwasher;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentDishwasher = (Dishwasher)item;
                            selectedItems.Add(currentDishwasher);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                case "Cooker":
                    Cooker currentCooker;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentCooker = (Cooker)item;
                            selectedItems.Add(currentCooker);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Oven":
                    Oven currentOven;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentOven = (Oven)item;
                            selectedItems.Add(currentOven);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Microwave":
                    Microwave currentMicrowave;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentMicrowave = (Microwave)item;
                            selectedItems.Add(currentMicrowave);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                // *********************************************************************************************************************************************

                case "Wireless":
                    Wireless currentWireless;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentWireless = (Wireless)item;
                            selectedItems.Add(currentWireless);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Wire":
                    Wire currentWire;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentWire = (Wire)item;
                            selectedItems.Add(currentWire);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                case "Smart":
                    Smart currentSmart;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentSmart = (Smart)item;
                            selectedItems.Add(currentSmart);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;
                case "Regular":
                    Regular currentRegular;
                    foreach (var item in allItems)
                    {
                        try
                        {
                            currentRegular = (Regular)item;
                            selectedItems.Add(currentRegular);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    break;

                // *********************************************************************************************************************************************

                // *********************************************************************************************************************************************
                
                default:
                    selectedItems = allItems;
                    break;
            }

            return selectedItems;
        }

        public List<object> GetItemsByTypeAndVersion(string choosenType, int version)
        {
            List<object> itemsByType = GetItemsByType(choosenType);
            Dictionary<int, List<object>> versionsOfChoosenType = GetVersionsOfChoosenType(itemsByType);

            List<object> retList = new List<object>();

            foreach(var IdAndVersions in versionsOfChoosenType)
            {
                retList.Add(IdAndVersions.Value.Find(i => ((Item)i).Version <= version));
            }

            return retList;
        }

        public Dictionary<string, List<object>> GetDictionaryByLiefType(List<object> selectedItems)
        {
            Dictionary<string, List<object>> retDictionary = new Dictionary<string, List<object>>();
            string currentItemType = "";

            foreach(var item in selectedItems)
            {
                currentItemType = item.GetType().ToString().Split('.').Last();

                if(!retDictionary.ContainsKey(currentItemType))
                {
                    retDictionary.Add(currentItemType, new List<object>());
                }

                retDictionary[currentItemType].Add(item);
            }

            return retDictionary;
        }

        public Dictionary<int, List<object>> GetVersionsOfChoosenType(List<object> itemsByType)
        {
            Dictionary<int, List<object>> versionsOfChoosenType = new Dictionary<int, List<object>>();

            if(itemsByType.Count != 0)
            {
                int id = ((Item)itemsByType[0]).ID;

                versionsOfChoosenType.Add(id, new List<object>());

                foreach (object item in itemsByType)
                {
                    if (id != ((Item)item).ID)
                    {
                        id = ((Item)item).ID;

                        if (!versionsOfChoosenType.ContainsKey(((Item)item).ID))
                        {
                            versionsOfChoosenType.Add(id, new List<object>());
                        }
                    }

                    versionsOfChoosenType[id].Add(item);
                }
            }

            return versionsOfChoosenType;
        }

        public List<object> Select(string choosenType, string property, string value, int version)
        {
            List<object> itemsByType = GetItemsByType(choosenType);
            List<object> itemsByTypeAndProperty = new List<object>();
            
            Dictionary<int, List<object>> itemsByTypePropertyAndVersion = new Dictionary<int, List<object>>();
            Dictionary<string, List<object>> magicalDictionary;

            List<object> retList = new List<object>();

            if (property == "")
            {
                itemsByTypeAndProperty = itemsByType;


            }
            else
            {
                foreach (var item in itemsByType)
                {
                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                    {
                        if (descriptor.Name == property)
                        {
                            if (descriptor.GetValue(item).ToString() == value)
                            {
                                itemsByTypeAndProperty.Add(item);
                                break;
                            }
                        }
                    }
                }
            }

            magicalDictionary = GetDictionaryByLiefType(itemsByTypeAndProperty);
            foreach(var magical_kvp in magicalDictionary)
            {
                itemsByTypePropertyAndVersion = GetVersionsOfChoosenType(magical_kvp.Value);

                foreach (var kvp in itemsByTypePropertyAndVersion)
                {
                    retList.Add(kvp.Value.Find(i => ((Item)i).Version <= version));
                }
            }

            return retList;
        }
    }
}
