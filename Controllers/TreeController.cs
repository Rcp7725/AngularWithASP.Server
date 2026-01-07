using AngularWithASP.Server.Core;
using AngularWithASP.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularWithASP.Server.Controllers
{

    [Route("/treenode/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        internal const string URI_TRENODE = "/treenode/Tree";
        private IDataManager dataManager;

        [HttpGet(URI_TRENODE)]
        public IActionResult GetDati()
        {
            if (this.dataManager == null)
                this.dataManager = DataManager.Instance;
            var data = new
            {
                message = "TreeModel",
                syncref = Guid.NewGuid().ToString(),
                model = this.dataManager.GetTreeModel() //new[] { "Item1", "Item2", "Item3" }
            };

            return Ok(data); // Restituisce un oggetto JSON
        }

        private void SetIsBusy(List<IMenuNodeModel> _childrenNodes)
        {
            if (_childrenNodes == null)
                return;
            Random random = new Random();
            foreach (MenuNodeModel item in _childrenNodes)
            {
                if (item.level == 3)
                {
                    //status = !status; 
                    //item.SetProperty(nameof(MenuNodeModel.isBusy), !item.isBusy);

                    byte randomNumber = (byte)random.Next(0, 3);
                    item.SetAlarm(randomNumber);
                    if ((randomNumber == 0) || (randomNumber == 2))
                        item.SetProperty(nameof(MenuNodeModel.isBusy), true);
                    else
                        item.SetProperty(nameof(MenuNodeModel.isBusy), false);

                }

                if (item.children != null && item.children.Count > 0)
                {
                    SetIsBusy(item.children);
                }
            }

        }

        [HttpGet("stream")]
        public async Task StreamUpdates()
        {
            Response.ContentType = "text/event-stream";

            for (int i = 0; i < 100; i++) // Simulazione di aggiornamenti
            {
                try
                {
                    if (this.dataManager == null)
                        this.dataManager = DataManager.Instance;

                    TreeModel model = this.dataManager.GetTreeModel();

                    foreach (var item in model.Data)
                    {
                        SetIsBusy(item.children);
                    }

                    var data = new
                    {
                        message = "TreeModel",
                        syncref = Guid.NewGuid().ToString(),
                        model = model //new[] { "Item1", "Item2", "Item3" }
                    };
                    var json = System.Text.Json.JsonSerializer.Serialize(data);
                    //await Response.WriteAsync($"data: {json}\n\n");
                    await Response.WriteAsync($"data: {json}\n\n");
                    await Response.Body.FlushAsync();
                }
                catch (Exception _ex)
                {
                    Console.WriteLine(_ex.ToString());
                }
                finally
                {
                    await Task.Delay(5000); // ogni 5 secondi
                }
            }
        }

        // POST api/<TreeController>
        [HttpPost(URI_TRENODE)]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{key}")]
        public IActionResult UpdateCheckboxSelection(string key, [FromBody] MenuNodeModel updatedItem)
        {
            // Trova il nodo corrispondente nel modello
            var model = DataManager.Instance.GetTreeModel();
            var node = FindNodeByKey(model.Data, key);

            if (node != null)
            {
                // Aggiorna lo stato del nodo
                (node as MenuNodeModel).SetProperty(nameof(MenuNodeModel.isBusy), updatedItem.Checked);
                //node.Checked = updatedItem.Checked;
                return Ok();
            }

            return NotFound();
        }

        private IMenuNodeModel? FindNodeByKey(List<IMenuNodeModel> nodes, string key)
        {
            foreach (var node in nodes)
            {
                if (node.Key == key)
                    return node;

                if (node.children != null)
                {
                    var childNode = FindNodeByKey(node.children, key);
                    if (childNode != null)
                        return childNode;
                }
            }
            return null;
        }
        //private IDataManager dataManager;

        ////public TreeController(IDataManager _dataManager)
        ////{
        ////    this.dataManager = _dataManager;
        ////}
        //// GET: api/<TreeController>
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    if (this.dataManager == null)
        //        this.dataManager = DataManager.Instance;

        //    var jsonString = JsonConvert.SerializeObject(this.dataManager.GetTreeModel());
        //    string[] result = new string[] { jsonString };
        //    return Ok(DataTreeFix.Get());
        //    //return Ok(this.dataManager.GetTreeModel());
        //    //string jsonFiles = JsonConvert.SerializeObject(this.dataManager.GetTreeModel());
        //    //HttpResponseMessage response = new HttpResponseMessage
        //    //{
        //    //    StatusCode = HttpStatusCode.OK,
        //    //    Content = new StringContent(jsonFiles)
        //    //};
        //    //response.Headers.Add("Key", "Value");

        //    //return Ok(response);
        //}

        //// GET api/<TreeController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return DataTreeFix.Get();
        //}

        //// POST api/<TreeController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<TreeController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TreeController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
