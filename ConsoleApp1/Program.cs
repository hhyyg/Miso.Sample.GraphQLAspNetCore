using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            My4();
        }

        static void My()
        {
            var schema = Schema.For(@"
    type Query {
        user: String
    }
");
            var root = new { User = "Taro" };
            string result = schema.Execute(_ =>
            {
                _.Query = @"{
    user
}";
                _.Root = root;
            });

            Console.WriteLine(result);
        }

        static void My2()
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
            string result = schema.Execute(_ =>
            {
                _.Query = @"{
    user {
        id
        name
    }
}";
                _.Root = root;
            });

            Console.WriteLine(result);
        }

        static void My3()
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
            string result = schema.Execute(_ =>
            {
                _.Query = @"{
    user {
        id
        name
    }
}";
                _.Root = root;
            });

            Console.WriteLine(result);
        }

        static void My4()
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
            string result = schema.Execute(_ =>
            {
                _.Query = @"{
  __schema {
    types {
      name
      kind
      description
      fields {
        name
      }
    }
  }
}";
                _.Root = root;
            });

            Console.WriteLine(result);
        }


        static void Sample1()
        {
            var schema = Schema.For(@"
  type Query {
    hello: String
  }
");
            var root = new { Hello = "Hello World!" };
            string result = schema.Execute(_ =>
            {
                _.Query = "{ hello }";
                _.Root = root;
            });

            Console.WriteLine(result);
        }

        static void Sample2()
        {

            var schema = Schema.For(@"
  type Droid {
    id: String
    name: String
  }

  type Query {
    hero: Droid
  }
", _ => {
                _.Types.Include<Query>();
            });

            string result = schema.Execute(_ =>
            {
                _.Query = "{ hero { id name } }";
            });
            Console.WriteLine(result);
        }

        static void Sample3()
        {
            var schema = Schema.For(@"
  type Droid {
    id: String
    name: String
  }

  type Query {
    hero2(id: String): Droid
  }
", _ => {
                _.Types.Include<Query>();
            });

            string id = "123";
            var result = schema.Execute(_ =>
            {
                _.Query = $"{{ hero2(id: \"{id}\") {{ id name }} }}";
            });
            Console.WriteLine(result);
        }
    }

    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Query
    {
        private List<Droid> _droids = new List<Droid>
        {
            new Droid { Id = "123", Name = "R2-D2" }
        };

        [GraphQLMetadata("hero")]
        public Droid GetHero()
        {
            return new Droid { Id = "123", Name = "R2-D2" };
        }

        [GraphQLMetadata("hero2")]
        public Droid GetHero(string id)
        {
            return _droids.FirstOrDefault(x => x.Id == id);
        }
    }

}
