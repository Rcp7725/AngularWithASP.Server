using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AngularWithASP.Server.Models
{
    public interface IMenuNodeModel
    {
        public string Key { get; }
        public string label { get; }
        public List<IMenuNodeModel>? children { get; }
        public bool? expanded { get; }
        public bool? isSelectable { get; }
        public bool? isExpanded { get; }    
        public bool? isBusy { get; }
        public bool? isMicBusy { get; }
        public bool? Checked { get; }
        public string subsystemType { get; }
        public bool? isVisabe { get; }
        public int level { get; }
        public string? alarmColor { get; }
    }

    public class MenuNodeModel : IMenuNodeModel
    {
        public MenuNodeModel(string _key, string _label, List<IMenuNodeModel>? _children)
        {
            this.Key = _key;
            this.label = _label;
            this.children = _children;
        }

        [JsonPropertyName("key")]
        public string Key { get; }
        [JsonPropertyName("label")]
        public string label { get; }
        [JsonPropertyName("children")]
        public List<IMenuNodeModel>? children { get; private set; }
        [JsonPropertyName("expanded")]
        public bool? expanded { get; private set; }
        [JsonPropertyName("isSelectable")]
        public bool? isSelectable { get; private set; }
        [JsonPropertyName("isExpanded")]
        public bool? isExpanded { get; private set; }
        [JsonPropertyName("isBusy")]
        public bool? isBusy { get; private set; }
        [JsonPropertyName("isMicBusy")]
        public bool? isMicBusy { get; private set; }
        [JsonPropertyName("Checked")]
        public bool? Checked { get; private set; }
        [JsonPropertyName("subsystemType")]
        public string subsystemType { get; private set; } = "ALL";
        [JsonPropertyName("isVisabe")]
        public bool? isVisabe { get; private set; }
        [JsonPropertyName("level")]
        public int level { get; private set; }
        [JsonPropertyName("alarmColor")]
        public string? alarmColor { get; private set; } = "none";

        public bool SetProperty(string name, object value)
        {
            PropertyInfo? prop = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(this, value);
                return true;
            }
            return false;
        }

        public void SetAlarm(byte _level)
        {
            switch (_level)
            {
                case 1:
                    this.alarmColor = "red";
                    break;
                case 2:
                    this.alarmColor = "yellow";
                    break;
                case 3:
                    this.alarmColor = "orange";
                    break;
                case 4:
                    this.alarmColor = "none";
                    break;
                case 5:
                    this.alarmColor = "none";
                    break;
                case 6:
                    this.alarmColor = "none";
                    break;
                case 7:
                    this.alarmColor = "none";
                    break;
                case 8:
                    this.alarmColor = "none";
                    break;
                case 9:
                    this.alarmColor = "none";
                    break;
                case 10:
                    this.alarmColor = "none";
                    break;
                default:
                    this.alarmColor = "none";
                    break;
            }
        }
    }

    public class TreeModel
    {
        public TreeModel(List<IMenuNodeModel> _data)
        {
            this.Data = _data;
        }
        
        public TreeModel()
        {
        }
        [JsonPropertyName("data")]
        public List<IMenuNodeModel> Data { get; private set; } = new List<IMenuNodeModel>();
    }

    

    public static class DataTreeFix
    {
        private static List<IMenuNodeModel> CreateMenuModel(string _subsystemType, string _baseKey, string _desc)
        {
            List<IMenuNodeModel> menuNodes = new List<IMenuNodeModel>();
            for (int i = 1; i < 4; i++)
            {
                MenuNodeModel childrenNode = new MenuNodeModel($"{_baseKey}{i.ToString().PadLeft(2, '0')}", $"{_desc} {i.ToString().PadLeft(2, '0')}", null);
                (childrenNode as MenuNodeModel).SetProperty(nameof(MenuNodeModel.subsystemType), _subsystemType);
                menuNodes.Add(childrenNode);
            }

            return menuNodes;
        }

        public static TreeModel CreatestaticModel()
        {
            TreeModel treeModel = new TreeModel();

            // Aggiungi nodi statici al modello dell'albero DI TIPO STATION
            //TRAIN/COMMON/ TUNNEL
            List<IMenuNodeModel> childrenNodesST = new List<IMenuNodeModel>();
            List<IMenuNodeModel> childrenNodesTR = new List<IMenuNodeModel>();

            for (int i = 1; i < 4; i++)
            {
                List<IMenuNodeModel> subsystemList = new List<IMenuNodeModel>();
                subsystemList.AddRange(CreateMenuModel("ECP", $"STA{i.ToString().PadLeft(2, '0')}_ECP", "ECP "));
                subsystemList.AddRange(CreateMenuModel("PA", $"STA{i.ToString().PadLeft(2, '0')}_PA", "PA "));
                subsystemList.AddRange(CreateMenuModel("PID", $"STA{i.ToString().PadLeft(2, '0')}_PID", "PID "));
                subsystemList.AddRange(CreateMenuModel("CCTV", $"STA{i.ToString().PadLeft(2, '0')}_CCTV", "CCTV "));

                MenuNodeModel childrenNode = new MenuNodeModel($"STA{i.ToString().PadLeft(2, '0')}", $"Staion {i.ToString().PadLeft(2, '0')}", subsystemList);
                (childrenNode as MenuNodeModel).SetProperty(nameof(MenuNodeModel.subsystemType), "ALL");
                childrenNodesST.Add(childrenNode);

            }

            for (int i = 1; i < 4; i++)
            {
                List<IMenuNodeModel> subsystemList = new List<IMenuNodeModel>();
                subsystemList.AddRange(CreateMenuModel("ECP", $"TR{i.ToString().PadLeft(2, '0')}_ECP", "ECP "));
                subsystemList.AddRange(CreateMenuModel("PA", $"TR{i.ToString().PadLeft(2, '0')}_PA", "PA "));
                subsystemList.AddRange(CreateMenuModel("PID", $"TR{i.ToString().PadLeft(2, '0')}_PID", "PID "));
                subsystemList.AddRange(CreateMenuModel("CCTV", $"TR{i.ToString().PadLeft(2, '0')}_CCTV", "CCTV "));

                MenuNodeModel childrenNode = new MenuNodeModel($"TR{i.ToString().PadLeft(2, '0')}", $"Train {i.ToString().PadLeft(2, '0')}", subsystemList);
                (childrenNode as MenuNodeModel).SetProperty(nameof(MenuNodeModel.subsystemType), "ALL");
                childrenNodesTR.Add(childrenNode);

            }

            treeModel.Data.Add(new MenuNodeModel("ST", "Stations", childrenNodesST));
            treeModel.Data.Add(new MenuNodeModel("TR", "Trains", childrenNodesTR));
            treeModel.Data.Add(new MenuNodeModel("CM", "Commons", null));
            treeModel.Data.Add(new MenuNodeModel("TN", "Tunnel", null));

            SetLevel(treeModel.Data as List<IMenuNodeModel>, 1);

            return treeModel;
        }

        private static void SetLevel(List<IMenuNodeModel> _childrenNodes, int _level)
        {
            bool status = false;
            foreach (MenuNodeModel item in _childrenNodes)
            {
                item.SetProperty(nameof(MenuNodeModel.isBusy), status);
                status = !status;
                item.SetProperty(nameof(MenuNodeModel.level), _level);
                if(item.children != null && item.children.Count > 0)
                {
                    SetLevel(item.children, _level + 1);
                }
            }

        }

        public static string Get()
        {
            string result = "{'data': [\r\n  { subsystemType: 'ALL', key: '', label: 'Station', level: 1 },\r\n  {\r\n    subsystemType: 'ALL', key: '', label: 'Train', level: 1, children: [\r\n      {\r\n        subsystemType: 'ALL', key: '', label: 'Train 01', level: 2, alarmColor: '#ff0000', children: [\r\n          { level: 3, subsystemType: 'ECP', key: '', label: 'ECP TR01 CAR A 01', isSelectable: true, checked: 'OFF', alarmColor: '#f0fa0a' },\r\n          { level: 3, subsystemType: 'ECP', key: '', label: 'ECP TR01 CAR A 02', checked: 'OFF' },\r\n          { level: 3, subsystemType: 'ECP', key: '', label: 'ECP TR01 CAR B 03', isSelectable: true, checked: 'OFF', alarmColor: '#ff0000' },\r\n          { level: 3, subsystemType: 'ECP', key: '', label: 'ECP TR01 CAR B 04', checked: 'OFF', isBusy: true, alarmColor: '#f0fa0a' },\r\n          { level: 3, subsystemType: 'ECP', key: '', label: 'ECP TR01 CAR C 05', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'ECP', key: '', label: 'ECP TR01 CAR C 06', checked: 'OFF', isBusy: true },\r\n          { level: 3, subsystemType: 'CCTV', key: '', label: 'CCTV TR01 CAR A 01', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'CCTV', key: '', label: 'CCTV TR01 CAR A 02', checked: 'OFF' },\r\n          { level: 3, subsystemType: 'CCTV', key: '', label: 'CCTV TR01 CAR B 03', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'CCTV', key: '', label: 'CCTV TR01 CAR B 04', checked: 'OFF', isBusy: true },\r\n          { level: 3, subsystemType: 'CCTV', key: '', label: 'CCTV TR01 CAR C 05', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'CCTV', key: '', label: 'CCTV TR01 CAR C 06', checked: 'OFF', isBusy: true },\r\n          { level: 3, subsystemType: 'PA', key: '', label: 'PA TR01 CAR A 01', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'PA', key: '', label: 'PA TR01 CAR A 02', checked: 'OFF', alarmColor: '#f0fa0a' },\r\n          { level: 3, subsystemType: 'PA', key: '', label: 'PA TR01 CAR B 03', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'PA', key: '', label: 'PA TR01 CAR B 04', checked: 'OFF', isBusy: true },\r\n          { level: 3, subsystemType: 'PA', key: '', label: 'PA TR01 CAR C 05', isSelectable: true, checked: 'ON', alarmColor: '#ff0000' },\r\n          { level: 3, subsystemType: 'PA', key: '', label: 'PA TR01 CAR C 06', checked: 'OFF', isBusy: true },\r\n          { level: 3, subsystemType: 'PID', key: '', label: 'PID TR01 CAR A 01', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'PID', key: '', label: 'PID TR01 CAR A 02', checked: 'OFF' },\r\n          { level: 3, subsystemType: 'PID', key: '', label: 'PID TR01 CAR B 03', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'PID', key: '', label: 'PID TR01 CAR B 04', checked: 'OFF', isBusy: true },\r\n          { level: 3, subsystemType: 'PID', key: '', label: 'PID TR01 CAR C 05', isSelectable: true, checked: 'OFF' },\r\n          { level: 3, subsystemType: 'PID', key: '', label: 'PID TR01 CAR C 06', checked: 'OFF', isBusy: true }\r\n        ]\r\n      },\r\n      { subsystemType: 'ALL', key: '', label: 'Train 02', level: 2 },\r\n      { subsystemType: 'ALL', key: '', label: 'Train 03', level: 2 },\r\n      { subsystemType: 'ALL', key: '', label: 'Train 04', level: 2 }\r\n    ]\r\n  },\r\n  { subsystemType: 'ALL', key: '', label: 'Tunnel', level: 1 },\r\n  { subsystemType: 'ALL', key: '', label: 'Commons', level: 1 }\r\n]}";
            result = "'[\r\n  {\r\n    \"subsystemType\": \"ALL\",\r\n    \"key\": \"\",\r\n    \"label\": \"Station\",\r\n    \"level\": 1\r\n  },\r\n  {\r\n    \"subsystemType\": \"ALL\",\r\n    \"key\": \"\",\r\n    \"label\": \"Train\",\r\n    \"level\": 1,\r\n    \"children\": [\r\n      {\r\n        \"subsystemType\": \"ALL\",\r\n        \"key\": \"\",\r\n        \"label\": \"Train 01\",\r\n        \"level\": 2,\r\n        \"alarmColor\": \"#ff0000\",\r\n        \"children\": [\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"ECP\",\r\n            \"key\": \"\",\r\n            \"label\": \"ECP TR01 CAR A 01\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\",\r\n            \"alarmColor\": \"#f0fa0a\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"ECP\",\r\n            \"key\": \"\",\r\n            \"label\": \"ECP TR01 CAR A 02\",\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"ECP\",\r\n            \"key\": \"\",\r\n            \"label\": \"ECP TR01 CAR B 03\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\",\r\n            \"alarmColor\": \"#ff0000\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"ECP\",\r\n            \"key\": \"\",\r\n            \"label\": \"ECP TR01 CAR B 04\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true,\r\n            \"alarmColor\": \"#f0fa0a\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"ECP\",\r\n            \"key\": \"\",\r\n            \"label\": \"ECP TR01 CAR C 05\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"ECP\",\r\n            \"key\": \"\",\r\n            \"label\": \"ECP TR01 CAR C 06\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"CCTV\",\r\n            \"key\": \"\",\r\n            \"label\": \"CCTV TR01 CAR A 01\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"CCTV\",\r\n            \"key\": \"\",\r\n            \"label\": \"CCTV TR01 CAR A 02\",\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"CCTV\",\r\n            \"key\": \"\",\r\n            \"label\": \"CCTV TR01 CAR B 03\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"CCTV\",\r\n            \"key\": \"\",\r\n            \"label\": \"CCTV TR01 CAR B 04\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"CCTV\",\r\n            \"key\": \"\",\r\n            \"label\": \"CCTV TR01 CAR C 05\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"CCTV\",\r\n            \"key\": \"\",\r\n            \"label\": \"CCTV TR01 CAR C 06\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PA\",\r\n            \"key\": \"\",\r\n            \"label\": \"PA TR01 CAR A 01\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PA\",\r\n            \"key\": \"\",\r\n            \"label\": \"PA TR01 CAR A 02\",\r\n            \"checked\": \"OFF\",\r\n            \"alarmColor\": \"#f0fa0a\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PA\",\r\n            \"key\": \"\",\r\n            \"label\": \"PA TR01 CAR B 03\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PA\",\r\n            \"key\": \"\",\r\n            \"label\": \"PA TR01 CAR B 04\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PA\",\r\n            \"key\": \"\",\r\n            \"label\": \"PA TR01 CAR C 05\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"ON\",\r\n            \"alarmColor\": \"#ff0000\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PA\",\r\n            \"key\": \"\",\r\n            \"label\": \"PA TR01 CAR C 06\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PID\",\r\n            \"key\": \"\",\r\n            \"label\": \"PID TR01 CAR A 01\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PID\",\r\n            \"key\": \"\",\r\n            \"label\": \"PID TR01 CAR A 02\",\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PID\",\r\n            \"key\": \"\",\r\n            \"label\": \"PID TR01 CAR B 03\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PID\",\r\n            \"key\": \"\",\r\n            \"label\": \"PID TR01 CAR B 04\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PID\",\r\n            \"key\": \"\",\r\n            \"label\": \"PID TR01 CAR C 05\",\r\n            \"isSelectable\": true,\r\n            \"checked\": \"OFF\"\r\n          },\r\n          {\r\n            \"level\": 3,\r\n            \"subsystemType\": \"PID\",\r\n            \"key\": \"\",\r\n            \"label\": \"PID TR01 CAR C 06\",\r\n            \"checked\": \"OFF\",\r\n            \"isBusy\": true\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"subsystemType\": \"ALL\",\r\n        \"key\": \"\",\r\n        \"label\": \"Train 02\",\r\n        \"level\": 2\r\n      },\r\n      {\r\n        \"subsystemType\": \"ALL\",\r\n        \"key\": \"\",\r\n        \"label\": \"Train 03\",\r\n        \"level\": 2\r\n      },\r\n      {\r\n        \"subsystemType\": \"ALL\",\r\n        \"key\": \"\",\r\n        \"label\": \"Train 04\",\r\n        \"level\": 2\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"subsystemType\": \"ALL\",\r\n    \"key\": \"\",\r\n    \"label\": \"Tunnel\",\r\n    \"level\": 1\r\n  },\r\n  {\r\n    \"subsystemType\": \"ALL\",\r\n    \"key\": \"\",\r\n    \"label\": \"Commons\",\r\n    \"level\": 1\r\n  }\r\n]'" ;
            //result = result.Replace('\r', ' ');
            //result = result.Replace('\n', ' ');
            return result;
        }
        //private static string GetStaticModel()
        //{
        //    TreeModel treeModel = new TreeModel();
        //    treeModel.Data.Add(new MenuNodeModel("ST","Station", null));
        //    treeModel.Data.Add(new MenuNodeModel("TR", "Trains", null));
        //    treeModel.Data.Add(new MenuNodeModel("CM", "Commons", null));
        //    treeModel.Data.Add(new MenuNodeModel("TN", "Tunnel", null));

        //    string jsonString = JsonConvert.SerializeObject(treeModel.Data);
        //    return jsonString;
        //}
    }
}
