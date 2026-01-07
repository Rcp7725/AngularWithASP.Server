using AngularWithASP.Server.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularWithASP.Server.Controllers
{
    //[Route("api/[controller]")]
    [Route("/treenode/[controller]")]
    [ApiController]
    public class TreeViewController : ControllerBase
    {
        //// GET: api/<TreeViewController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}// GET: api/<TreeViewController>
        [HttpGet]
        public string Get()
        {
            TreeModel treeModel = DataTreeFix.CreatestaticModel();
            string result = DataTreeFix.Get();

            return result;
        }

        // GET api/<TreeViewController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TreeViewController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TreeViewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TreeViewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
