using AngularWithASP.Server.Models;
using System.Text.Json;

namespace AngularWithASP.Server.Core
{
    internal class DataManager : IDataManager
    {
        #region Attribute
        private TreeModel treeModel;
        private static volatile DataManager instance = null;
        private static object syncRoot = new object();
        #endregion

        #region Constructor
        private DataManager()
        {
            //string jsonString = DataTreeFix.Get();
            //List<MenuNodeModel> Data = JsonSerializer.Deserialize<List<MenuNodeModel>>(jsonString);
            this.treeModel = DataTreeFix.CreatestaticModel();
        }
        #endregion

        #region Private Methods

        #endregion

        #region public Methods
        public TreeModel GetTreeModel()
        {
            return this.treeModel;
        }
        #endregion

        #region properties


        public static DataManager Instance
        {
            get
            {
                // only create a new instance if one doesn't already exist.
                if (instance == null)
                {
                    // use this lock to ensure that only one thread can access
                    // this block of code at once.
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new DataManager();
                        }
                    }
                }
                // return instance where it was just created or already existed.
                return instance;
            }
        }
        #endregion

        #region Event/Delegate

        #endregion

    }
}
