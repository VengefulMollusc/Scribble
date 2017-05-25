using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

[Serializable]
[XmlRoot("Gesture")]
public struct Template
{
    [XmlAttribute("Name")]
    public string name;

    [XmlElement("Point")]
    public List<Point> points;
}

[Serializable]
public struct Point
{
    [XmlAttribute("X")]
    public int x;

    [XmlAttribute("Y")]
    public int y;
}

public class TemplateLoader : MonoBehaviour
{
    [SerializeField]
    private string xmlPath;

    private List<Template> templateList;

    public List<StrokePath> LoadTemplates()
    {
        templateList = new List<Template>();

        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.dataPath, xmlPath));
        FileInfo[] files = dir.GetFiles("*.xml");

        foreach (FileInfo f in files)
        {
            var xmlSerialiser = new XmlSerializer(typeof(Template));
            var stream = File.Open(f.FullName, FileMode.Open);
            Template deserialisedTemplate = (Template)xmlSerialiser.Deserialize(stream);

            stream.Close();

            templateList.Add(deserialisedTemplate);
        }

        return ConvertTemplates();

        //for (int i = 0; i < deserialisedTemplate.points.Count; i++)
        //{
        //    Template currentTemplate = deserialisedTemplate.points[i];
        //    templateList.Add(currentTemplate);
        //}
    }

    /*
     * Converts templates from XML struct format to StrokePaths
     */
    private List<StrokePath> ConvertTemplates()
    {
        List<StrokePath> convertedTemplates = new List<StrokePath>();

        foreach (Template t in templateList)
        {
            List<Vector2> rawPoints = new List<Vector2>();

            foreach (Point p in t.points)
            {
                rawPoints.Add(new Vector2(p.x, p.y));
                //Debug.Log("X = " + p.x + " Y = " + p.y);
            }

            string name = t.name;
            //string name = t.name.Substring(0, t.name.Length - 2);
            StrokePath newPath = new StrokePath(name, rawPoints);
            convertedTemplates.Add(newPath);
        }

        return convertedTemplates;
    }
}

//[XmlRoot("Gesture"), XmlType("Point")]
//public class TemplateLoader
//{

//    [XmlArray("Questions")]
//    [XmlArrayItem("Question")]
//    public List<Montage> questions = new List<Montage>();

//    public static ConfigScene Load(string path)
//    {
//        try
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(ConfigScene));
//            using (FileStream stream = new FileStream(path, FileMode.Open))
//            {
//                return serializer.Deserialize(stream) as ConfigScene;
//            }
//        }
//        catch (Exception e)
//        {
//            UnityEngine.Debug.LogError("Exception loading config file: " + e);

//            return null;
//        }
//    }
//}