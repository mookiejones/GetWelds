using GetWelds.Robots;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using GetWelds.ViewModels;

namespace GetWelds.Helpers
{
    public static class Serializer
    {
        public static void Serialize(ObservableCollection<AbstractRobot> robots, string path)
        {

            var serial = new XmlSerializer(robots.GetType());
            using (var stream = new StreamWriter(path))
                serial.Serialize(stream, robots);
        }
    }


    internal class SerializingClass
    {
        #region Constructor
        #endregion

        #region Properties
        #endregion

        #region Members
        #endregion


    }




    public class Style
    {
        public string Name { get; set; }
        public List<SerializedProgram> Programs { get; set; }
        public Style()
        {
            Programs = new List<SerializedProgram>();
        }
    }
    public class SerializedProgram
    {


        public string Name { get; set; }
        public List<Kuka.RobotWeld> Welds { get; set; }
        public SerializedProgram()
        {
            Welds = new List<Kuka.RobotWeld>();
        }
    }
    public class SerializeRobot
    {
        public string RobotName { get; set; }
        public string Process1 { get; set; }
        public string Process2 { get; set; }

        public List<SearchParam> Options { get; set; }
        public List<Tool> Tools { get; set; }

        public List<Zone> Zones { get; set; }
        public List<Kuka.RobotWeld> Welds { get; set; }


        public List<SerializedProgram> Styles { get; set; }

        public SerializeRobot()
        {
            Options = new List<SearchParam>();
            Tools = new List<Tool>();
            Welds = new List<Kuka.RobotWeld>();
            Zones = new List<Zone>();
        }



    }

    public class MySerializer : XmlSerializer
    {
        public MySerializer() { }

        public MySerializer(Type type, XmlAttributeOverrides overrides) : base(type, overrides) { }
        public MySerializer(Type type, Type[] extraTypes) : base(type, extraTypes) { }
        public MySerializer(Type type, string defaultNamepace) : base(type, defaultNamepace) { }

        public MySerializer(XmlTypeMapping xmlTypeMapping)
            : base(xmlTypeMapping)
        {

        }

        public MySerializer(Type type)
            : base(type)
        {

        }
        public override bool CanDeserialize(System.Xml.XmlReader xmlReader)
        {
            return base.CanDeserialize(xmlReader);
        }

        protected override XmlSerializationReader CreateReader()
        {
            return base.CreateReader();
        }

        protected override XmlSerializationWriter CreateWriter()
        {
            return base.CreateWriter();
        }


        protected override void Serialize(object o, XmlSerializationWriter writer)
        {
            base.Serialize(o, writer);
        }

        protected override object Deserialize(XmlSerializationReader reader)
        {
            return base.Deserialize(reader);
        }


    }
}
