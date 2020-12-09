using Authentification.API.Routes;
using Authentification.API.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authentification.Controllers
{
    [ApiController]
    public class GlossaryController : ControllerBase
    {
        private static List<GlossaryItem> Glossary = new List<GlossaryItem> {
            new GlossaryItem
            {
                Term= "Access Token",
                Definition = "A credential that can be used by an application to access an API. It informs the API that the bearer of the token has been authorized to access the API and perform specific actions specified by the scope that has been granted."
            },
            new GlossaryItem
            {
                Term= "JWT",
                Definition = "An open, industry standard RFC 7519 method for representing claims securely between two parties. "
            },
            new GlossaryItem
            {
                Term= "OpenID",
                Definition = "An open standard for authentication that allows applications to verify users are who they say they are without needing to collect, store, and therefore become liable for a user’s login information."
            }
        };

        
        [HttpGet(DefaultRoutes.Glossary.Main)]
        public ActionResult<List<GlossaryItem>> Get()
        {
            return Ok(Glossary);
        }

        [HttpGet(DefaultRoutes.Glossary.Term)]
        public ActionResult<GlossaryItem> Get(string term)
        {
            var glossaryItem = Glossary.Find(item =>
                    item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (glossaryItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(glossaryItem);
            }
        }

        [HttpPost(DefaultRoutes.Glossary.Main)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Post(GlossaryItem glossaryItem)
        {
            var existingGlossaryItem = Glossary.Find(item =>
                    item.Term.Equals(glossaryItem.Term, StringComparison.InvariantCultureIgnoreCase));

            if (existingGlossaryItem != null)
            {
                return Conflict("Cannot create the term because it already exists.");
            }
            else
            {
                Glossary.Add(glossaryItem);
                var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(glossaryItem.Term));
                return Created(resourceUrl, glossaryItem);
            }
        }

        [HttpPut(DefaultRoutes.Glossary.Main)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Put(GlossaryItem glossaryItem)
        {
            var existingGlossaryItem = Glossary.Find(item =>
            item.Term.Equals(glossaryItem.Term, StringComparison.InvariantCultureIgnoreCase));

            if (existingGlossaryItem == null)
            {
                return BadRequest("Cannot update a nont existing term.");
            }
            else
            {
                existingGlossaryItem.Definition = glossaryItem.Definition;
                return Ok();
            }
        }

        [HttpDelete(DefaultRoutes.Glossary.Main)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Delete(string term)
        {
            var glossaryItem = Glossary.Find(item =>
                   item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (glossaryItem == null)
            {
                return NotFound();
            }
            else
            {
                Glossary.Remove(glossaryItem);
                return NoContent();
            }
        }
    }
}
