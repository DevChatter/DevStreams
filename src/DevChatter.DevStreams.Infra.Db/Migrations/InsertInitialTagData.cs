using System.Collections.Generic;
using FluentMigrator;

namespace DevChatter.DevStreams.Infra.Db.Migrations
{
    [Migration(201902220238)]
    public class InsertInitialTagData : Migration
    {
        public override void Up()
        {
            var programmingLanguages = new List<string> { "C#", "JavaScript", "Java", "Python", "Ruby", "Objective-C",
                "C++", "F#", "Rust", "SQL", "C", "TypeScript", "Swift", "Assembly", "Go", "VB.NET", "R",  "Scala",
                "Kotlin", "Groovy", "Perl",
            };
            programmingLanguages.ForEach(x => {
                Insert.IntoTable("Tags").Row(new { Name = x, Description = $"For streams using the {x} programming language." });
            });
            
            var spokenLanguages = new List<string> { "English", "German", "Mandarin", "Spanish", "Hindi", "Arabic",
                "Portuguese", "Russian", "Japanese", "French", "Italian",
            };
            spokenLanguages.ForEach(x => {
                Insert.IntoTable("Tags").Row(new { Name = x, Description = $"For streams where the host speaks in {x}." });
            });
            
            var programmingTypes = new List<string> { "Web Development", "Game Development", "User Experience",
                "Graphic Design", "Windows Applications", "Mac Applications", "Mobile Development",
                "Android Applications", "iOS Applications", "IoT Devices", "Security",
            };
            programmingTypes.ForEach(x => {
                Insert.IntoTable("Tags").Row(new { Name = x, Description = $"For streams focusing on {x}." });
            });
            
            var frameworks = new List<string> { ".NET Core", "ASP.NET Core", "ASP.NET", ".NET Framework", "NodeJS",
                "VueJS", "React", "Angular", "Azure", "AWS", "Docker", "Google Cloud", "Unity", "Xamarin", "WordPress",
                "Ruby on Rails", "WPF", "ASP.NET MVC", "Backbone", "Aurelia", "Django"
            };
            frameworks.ForEach(x => {
                Insert.IntoTable("Tags").Row(new { Name = x, Description = $"For streams using {x}." });
            });

        }

        public override void Down()
        {
            Delete.FromTable("Tags").AllRows();
        }
    }
}