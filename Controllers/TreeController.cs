using AngularWithASP.Server.Core;
using AngularWithASP.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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
            TreeModel model = DataManager.Instance.GetTreeModel();

            try
            {
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
        }

        // POST api/<TreeController>
        [HttpPost(URI_TRENODE)]
        public void Post([FromBody] string value)
        {
            Console.WriteLine("");
        }

        private void SetChildrenChecked(IMenuNodeModel _node, bool _checked)
        {
            if (_checked == null)
                return;

            _node.SetProperty(nameof(MenuNodeModel.Checked), _checked);

            if (_node.children != null)
            {
                foreach (var item in _node.children)
                {
                    this.SetChildrenChecked(item, _checked);
                }
            }
        }

        [HttpPut("{key}")]
        public IActionResult UpdateCheckboxSelection(string key, [FromBody] UpdateNodeDto dto)
        {
            if (dto == null)
                return BadRequest();

            var node = DataManager.Instance.GetTreeModel(key);

            if (node != null)
            {
                if(dto.Checked != null)
                    this.SetChildrenChecked(node, (dto.Checked == "ON" ? true : false));
                if (dto.expanded != null)
                node.SetProperty(nameof(MenuNodeModel.expanded), dto.expanded);

                Task task = StreamUpdates();

                return Ok();
            }

            return NotFound();
        }
    }
}
