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
    public class EagleBatchController : TableController<EagleBatch>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<EagleBatch>(context, Request, enableSoftDelete: true);
        }

        // GET tables/EagleBatch
        public IQueryable<EagleBatch> GetAllEagleBatch()
        {
            return Query(); 
        }

        // GET tables/EagleBatch/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<EagleBatch> GetEagleBatch(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/EagleBatch/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<EagleBatch> PatchEagleBatch(string id, Delta<EagleBatch> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/EagleBatch
        public async Task<IHttpActionResult> PostEagleBatch(EagleBatch item)
        {
            EagleBatch current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/EagleBatch/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEagleBatch(string id)
        {
             return DeleteAsync(id);
        }
    }
}
