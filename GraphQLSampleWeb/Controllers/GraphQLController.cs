using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Types;
using GraphQL;
using GraphQLSampleWeb.GraphQL;

namespace GraphQLSampleWeb.Controllers
{
    [Produces("application/json")]
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var schema = Schema.For(@"
    type Query {
        user: Person
    }

    type Person {
        id: String
        name: String
    }
");
            var root = new { User = new { Id = "1", Name = "Taro" } };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.Root = root;
            }).ConfigureAwait(false);

            return Ok(result);
        }
    }
}