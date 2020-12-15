using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class YAMLParser
{
    public List<Gesture> DeserializeGestureDataset(string path)
    {
        string gestureConfigFile = readFile(@path); 

        IDeserializer deserializer = new DeserializerBuilder().Build();
        List<Gesture> gesturesDataset = deserializer.Deserialize<List<Gesture>>(gestureConfigFile);

        return gesturesDataset;
    }

    public string readFile(string path)
    {
        return System.IO.File.ReadAllText(@path);
        //".\Assets\Script\gestureConfiguration.yaml"
    }
}
