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
    public class LineController : TableController<Line>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Line>(context, Request);
        }

        // GET tables/Line
        public IQueryable<Line> GetAllLine()
        {
            return Query(); 
        }

        // GET tables/Line/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Line> GetLine(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Line/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Line> PatchLine(string id, Delta<Line> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Line
        public async Task<IHttpActionResult> PostLine(Line item)
        {
            Line current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Line/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteLine(string id)
        {
             return DeleteAsync(id);
        }
    }
}
