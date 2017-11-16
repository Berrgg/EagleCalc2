using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.DataObjects;
using Backend.Models;

namespace Backend.Controllers
{
    public class TestTableController : TableController<TestTable>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<TestTable>(context, Request);
        }

        // GET tables/TestTable
        public IQueryable<TestTable> GetAllTestTable()
        {
            return Query(); 
        }

        // GET tables/TestTable/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TestTable> GetTestTable(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TestTable/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TestTable> PatchTestTable(string id, Delta<TestTable> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TestTable
        public async Task<IHttpActionResult> PostTestTable(TestTable item)
        {
            TestTable current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TestTable/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTestTable(string id)
        {
             return DeleteAsync(id);
        }
    }
}
