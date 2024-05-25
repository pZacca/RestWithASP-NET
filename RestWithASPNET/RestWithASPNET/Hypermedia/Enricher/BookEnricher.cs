using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Constants;
using System.Text;

namespace RestWithASPNET.Hypermedia.Enricher
{
    public class BookEnricher : ContentResponseEnricher<BookVO>
    {
        protected override Task EnrichModel(BookVO content, IUrlHelper urlHelper)
        {
            var path = "api";
            string linkWithId = getLink(content.Id, urlHelper, path, "book");
            string linkWithoutId = getLink(null, urlHelper, path, "book");

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = linkWithId,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.POST,
                Href = linkWithoutId,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = linkWithoutId,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPut
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = linkWithId,
                Rel = RelationType.self,
                Type = "int"
            });
            return Task.CompletedTask;
        }

        private string getLink(long? id, IUrlHelper urlHelper, string path, string type)
        {
            lock (this)
            {
                var url = new { controller = path, type, id };
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F", "/").ToString();
            }
        }
    }
}
