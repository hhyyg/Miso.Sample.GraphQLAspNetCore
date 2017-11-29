using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Type
{
    class Program2
    {
        public static void Main(string[] args)
        {
            Run().Wait();
        }

        private static async Task Run()
        {
            Console.WriteLine("Hello GraphQL!");

            var schema = new Schema { Query = new StarWarsQuery() };
            
            ExecutionResult result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = @"
                query {
                  hero {
                    id
                    name
                  }
                  note(id: ""2"") {
                    text
                  }
                  friends(group: ""A"") {
                    id
                    name
                  }
                }
              ";
            }).ConfigureAwait(false);

            var json = new DocumentWriter(indent: true).Write(result);

            Console.WriteLine(json);

            /*
             * {
  "data": {
    "hero": {
      "id": "123",
      "name": "R2-D2"
    },
    "note": {
      "text": "no.2 bbbbb"
    },
    "friends": [
      {
        "id": "1",
        "name": "Jack"
      }
    ]
  }
}
             */
        }
    }
    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType()
        {
            Field(x => x.Id).Description("The Id of the Droid.");
            Field(x => x.Name, nullable: true).Description("The name of the Droid.");
        }
    }

    public class Note
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class NoteType : ObjectGraphType<Note>
    {
        public NoteType()
        {
            Field(x => x.Id).Description("The Id");
            Field(x => x.Text, nullable: true).Description("The text");
        }
    }

    public class Friend
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Group { get; set; }
    }

    public class FriendType : ObjectGraphType<Friend>
    {
        public FriendType()
        {
            Field(x => x.Id).Description("The Id");
            Field(x => x.Name, nullable: true).Description("The name");
            Field(x => x.Group, nullable: true).Description("The group");
        }
    }

    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery()
        {
            var myData = new MyData();

            Field<DroidType>(
                "hero",
                resolve: context => myData.GetDroid()
            );

            Field<NoteType>(
                "note",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the note" }
                    ),
                resolve: context => myData.GetNotes(context.GetArgument<string>("id"))
                );

            Field<ListGraphType<FriendType>>(
                "friends",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "group", Description = "group val" }
                    ),
                resolve: context => myData.GetFriends(context.GetArgument<string>("group"))
                );
        }
    }

    public class MyData
    {
        private List<Droid> _droids = new List<Droid>
        {
            new Droid { Id = "123", Name = "R2-D2" },
            new Droid { Id = "456", Name = "R45-D456" }
        };

        private List<Note> _notes = new List<Note>
        {
            new Note { Id = "1", Text = "no.1 aaaaa"},
            new Note { Id = "2", Text = "no.2 bbbbb"},
        };

        private List<Friend> _friends = new List<Friend>
        {
            new Friend { Id = "1", Name = "Jack", Group = "A" },
            new Friend { Id = "2", Name = "Siri", Group = "B" },
        };
        
        public Droid GetDroid()
        {
            return _droids.First();
        }
        
        public Note GetNotes(string id)
        {
            return _notes.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Friend> GetFriends(string group)
        {
            return _friends.Where(x => x.Group == group);
        }
    }


}
